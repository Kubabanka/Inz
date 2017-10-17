using eVote.Database;
using eVote.Messages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eVote.WebClient
{
    public partial class PollPage : System.Web.UI.Page
    {
        private Poll selectedPoll;
        protected void Page_Load( object sender, EventArgs e )
        {
            
            selectedPoll = eVote.WebClient.Site.selectedPoll;
            LastVoteLabel.Text = Client.Client.GetLastVote(selectedPoll.ID, HttpContext.Current.User.Identity.Name);
            var voteOptList = selectedPoll.VoteOptions.ToList();

            PollNameLabel.Text = selectedPoll.Name;
            EndDateLabel.Text = "Ends on: " + selectedPoll.EndDate.ToString();
            PollOptionsListView.DataSource = voteOptList;
            PollOptionsListView.DataBind();
            if (FirstChoiceDropDownList.Items.Count <2)
            {
                FirstChoiceDropDownList.DataSource = voteOptList; 
            }
            
            if (SecondChoiceDropDownList.Items.Count < 2)
            {
                SecondChoiceDropDownList.DataSource = voteOptList;
            } 
            
            if (ThirdChoiceDropDownList.Items.Count < 2)
            {
                ThirdChoiceDropDownList.DataSource = voteOptList;
            }

            FirstChoiceDropDownList.DataBind();
            SecondChoiceDropDownList.DataBind();
            ThirdChoiceDropDownList.DataBind();
        }

        protected void VoteButton_Click( object sender, EventArgs e )
        {
            CheckLabel.Text = "";

            if (FirstChoiceDropDownList.SelectedValue == "-1")
            {
                CheckLabel.Text = "First choice cannot be empty!";
                return;
            }

            if (FirstChoiceDropDownList.SelectedValue == SecondChoiceDropDownList.SelectedValue || FirstChoiceDropDownList.SelectedValue == ThirdChoiceDropDownList.SelectedValue || ThirdChoiceDropDownList.SelectedValue == SecondChoiceDropDownList.SelectedValue)
            {
                if (SecondChoiceDropDownList.SelectedValue != "-1" || ThirdChoiceDropDownList.SelectedValue != "-1")
                {
                    CheckLabel.Text = "Choices must be different!";
                    return;
                }
            }

            var choice = new long[] { Convert.ToInt64(FirstChoiceDropDownList.SelectedValue), Convert.ToInt64(SecondChoiceDropDownList.SelectedValue), Convert.ToInt64(ThirdChoiceDropDownList.SelectedValue) };

            Client.Client.CastVote(HttpContext.Current.User.Identity.Name,choice,selectedPoll.ID);
            LastVoteLabel.Text = Client.Client.GetLastVote(selectedPoll.ID, HttpContext.Current.User.Identity.Name);
        }

    }
}