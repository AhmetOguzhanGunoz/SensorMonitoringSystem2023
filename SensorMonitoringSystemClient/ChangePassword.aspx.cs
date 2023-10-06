using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ChangePassword : System.Web.UI.Page
{
    const string wsUrl = "http://localhost:63420/SensorMonitoringSystemService.svc/rest";
    public static string username = "";
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
        if(!IsPostBack)
        {
            if (Session["username"] == null || String.IsNullOrEmpty(Session["username"].ToString()))
            {
                Response.Redirect("ProfilePage.aspx");
            }
            else
            {
                username = Session["username"].ToString();
                Session.RemoveAll();
            }
        }
    }

    protected void Submitbtn_Click(object sender, EventArgs e)
    {
        if (NewPasswordtxt.Text != NewPasswordConfirmtxt.Text)
        {
            Successlbl.Text = "New password and its confirmation are not same.";
        }
        else
        {
            var JsonFoundUser = JsonWebClient.DownloadString(wsUrl + "/finduser/" + username);
            var FoundUser = JsonHelper.Deserialize<Users>(JsonFoundUser);
            if (FoundUser == null)
            {
                Successlbl.Text = "Problem occurred during getting searched user data. Please try again.";
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


    protected void Backbtn_Click(object sender, EventArgs e)
    {
        Session["username"] = username;
        Response.Redirect("ProfilePage.aspx");
    }
}