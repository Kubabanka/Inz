using eVote.Database;
using eVote.Messages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Timers;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace eVote.Server
{
    public class DatabaseServer
    {
        /// <summary>
        /// Listrner portu TCP do komunikacji z klientem.
        /// </summary>
        private TcpListener listener;

        /// <summary>
        /// Objekt bazy danych.
        /// </summary>
        public eVoteModel database;
        
        /// <summary>
        /// Objekt synchronizujący wątki.
        /// </summary>
        private object syncRoot;

        /// <summary>
        /// Lista Timerów do obsługi głosowań.
        /// </summary>
        private List<System.Timers.Timer> pollsTimers;

        private RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();

        private AesManaged aes = new AesManaged();
        public DatabaseServer(int port)
        {
            listener = new TcpListener(new IPEndPoint(IPAddress.Any, port));
            database = new eVoteModel();
            syncRoot = new object();
            pollsTimers = new List<System.Timers.Timer>();
        }

        public void Initialize()
        {
            listener.Start();
            while(true)
            {
                var client = listener.AcceptTcpClient();
                var clientThread = new Thread(new ParameterizedThreadStart(HandleConnection));
                clientThread.IsBackground = true;
                clientThread.Start(client);
            }
        }

        public void AddNewPoll(Poll p, List<string> mails)
        {
            foreach (var login in mails)
            {
                if (!String.IsNullOrWhiteSpace(login))
                {
                    var v = database.Voters.Where(x => x.Login == login.Trim()).FirstOrDefault();
                    if (v != null)
                    {
                        p.Voters.Add(v);
                    }
                }
            }

            var timeToEnd = p.EndDate - DateTime.Now;
            if (timeToEnd <= TimeSpan.Zero)
            {
                return;
            }
            else
            {
                var timer = new System.Timers.Timer(Convert.ToInt64(timeToEnd.TotalMilliseconds));
                pollsTimers.Add(timer);
                timer.AutoReset = false;
                timer.Elapsed += ( sender, e ) => PollFinished(p);
                timer.Start();
            }
            AesManaged aes = new AesManaged();
            aes.GenerateIV();
            aes.GenerateKey();
            p.AesIV = aes.IV;
            p.AesKey = aes.Key;
            EncryptPollKey(p, JsonConvert.DeserializeObject<byte[]>("\"f3em5GbGPXYrSuakqSuxIDk31JKoJQNBDzsHqA60gkE=\""), JsonConvert.DeserializeObject<byte[]>("\"7Pe/z8mtaiUD7wa/Mdw2gw==\""));
            database.Polls.Add(p);
            database.SaveChanges();
        }

        private void PollFinished(Poll p)
        {
            var client = new TcpClient("localhost", 5888);
            var pollToSend = DecryptPollKey(p, JsonConvert.DeserializeObject<byte[]>("\"f3em5GbGPXYrSuakqSuxIDk31JKoJQNBDzsHqA60gkE=\""), JsonConvert.DeserializeObject<byte[]>("\"7Pe/z8mtaiUD7wa/Mdw2gw==\""));
            var mes = new Message(JsonConvert.SerializeObject(pollToSend), "Poll finished", "Server", "Publish element");
            var mesStr = JsonConvert.SerializeObject(mes);
            var writer = new BinaryWriter(client.GetStream());
            writer.Write(mesStr);
            writer.Close();
            client.Close();
        }

        public static void EncryptPollKey(Poll p, byte[] key, byte[] iv)
        {
            var pk = Convert.ToBase64String(p.AesKey);
            var piv = Convert.ToBase64String(p.AesIV);
            p.AesKey = Message.EncryptStringToBytes_Aes(pk, key, iv);
            p.AesIV = Message.EncryptStringToBytes_Aes(piv, key, iv);

        }

        public static Poll DecryptPollKey(Poll p, byte[] key, byte[] iv)
        {
            var tmp1 = Message.DecryptStringFromBytes_Aes(p.AesKey, key, iv);
            var tmp2 = Message.DecryptStringFromBytes_Aes(p.AesIV, key, iv);
            var pk = Convert.FromBase64String(tmp1);
            var piv = Convert.FromBase64String(tmp2);
            return new Poll() { AesKey = pk, AesIV = piv };
        }

        private void HandleConnection(object obj)
        {
            var client = (TcpClient)obj;
            var stream = client.GetStream();
            var reader = new BinaryReader(stream);
            string serializedMessage= reader.ReadString();
            var message = JsonConvert.DeserializeObject<Message>(serializedMessage);
            var type = message.GetType();
            if (message.GetType() == typeof(Message))
            {
                switch(message.Subject)
                {

                    case "Key exchange":
                        KeyExchange(client);
                        break;
                    case "Voter login":
                        var data = Message.DecryptStringFromBytes_Aes(JsonConvert.DeserializeObject<byte[]>(message.Data), aes.Key, aes.IV);
                        var voter = JsonConvert.DeserializeObject<Voter>(data);
                        VoterLogin(voter, client);
                        break;
                    case "New voter":
                        var data1 = Message.DecryptStringFromBytes_Aes(JsonConvert.DeserializeObject<byte[]>(message.Data), aes.Key, aes.IV);
                        var newVoter = JsonConvert.DeserializeObject<Voter>(data1);
                        newVoter.ID = database.Voters.Max(v => v.ID) + 1;
                        AddNewVoter(newVoter, client);
                        break;
                    case "New poll":
                        var data2 = Message.DecryptStringFromBytes_Aes(JsonConvert.DeserializeObject<byte[]>(message.Data), aes.Key, aes.IV);
                        var des = JsonConvert.DeserializeObject<string[]>(data2);
                        var poll = JsonConvert.DeserializeObject<Poll>(des[0]);
                        var mails = JsonConvert.DeserializeObject<List<string>>(des[1]);
                        AddNewPoll(poll,mails);
                        break;
                    case "Last vote":
                        var data3 = Message.DecryptStringFromBytes_Aes(JsonConvert.DeserializeObject<byte[]>(message.Data), aes.Key, aes.IV);
                        GetLastVote(data3, client);
                        break;
                    case "Get Poll Keys":
                        var data4 = Message.DecryptStringFromBytes_Aes(JsonConvert.DeserializeObject<byte[]>(message.Data), aes.Key, aes.IV);
                        var pollID = Convert.ToInt64(data4);
                        GetPollKeys(pollID, client);
                        break;
                    case "Add new vote":
                        var data5 = Message.DecryptStringFromBytes_Aes(JsonConvert.DeserializeObject<byte[]>(message.Data), aes.Key, aes.IV);
                        var newVote = JsonConvert.DeserializeObject<Vote>(data5);
                        AddNewVote(newVote);
                        break;
                    default:
                        Console.WriteLine("Unrecognizable subject!");
                        break;
                }
            }
        }

        private void KeyExchange(TcpClient client)
        {
            using (var stream = client.GetStream())
            {
                using (var reader = new BinaryReader(stream))
                {
                    using (var writer = new BinaryWriter(stream))
                    {
                        var clientRsaKey = reader.ReadString();
                        var clientRsa = new RSACryptoServiceProvider();
                        clientRsa.FromXmlString(clientRsaKey);
                        var aesKeyString = JsonConvert.SerializeObject(Message.EncryptStringToBytes_RSA(JsonConvert.SerializeObject(aes.Key), clientRsa.ExportParameters(false)));
                        var aesIVString = JsonConvert.SerializeObject(Message.EncryptStringToBytes_RSA(JsonConvert.SerializeObject(aes.IV), clientRsa.ExportParameters(false)));
                        var data = new string[] { aesKeyString, aesIVString };
                        var m = JsonConvert.SerializeObject(new Message(JsonConvert.SerializeObject(data), "AesKeys", "server", "client/publish element"));
                        writer.Write(m);
                    }
                }
            }
        }

        private void GetPollKeys(long pollID, TcpClient client)
        {
            var p = database.Polls.Where(x => x.ID == pollID).First();
            var p1 = DecryptPollKey(p, JsonConvert.DeserializeObject<byte[]>("\"f3em5GbGPXYrSuakqSuxIDk31JKoJQNBDzsHqA60gkE=\""), JsonConvert.DeserializeObject<byte[]>("\"7Pe/z8mtaiUD7wa/Mdw2gw==\""));
            var mes = new Message(JsonConvert.SerializeObject(p1), "Poll key", "Server", "Client");
            var mesStr = JsonConvert.SerializeObject(mes);
            var writer = new BinaryWriter(client.GetStream());
            mesStr = JsonConvert.SerializeObject(Message.EncryptStringToBytes_Aes(mesStr, aes.Key, aes.IV));
            writer.Write(mesStr);
            writer.Close();
            client.Close();

        }

        private void GetLastVote( string data, TcpClient client)
        {
            Message mes;
            var d = data.Split( '\n' );
            var pollID = Convert.ToInt64( d[0]);
            var userLogin = d[1];
            var votes = database.Votes.Where(v => v.PollID == pollID).ToList();
            var p = database.Polls.Where(x => x.ID == pollID).FirstOrDefault();
            var keys = DecryptPollKey(p, JsonConvert.DeserializeObject<byte[]>("\"f3em5GbGPXYrSuakqSuxIDk31JKoJQNBDzsHqA60gkE=\""), JsonConvert.DeserializeObject<byte[]>("\"7Pe/z8mtaiUD7wa/Mdw2gw==\""));
            var encVoter = Messages.Message.EncryptStringToBytes_Aes(userLogin, keys.AesKey, keys.AesIV);
            var voterVotes = votes.Where(x => x.EncryptedVoter.SequenceEqual(encVoter)).ToList();



            if (voterVotes.Count == 0)
            {
                mes = new Message("You did not vote in this poll.", "No vote", "Server", "Client");
            }
            else
            {
                var maxDate = voterVotes.Max(x => x.VoteTime);
                var lastVote = votes.Where(x => x.VoteTime == maxDate).FirstOrDefault();
                var choiceStr = Message.DecryptStringFromBytes_Aes(lastVote.Choice, keys.AesKey, keys.AesIV);
                var choice = JsonConvert.DeserializeObject<long[]>(choiceStr);

                var options = new string[3];

                for (int i = 0; i < 3; i++)
                {
                    if (choice[i] > 0)
                    { options[i] = p.VoteOptions.Where(c => c.ID == choice[i]).Select(c => c.Name).First(); }
                    else
                    { options[i] = "not chosen"; }
                }

                var t = String.Format("Last vote in this poll was cast on {0}. Your choices:\n1.{1}\n2.{2}\n3.{3}", lastVote.VoteTime.ToString(), options[0], options[1], options[2]);
                mes = new Message(t, "Last Vote", "Server", "Client");
            }

            var mesStr = JsonConvert.SerializeObject(mes);
            mesStr = JsonConvert.SerializeObject(Message.EncryptStringToBytes_Aes(mesStr, aes.Key, aes.IV));

            var writer = new BinaryWriter(client.GetStream());
            writer.Write(mesStr);
            writer.Close();
            client.Close();
        }

        private void AddNewVote(Vote vote)
        {
            var p = database.Polls.Where(r => r.ID == vote.PollID).Select(r => r.EndDate).FirstOrDefault();
            if (p < vote.VoteTime)
                return;
            database.Votes.Add(vote);
            database.SaveChanges();
        }

        private void VoterLogin(Voter logger, TcpClient client)
        {
            string mesStr;
            try
            {
                 var voter = database.Voters.Where(x => x.Login == logger.Login).First();
                if (voter.Password == logger.Password)
                {
                    var mes = new Message(voter.Keys, "User is valid", "Server", "Client");
                    mesStr = JsonConvert.SerializeObject(mes);
                }
                else
                {
                    var mes = new Message("Authentication failed.", "User is not valid", "Server", "Client");
                    mesStr = JsonConvert.SerializeObject(mes);
                }
            }
            catch (Exception)
            {
                var mes = new Message("Authentication failed.", "No user found", "Server", "Client");
                mesStr = JsonConvert.SerializeObject(mes);
            }

            mesStr = JsonConvert.SerializeObject(Message.EncryptStringToBytes_Aes(mesStr, aes.Key, aes.IV));

            var writer = new BinaryWriter( client.GetStream() );
            writer.Write( mesStr );
            writer.Close();
            client.Close();
        }

        private void AddNewVoter(Voter voter, TcpClient client)
        {
            string mesStr;
            if (database.Voters.Select(x => x.Login).Contains(voter.Login))
            {
                mesStr = JsonConvert.SerializeObject(new Message("", "User exists", "Server", "Client"));
            }
            else
            {
                
                voter.Keys = new RSACryptoServiceProvider().ToXmlString(true);
                mesStr = JsonConvert.SerializeObject(new Message(voter.Keys, "User created", "Server", "Client"));
                database.Voters.Add(voter);
                database.SaveChanges();
            }


            var writer = new BinaryWriter(client.GetStream());
            writer.Write(mesStr);
            writer.Close();
            client.Close();
        }
    }
}
