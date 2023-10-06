using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WelcomePage : System.Web.UI.Page
{
    const string wsUrl = "http://localhost:63420/SensorMonitoringSystemService.svc/rest";
    private static WebClient JsonWebClient = new WebClient()
    {
        Encoding = System.Text.Encoding.UTF8,
        Headers = new WebHeaderCollection()
        {
            { HttpRequestHeader.AcceptCharset, "UTF-8" },
            { "Content-Type", "application/json" }
        } //Every binary valued variable object post needs adding header collection again?
    };
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Activationbtn_Click(object sender, EventArgs e)
    {
        Response.Redirect("ActivationPage.aspx");
    }

    protected void Forgotpassbtn_Click(object sender, EventArgs e)
    {
        Response.Redirect("ForgotPassword.aspx");
    }

    protected void Registerbtn_Click(object sender, EventArgs e)
    {
        Response.Redirect("RegisterPage.aspx");
    }

    protected void Loginbtn_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(Usernametxt.Text) || string.IsNullOrEmpty(Passwordtxt.Text))
        {
            Checklbl.Text = "Please enter your login information.";
        }
        else
        {
            var JsonUsernameExistOrNot = JsonWebClient.DownloadString(wsUrl + "/usernamecontrol/" + Usernametxt.Text);
            var UsernameExistOrNot = JsonHelper.Deserialize<bool>(JsonUsernameExistOrNot);

            if (!UsernameExistOrNot)
            {
                Checklbl.Text = "Username does not exist.";
            }
            else
            {
                var JsonFoundUser = JsonWebClient.DownloadString(wsUrl + "/finduser/" + Usernametxt.Text);
                var FoundUser = JsonHelper.Deserialize<Users>(JsonFoundUser);

                if (FoundUser == null)
                {
                    Checklbl.Text = "Problem occurred. Please try again.";
                }
                else if(!FoundUser.IsActivated)
                {
                    Checklbl.Text = "Account is not activated. Please activate your account throught activation page.";
                }
                else if(!FoundUser.IsApproved)
                {
                    Checklbl.Text = "Account is not approved. Please contact with your system administrator or techexpertmonitoringsystem@gmail.com";
                }
                else
                {
                    string LoginResult;
                    var Username = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(Usernametxt.Text)); // API Receives base64 string value of login information
                    var Password = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(Passwordtxt.Text)); // API Receives base64 string value of login information
                    LoginResult = JsonWebClient.DownloadString(wsUrl + "/login/" + Username + "/" + Password);
                    if(!Convert.ToBoolean(LoginResult)) 
                    {
                        Checklbl.Text = "Password is wrong. Please try again.";
                    }
                    else
                    {
                        Session["username"] = FoundUser.Username;
                        Response.Redirect("ProfilePage.aspx");
                    }
                }
            }
        }
    }
}