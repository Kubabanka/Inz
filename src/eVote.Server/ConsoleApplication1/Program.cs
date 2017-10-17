using eVote.Database;
using eVote.Messages;
using eVote.Server;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            DatabaseServer server = new DatabaseServer(5454);

            server.Initialize();
            //var db = new eVoteModel();

            //Stopwatch stopwatch = new Stopwatch();


            //Console.WriteLine(DateTime.Now);
            //stopwatch.Start();

            //VoteCounter c = new VoteCounter(db.Polls.First());
            //c.CountVotes();

            //stopwatch.Stop();
            //Console.WriteLine(stopwatch.Elapsed);
            //Console.WriteLine(DateTime.Now);

            while (Console.ReadLine() != "q")
            { }
        }
    }
}
