using eVote.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eVote.WebClient
{
    public partial class AddNewPoll : System.Web.UI.Page
    {
        eVoteModel db = new eVoteModel();
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }

        protected void AddButton_Click(object sender, EventArgs e)
        {
            ErrorLabel.Text = "";
            DateTime endDate;
            try
            {
                endDate = DateTime.Parse(EndDateTextbox.Text);
            }
            catch
            {
                ErrorLabel.Text += "Bad date format.";
                return;
            }
            var votersEmails = VotersTextbox.Text.Split(new char[] { '\n', ';' }).Select(x => x.Trim()).Distinct();
            var voteOpt = OptionTextbox.Text.Split(new char[] { '\n', ';' }).Select(x => x.Trim()).Distinct();

            var voters = new List<Voter>();
            var voteOptions = new List<VoteOption>();

            var p = new Poll();

            foreach (var login in votersEmails)
            {
                if (!String.IsNullOrWhiteSpace(login))
                {
                    var v = db.Voters.Where(x => x.Login == login.Trim()).FirstOrDefault();
                    if(v != null)
                    {
                        voters.Add(v);
                    }
                }
            }

            foreach (var opt in voteOpt)
            {
                if (!String.IsNullOrWhiteSpace(opt))
                {
                    var vo = new VoteOption(opt.Trim());

                    voteOptions.Add(vo);
                }
            }
            p.Name = PollNameTextbox.Text.Trim();
            p.EndDate = endDate;
            p.VoteOptions = voteOptions;
            p.Voters = new List<Voter>();
            Client.Client.AddNewPoll(p,voters.Select(v => v.Login).ToList());
            
            Response.Redirect("default.aspx", true);
        }
    }
}