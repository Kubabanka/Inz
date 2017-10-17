using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eVote.Database
{
    public class Poll
    {
        private long id;
        [Key]
        public long ID { get { return id; } set { id = value; } }

        private DateTime startDate=DateTime.Now;
        public DateTime StartDate { get { return startDate; } set { startDate = value; } }

        private string name;
        public string Name { get { return name; } set { name = value; } }

        private DateTime endDate=DateTime.Now;
        public DateTime EndDate { get { return endDate; } set { endDate = value; } }

        private byte[] aesKey;

        public byte[] AesKey { get { return aesKey; } set { aesKey = value; } }

        private byte[] aesIV;

        public byte[] AesIV { get { return aesIV; } set { aesIV = value; } }

        public virtual ICollection<Voter> Voters { get; set; }

        public virtual ICollection<VoteOption> VoteOptions { get; set; }

        public Poll(string name, DateTime start, DateTime end)
        {
            this.name = name;
            this.startDate = start;
            this.endDate = end;
        }

        public  override string ToString()
        {
            return name;
        }

        public Poll() { }

        public void AddVoter(Voter voter)
        {
            Voters.Add(voter);
        }
    }
}
