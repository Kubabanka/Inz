using eVote.Messages;
using eVote.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.IO;

namespace eVote.Client
{
    public static class Client
    {
        private static AesManaged aes = new AesManaged();
        public static bool Login(string login, string pass)
        {
            KeyExchange();
            var client = new TcpClient("localhost", 5454);
            var voter = new Voter();
            voter.Login = login;
            voter.Password = Message.SHA512(pass);
            var voterStr = JsonConvert.SerializeObject(Message.EncryptStringToBytes_Aes(JsonConvert.SerializeObject(voter), aes.Key, aes.IV));
            var mes = new Message(voterStr, "Voter login", "Client", "Server");
            var mesString = JsonConvert.SerializeObject(mes);
            Message replyMessage;
            using (var stream = client.GetStream())
            {
                var writer = new BinaryWriter(stream);
                writer.Write(mesString);
                var reader = new BinaryReader(stream);
                var reply = reader.ReadString();
                reply = Message.DecryptStringFromBytes_Aes(JsonConvert.DeserializeObject<byte[]>(reply), aes.Key, aes.IV);
                replyMessage = JsonConvert.DeserializeObject<Message>(reply);
            }

            if (replyMessage.Subject == "User is valid")
                return true;

            return false;
        }

        public static bool AddNewVoter(string login, string pass, string fname, string lname)
        {
            KeyExchange();
            var client = new TcpClient("localhost", 5454);
            var voter = new Voter() { FirstName = fname, LastName = lname, Login = login, Password = Message.SHA512(pass) };
            var votrStr = JsonConvert.SerializeObject(Message.EncryptStringToBytes_Aes(JsonConvert.SerializeObject(voter), aes.Key, aes.IV));
            var mes = new Message(votrStr, "New voter", "Client", "Server");
            var mesString = JsonConvert.SerializeObject(mes);
            
            Message replyMessage;
            using (var stream = client.GetStream())
            {
                var writer = new BinaryWriter(stream);
                writer.Write(mesString);
                var reader = new BinaryReader(stream);
                var reply = reader.ReadString();
                replyMessage = (Message)JsonConvert.DeserializeObject<Message>(reply);
            }

            if (replyMessage.Subject == "User created")
            {
                return true;
            }
            return false;
        }

        public static void CastVote(string login, long[] choice, long pollID)
        {
            var client = new TcpClient("localhost", 5454);
            var mes = new Message(JsonConvert.SerializeObject(Message.EncryptStringToBytes_Aes(pollID.ToString(), aes.Key, aes.IV)), "Get Poll Keys", "Client", "Server");
            var mesString = JsonConvert.SerializeObject(mes);

            using (var stream = client.GetStream())
            {
                var writer = new BinaryWriter(stream);
                writer.Write(mesString);
                var reader = new BinaryReader(stream);
                var reply = reader.ReadString();
                reply =Message.DecryptStringFromBytes_Aes(JsonConvert.DeserializeObject<byte[]>(reply), aes.Key, aes.IV);
                var replyMessage = JsonConvert.DeserializeObject<Message>(reply);
                var onlyKeys = JsonConvert.DeserializeObject<Poll>(replyMessage.Data);
                var encChoice = Message.EncryptStringToBytes_Aes(JsonConvert.SerializeObject(choice), onlyKeys.AesKey, onlyKeys.AesIV);
                var encVoter = Message.EncryptStringToBytes_Aes(login, onlyKeys.AesKey, onlyKeys.AesIV);
                var v = new Vote(encVoter, encChoice, pollID);
                var data = Message.EncryptStringToBytes_Aes(JsonConvert.SerializeObject(v), aes.Key, aes.IV);
                mes = new Message(JsonConvert.SerializeObject(data), "Add new vote", "Client", "Server");
                mesString = JsonConvert.SerializeObject(mes);
            }
            client.Close();
            client = new TcpClient("localhost", 5454);
            using (var s = client.GetStream())
            {
                var writer = new BinaryWriter(s);
                writer.Write(mesString);
            }
        }

        public static string GetLastVote(long pollID, string usersLogin)
        {
            var client = new TcpClient("localhost", 5454);
            eVoteModel db = new eVoteModel();

            var mes = new Message(JsonConvert.SerializeObject(Message.EncryptStringToBytes_Aes(String.Format("{0}\n{1}", pollID, usersLogin), aes.Key, aes.IV)), "Last vote", "Client", "Server");
            var mesString = JsonConvert.SerializeObject(mes);
            Message replyMessage;
            using (var stream = client.GetStream())
            {
                var writer = new BinaryWriter(stream);
                writer.Write(mesString);
                var reader = new BinaryReader(stream);
                var reply = reader.ReadString();
                reply = Message.DecryptStringFromBytes_Aes(JsonConvert.DeserializeObject<byte[]>(reply),aes.Key,aes.IV);
                replyMessage = (Message)JsonConvert.DeserializeObject<Message>(reply);
            }

            return replyMessage.Data;
        }

        public static void KeyExchange()
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
            var aesKey = Message.DecryptStringFromBytes_RSA(JsonConvert.DeserializeObject<byte[]>(aesKeyAndIV[0]), rsa.ExportParameters(true));
            var aesIV = Message.DecryptStringFromBytes_RSA(JsonConvert.DeserializeObject<byte[]>(aesKeyAndIV[1]), rsa.ExportParameters(true));
            aes.Key = JsonConvert.DeserializeObject<byte[]>(aesKey);
            aes.IV = JsonConvert.DeserializeObject<byte[]>(aesIV);
        }

        public static void AddNewPoll(Poll newPoll, List<string> votersMails)
        {
            var pollStr = JsonConvert.SerializeObject(newPoll);
            var voterStr = JsonConvert.SerializeObject(votersMails);
            var toSer = new string[] { pollStr, voterStr };
            var k =JsonConvert.SerializeObject(Message.EncryptStringToBytes_Aes(JsonConvert.SerializeObject(toSer),aes.Key,aes.IV));
            var client = new TcpClient("localhost", 5454);
            using (var stream = client.GetStream())
            {
                var mes = new Message(k, "New poll", "Client", "Server");
                var mesString = JsonConvert.SerializeObject(mes);
                var writer = new BinaryWriter(stream);
                writer.Write(mesString);

            }
        }

    }
}
