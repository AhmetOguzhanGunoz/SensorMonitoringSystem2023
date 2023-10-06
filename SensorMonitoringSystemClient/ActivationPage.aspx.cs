using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ActivationPage : System.Web.UI.Page
{
    const string wsUrl = "http://localhost:63420/SensorMonitoringSystemService.svc/rest";
    private static WebClient JsonWebClient = new WebClient()
    {
        Encoding = System.Text.Encoding.UTF8,
        Headers = new WebHeaderCollection()
        {
            { HttpRequestHeader.AcceptCharset, "UTF-8" },
            { "Content-Type", "application/json" }
        }//Every binary valued variable object post needs adding header collection again?
    };
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Activationbtn_Click(object sender, EventArgs e)
    {
        if(string.IsNullOrEmpty(Codetxt.Text))
        {
            Activationlbl.Text = "Please enter account registration code for activation.";
        }
        else 
        {
            var JsonUsernameExistOrNot = JsonWebClient.DownloadString(wsUrl + "/usernamecontrol/" + UsernameRgstxt.Text);
            var UsernameExistOrNot = JsonHelper.Deserialize<bool>(JsonUsernameExistOrNot);

            if (!UsernameExistOrNot)
            {
                Activationlbl.Text = "Username does not exist.";
            }
            else
            {

                var JsonFoundUser = JsonWebClient.DownloadString(wsUrl + "/finduser/" + UsernameRgstxt.Text);
                var FoundUser = JsonHelper.Deserialize<Users>(JsonFoundUser);

                if (Codetxt.Text != FoundUser.RegistrationCode.ToString())
                {
                    Activationlbl.Text = "Activation code is wrong. Please try again.";
                }
                else if (FoundUser.IsActivated)
                {
                    Activationlbl.Text = "Account is activated already.";
                }
                else
                {
                    string PostResult = string.Empty;
                    JsonWebClient.Headers.Add(HttpRequestHeader.AcceptCharset, "UTF-8");
                    JsonWebClient.Headers["Content-type"] = "application/json";
                    PostResult = JsonWebClient.UploadString(wsUrl + "/activation", JsonFoundUser);

                    if (!Convert.ToBoolean(int.Parse(PostResult)))
                    {
                        Activationlbl.Text = "Account is not activated. Please try again.";
                    }
                    else
                    {
                        Activationlbl.Text = "Account is activated successfully";
                    }
                }
            }
        }       
    }

    protected void Resendbtn_Click1(object sender, EventArgs e)
    {
        var JsonUsernameExistOrNot = JsonWebClient.DownloadString(wsUrl + "/usernamecontrol/" + UsernameRgstxt.Text);
        var UsernameExistOrNot = JsonHelper.Deserialize<bool>(JsonUsernameExistOrNot);

        if (!UsernameExistOrNot)
        {
            Activationlbl.Text = "Username does not exist.";
        }
        else
        {
            var JsonFoundUser = JsonWebClient.DownloadString(wsUrl + "/finduser/" + UsernameRgstxt.Text);
            var FoundUser = JsonHelper.Deserialize<Users>(JsonFoundUser);
            var JsonFoundUserDetail = JsonWebClient.DownloadString(wsUrl + "/finduserdetail/" + FoundUser.UserID.ToString());

            string PostResult = string.Empty;
            JsonWebClient.Headers.Add(HttpRequestHeader.AcceptCharset, "UTF-8");
            JsonWebClient.Headers["Content-type"] = "application/json";
            PostResult = JsonWebClient.UploadString(wsUrl + "/sendmail", JsonFoundUserDetail);
            if (!Convert.ToBoolean(int.Parse(PostResult)))
            {
                Activationlbl.Text = "Sending activation code is failed. Please contact with techexpertmonitoringsystem@gmail.com";
            }
            else
            {
                Activationlbl.Text = "New regisration code has been sent to your username specified e-mail address successfully.";
            }            
        }
    }
    protected void Backbtn_Click(object sender, EventArgs e)
    {
        Response.Redirect("WelcomePage.aspx");
    }
}