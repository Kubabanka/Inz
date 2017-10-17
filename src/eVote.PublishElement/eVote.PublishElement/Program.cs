using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eVote.PublishElement
{
    class Program
    {
        static void Main(string[] args)
        {
            var vc = new VoteCounter(5888);
            vc.Initialize();
        }
    }
}
