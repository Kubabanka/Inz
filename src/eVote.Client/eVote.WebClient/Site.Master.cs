using eVote.Database;
using eVote.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eVote.WebClient
{
    public partial class Site : System.Web.UI.MasterPage
    {
        public static Poll selectedPoll { get; set; }
        //public static Client.Client client = new Client.Client(); 
        private eVoteModel db;
        protected void Page_Load(object sender, EventArgs e)
        {
            //var l = new List<string>() { "OP1", "OP2", "OP3", "OP4" };
            db = new eVoteModel();
            var voter = db.Voters.Where(v => v.Login == HttpContext.Current.User.Identity.Name).FirstOrDefault();
            var pollList = voter.Polls.ToList();
            ListView1.DataSource = pollList;
            ListView1.DataBind();
        }

        protected void LogoutButton_Click( object sender, EventArgs e )
        {
            FormsAuthentication.SignOut();
            Response.Redirect( "WelcomePage.aspx" );
        }


        protected void ListView1_SelectedIndexChanging( object sender, ListViewSelectEventArgs e )
        {
            ListView1.SelectedIndex = e.NewSelectedIndex;
            ListView1.DataBind();

        }

        protected void ListView1_SelectedIndexChanged( object sender, EventArgs e )
        {
            var t = (long) ListView1.SelectedDataKey.Value;
            selectedPoll = db.Polls.Where( p => p.ID == t ).FirstOrDefault();
            if (selectedPoll.EndDate<DateTime.Now)
            {
                Response.Redirect("FinishedPoll.aspx");
            }
            else
            {
                Response.Redirect("PollPage.aspx");
            }
            
        }

        protected void AddNewPollButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("AddNewPoll.aspx");
        }
    }
}