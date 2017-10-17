using eVote.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eVote.WebClient
{
    public partial class FinishedPoll : System.Web.UI.Page
    {
        private Poll selectedPoll;
        protected void Page_Load(object sender, EventArgs e)
        {
            selectedPoll = eVote.WebClient.Site.selectedPoll;
            var voteOptList = selectedPoll.VoteOptions.OrderByDescending(x => x.VoteCount).Select(x => string.Format("{0} - {1} votes", x.Name, x.VoteCount)).ToList();

            PollNameLabel.Text = selectedPoll.Name;
            EndDateLabel.Text = "This poll ended on: " + selectedPoll.EndDate.ToString();
            PollOptionsListView.DataSource = voteOptList;
            PollOptionsListView.DataBind();

        }
    }
}