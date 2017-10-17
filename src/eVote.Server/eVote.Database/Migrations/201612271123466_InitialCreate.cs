namespace eVote.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Polls",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        StartDate = c.DateTime(nullable: false),
                        Name = c.String(),
                        EndDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Voters",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Login = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Votes",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        Choice = c.Binary(),
                        VoteTime = c.DateTime(nullable: false),
                        PollID = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Polls", t => t.PollID, cascadeDelete: true)
                .Index(t => t.PollID);
            
            CreateTable(
                "dbo.VoterPolls",
                c => new
                    {
                        Voter_ID = c.Long(nullable: false),
                        Poll_ID = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.Voter_ID, t.Poll_ID })
                .ForeignKey("dbo.Voters", t => t.Voter_ID, cascadeDelete: true)
                .ForeignKey("dbo.Polls", t => t.Poll_ID, cascadeDelete: true)
                .Index(t => t.Voter_ID)
                .Index(t => t.Poll_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Votes", "PollID", "dbo.Polls");
            DropForeignKey("dbo.VoterPolls", "Poll_ID", "dbo.Polls");
            DropForeignKey("dbo.VoterPolls", "Voter_ID", "dbo.Voters");
            DropIndex("dbo.VoterPolls", new[] { "Poll_ID" });
            DropIndex("dbo.VoterPolls", new[] { "Voter_ID" });
            DropIndex("dbo.Votes", new[] { "PollID" });
            DropTable("dbo.VoterPolls");
            DropTable("dbo.Votes");
            DropTable("dbo.Voters");
            DropTable("dbo.Polls");
        }
    }
}
