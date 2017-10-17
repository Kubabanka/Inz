using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eVote.Database
{
    public class Vote
    {
        private long id;
        [Key]
        public long ID { get { return id; } set { id = value; } }

        private byte[] choice;
        public byte[] Choice { get { return choice; } set { choice = value; } }

        private DateTime voteTime = DateTime.Now;
        public DateTime VoteTime { get { return voteTime; } set { voteTime = value; } }

        private Poll poll;
        [ForeignKey("PollID")]
        public Poll Poll { get { return poll; } set { poll = value; } }

        public long PollID { get; set; }

        private byte[] encryptedVoter;

        public byte[] EncryptedVoter { get { return encryptedVoter; } set { encryptedVoter = value; } }

        public Vote () { }
        public Vote (byte[] voter, byte[] choice, long poll)
        {
            this.encryptedVoter = voter;
            this.choice = choice;
            this.PollID = poll;
            this.voteTime = DateTime.Now;
        }
    }
}
