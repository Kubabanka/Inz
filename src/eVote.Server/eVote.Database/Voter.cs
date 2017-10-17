using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace eVote.Database
{
    public class Voter
    {
        private long id;
        [Key]
        public long ID { get { return id; } set { id = value; } }

        private string firstName;
        public string FirstName { get { return firstName; } set { firstName = value; } }

        private string lastName;
        public string LastName { get { return lastName; } set { lastName = value; } }

        private string login;
        public string Login { get { return login; } set { login = value; } }

        private string password;
        public string Password { get { return password; } set { password = value; } }

        private string keys;
        public string Keys { get { return keys; } set { keys = value; } }

        public virtual ICollection<Poll> Polls { get; set; }
    }
}
