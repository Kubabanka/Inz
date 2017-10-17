using eVote.Database;
using eVote.Messages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace eVote.PublishElement
{
    public class VoteCounter
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

        private AesManaged aes = new AesManaged();
        public VoteCounter(int port)
        {
            listener = new TcpListener(new IPEndPoint(IPAddress.Any, port));
            database = new eVoteModel();
            syncRoot = new object();
        }

        public bool CountVotes(Poll pollToCount)
        {
            var votesToCount = database.Votes.Where(x => x.PollID == pollToCount.ID).ToList();
            foreach (var voter in pollToCount.Voters)
            {
                var encVoter = Messages.Message.EncryptStringToBytes_Aes(voter.Login, pollToCount.AesKey, pollToCount.AesIV);
                var voterVotes = votesToCount.Where(x => x.EncryptedVoter.SequenceEqual(encVoter)).ToList();
                if (voterVotes.Count == 0)
                {
                    continue;
                }
                var maxDate = voterVotes.Max(x => x.VoteTime);
                var vote = votesToCount.Where(x => x.VoteTime == maxDate).FirstOrDefault();
                var choiceStr = Messages.Message.DecryptStringFromBytes_Aes(vote.Choice, pollToCount.AesKey, pollToCount.AesIV);
                var choice = (long[])JsonConvert.DeserializeObject<long[]>(choiceStr);
                var ch = choice[0];
                var choiceOneOption = database.VoteOptions.SingleOrDefault(x => x.ID == ch);
                VoteOption choiceTwoOption;
                VoteOption choiceThreeOption;
                choiceOneOption.VoteCount += 5;
                if (choice[1] > 0)
                {
                    ch = choice[1];
                    choiceTwoOption = database.VoteOptions.SingleOrDefault(x => x.ID == ch);
                    choiceTwoOption.VoteCount += 3;
                }
                if (choice[2] > 0)
                {
                    ch = choice[2];
                    choiceThreeOption = database.VoteOptions.SingleOrDefault(x => x.ID == ch);
                    choiceThreeOption.VoteCount += 1;
                }
                database.SaveChanges();
            }
            return true;
        }

        public void Initialize()
        {
            KeyExchange();
            listener.Start();
            while (true)
            {
                var client = listener.AcceptTcpClient();
                var clientThread = new Thread(new ParameterizedThreadStart(HandleConnection));
                clientThread.IsBackground = true;
                clientThread.Start(client);
            }
        }

        public void KeyExchange()
        {
            var client = new TcpClient("localhost", 5454);
            var rsa = new RSACryptoServiceProvider();
            string mes;

            using (var stream = client.GetStream())
            {
                using (var reader = new BinaryReader(stream))
                {
                    using (var writer = new BinaryWriter(stream))
                    {
                        var mess = new Message("", "Key exchange", "Client", "Server");
                        writer.Write(JsonConvert.SerializeObject(mess));
                        writer.Write(rsa.ToXmlString(false));
                        mes = reader.ReadString();
                    }
                }
            }

            var aesKeyAndIV = JsonConvert.DeserializeObject<string[]>(JsonConvert.DeserializeObject<Message>(mes).Data);
            aes.Key = JsonConvert.DeserializeObject<byte[]>(aesKeyAndIV[0]);
            aes.IV = JsonConvert.DeserializeObject<byte[]>(aesKeyAndIV[1]);
        }

        private void HandleConnection(object obj)
        {
            var client = (TcpClient)obj;
            var stream = client.GetStream();
            var reader = new BinaryReader(stream);
            string serializedMessage = reader.ReadString();
            var message = JsonConvert.DeserializeObject<Message>(serializedMessage);
            switch (message.Subject)
            {

                case "Poll finished":
                    var p = JsonConvert.DeserializeObject<Poll>(message.Data);
                    CountVotes(p);
                    break;
                default:
                    Console.WriteLine("Unrecognizable subject!");
                    break;
            }

        }

    }
}
