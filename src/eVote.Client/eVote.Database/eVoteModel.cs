namespace eVote.Database
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class eVoteModel : DbContext
    {
        // Your context has been configured to use a 'eVoteModel' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'eVote.Database.eVoteModel' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'eVoteModel' 
        // connection string in the application configuration file.
        public eVoteModel()
            : base("name=eVoteDatabase")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<Poll> Polls { get; set; }
        public virtual DbSet<Vote> Votes { get; set; }
        public virtual DbSet<Voter> Voters { get; set; }
        public virtual DbSet<VoteOption> VoteOptions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);           
        }

    }

}