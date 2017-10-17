using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eVote.WebClient
{
    public partial class RegisterPage : System.Web.UI.Page
    {
        protected void Page_Load( object sender, EventArgs e )
        {

        }

        protected void RegisterButton_Click( object sender, EventArgs e )
        {
            if (Client.Client.AddNewVoter(this.LoginTextbox.Text,Password1TextBox.Text,FirstNameTextBox.Text,LastNameTextbox.Text))
            {
                FormsAuthentication.RedirectFromLoginPage(LoginTextbox.Text, false);
            }
            else
            {
                this.ErrorLabel.Text = "User exists";
            }
        }

        protected void CancelButton_Click( object sender, EventArgs e )
        {
            Response.Redirect( "WelcomePAge.aspx", true );
        }
    }
}