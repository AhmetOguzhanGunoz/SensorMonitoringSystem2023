using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ForgotPassword : System.Web.UI.Page
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

    protected void Checkbtn_Click(object sender, EventArgs e)
    {

        var JsonUsernameExistOrNot = JsonWebClient.DownloadString(wsUrl + "/usernamecontrol/" + UsernameRgstxt.Text);
        var UsernameExistOrNot = JsonHelper.Deserialize<bool>(JsonUsernameExistOrNot);

        if (!UsernameExistOrNot)
        {
            Checklbl.Text = "Username does not exist.";
        }
        else
        {
            var JsonFoundUser = JsonWebClient.DownloadString(wsUrl + "/finduser/" + UsernameRgstxt.Text);
            var FoundUser = JsonHelper.Deserialize<Users>(JsonFoundUser);

            if (Codetxt.Text != FoundUser.RegistrationCode.ToString())
            {
                Checklbl.Text = "Activation code is wrong. Please try again.";
            }
            else
            {
                Checklbl.Text = "User is found.";
            }
        }
    }
    

    protected void Submitbtn_Click(object sender, EventArgs e)
    {

        var JsonUsernameExistOrNot = JsonWebClient.DownloadString(wsUrl + "/usernamecontrol/" + UsernameRgstxt.Text);
        var UsernameExistOrNot = JsonHelper.Deserialize<bool>(JsonUsernameExistOrNot);

        if (!UsernameExistOrNot)
        {
            Checklbl.Text = "Username does not exist.";
        }
        else
        {
            var JsonFoundUser = JsonWebClient.DownloadString(wsUrl + "/finduser/" + UsernameRgstxt.Text);
            var FoundUser = JsonHelper.Deserialize<Users>(JsonFoundUser);

            if (FoundUser == null)
            {
                Successlbl.Text = "Problem occurred during getting searched user data. Please try again.";
            }
            else if (Codetxt.Text != FoundUser.RegistrationCode.ToString())
            {
                Checklbl.Text = "Activation code is wrong. Please try again.";
            }
            else
            {
                Checklbl.Text = "User is found.";
                if (NewPasswordtxt.Text != NewPasswordConfirmtxt.Text)
                {
                    Successlbl.Text = "New password and its confirmation are not same.";
                }
                else
                {
                    string PostResult = string.Empty;
                    FoundUser.Password = Encoding.UTF8.GetBytes(NewPasswordtxt.Text);
                    var JsonChangedPasswordUser = JsonHelper.Serialize(FoundUser);
                    JsonWebClient.Headers.Add(HttpRequestHeader.AcceptCharset, "UTF-8");
                    JsonWebClient.Headers["Content-type"] = "application/json";
                    PostResult = JsonWebClient.UploadString(wsUrl + "/changepassword", JsonChangedPasswordUser);

                    if (!Convert.ToBoolean(int.Parse(PostResult)))
                    {
                        Successlbl.Text = "Password does not changed. Please try again.";
                    }
                    else
                    {
                        Successlbl.Text = "Password has been changed successfully.";
                    }
                }
            }
        }     
    }
    protected void Backbtn_Click(object sender, EventArgs e)
    {
        Response.Redirect("WelcomePage.aspx");
    }
}