using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eVote.WebClient
{
    public partial class LogonPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Login_Click(object sender, EventArgs e)
        {

            if (eVote.Client.Client.Login(LoginTextbox.Text, PasswordTextBox.Text))
            {
                FormsAuthentication.RedirectFromLoginPage
                   (LoginTextbox.Text, PersistCheckbox.Checked);
            }
            else
            {
                InvalidCredentialsLabel.Text = "Invalid credentials. Please try again.";
            }
        }

    }
}