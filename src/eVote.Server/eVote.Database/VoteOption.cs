using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eVote.Database
{
    public class VoteOption
    {
        private long id;
        [Key]
        public long ID { get { return id; } set { id = value; } }

        private string name;
        public string Name { get { return name; } set { name = value; } }

        private long voteCount;
        public long VoteCount { get { return voteCount; } set { voteCount = value; } }

        [ForeignKey("PollID")]
        public virtual Poll Poll { get; set; }

        public long PollID { get; set; }

        public VoteOption () { }
        public VoteOption (string name)
        {
            this.name = name;
        }

        public override string ToString()
        {
            return name;
        }
    }
}
