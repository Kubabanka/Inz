using eVote.Database;
using eVote.Messages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace testConsole
{
    class Program
    {
        static void Main( string[] args )
        {
            RSACryptoServiceProvider key = new RSACryptoServiceProvider();

            var et = key.ExportParameters( true );
            var ef = key.ExportParameters( false );

            var t = JsonConvert.SerializeObject(et);
            var f = JsonConvert.SerializeObject(ef);

            Console.WriteLine();
        }
    }
}
