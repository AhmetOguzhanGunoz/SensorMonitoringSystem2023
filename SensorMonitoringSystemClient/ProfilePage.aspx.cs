using System;
using System.Activities.Expressions;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.Configuration;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ProfilePage : System.Web.UI.Page
{
    public static string username = "";
    public static List<Sensors> AllSensors = new List<Sensors>();
    public static Users LoggedUser = new Users();
    public static UsersDetails LoggedUserDetail = new UsersDetails();
    public static List<Users> RegisteredandActivatedCompanyUsers = new List<Users>();
    public static List<Users> CompanysAllApprovedUsers = new List<Users>();

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
        if (!IsPostBack)
        {
            if (Session["username"] == null || String.IsNullOrEmpty(Session["username"].ToString()))
            {
                Response.Redirect("WelcomePage.aspx");
            }
            else
            {
                username = Session["username"].ToString();
                Session.RemoveAll();
            }

            var JsonFoundUser = JsonWebClient.DownloadString(wsUrl + "/finduser/" + username);
            var FoundUser = JsonHelper.Deserialize<Users>(JsonFoundUser);
            LoggedUser = FoundUser;

            var JsonFoundUserDetail = JsonWebClient.DownloadString(wsUrl + "/finduserdetail/" + FoundUser.UserID.ToString());
            var FoundUserDetail = JsonHelper.Deserialize<UsersDetails>(JsonFoundUserDetail);
            LoggedUserDetail = FoundUserDetail;

            Dearlbl.Text = "Welcome Dear, <b>" + FoundUser.Name + " " + FoundUser.Surname.ToUpper() + "</b>";

            var JsonFoundUserTypes = JsonWebClient.DownloadString(wsUrl + "/findallusertypes");
            var FoundUserTypes = JsonHelper.Deserialize<List<UserTypes>>(JsonFoundUserTypes);

            if (FoundUserTypes == null)
            {
                UserTypeRadlist.Items.Add("No Recorded UserType");
            }
            else
            {
                foreach (UserTypes UserType in FoundUserTypes)
                {
                    UserTypeRadlist.Items.Add(UserType.UserTypeName);
                }
            }

            switch(LoggedUser.UserType)
            {
                case ("System Admin"):
                case ("Company Owner"):
                    RegisteredandActivatedCompanyUsers.Clear();
                    AllSensors.Clear();
                    var JsonFoundSensorsAdmin = JsonWebClient.DownloadString(wsUrl + "/findallsensors/" + LoggedUser.CompanyID.ToString());
                    var FoundSensorsAdmin = JsonHelper.Deserialize<List<Sensors>>(JsonFoundSensorsAdmin);

                    if (FoundSensorsAdmin.Count <= 0)
                    {
                        Sensorsddl.Items.Add("No Recorded Sensor");
                        Sensorsddl.Enabled = false;
                    }
                    else
                    {
                        foreach (Sensors Sensor in FoundSensorsAdmin)
                        {
                            Sensorsddl.Items.Add(Sensor.SensorDescription);
                            AllSensors.Add(Sensor);
                        }
                        SensorAddresslbl.Text = AllSensors.First().SensorAddress;
                    }

                    var JsonFoundRegisteredandActivatedUsersAdmin = JsonWebClient.DownloadString(wsUrl + "/findcompanysallactivatednotapprovedusers/" + LoggedUser.CompanyID.ToString());
                    var FoundRegisteredandActivatedUsersAdmin = JsonHelper.Deserialize<List<Users>>(JsonFoundRegisteredandActivatedUsersAdmin);

                    if (FoundRegisteredandActivatedUsersAdmin.Count <= 0 || FoundRegisteredandActivatedUsersAdmin == null)
                    {
                        RegisteredUsersChklist.Items.Add("No Record of Activated User");
                        RegisteredUsersChklist.Enabled = false;
                    }
                    else
                    {
                        foreach (Users User in FoundRegisteredandActivatedUsersAdmin)
                        {
                            RegisteredUsersChklist.Items.Add(" " + User.Name + " " + User.Surname + " (" + User.Username + ")");
                            RegisteredandActivatedCompanyUsers.Add(User);
                        }
                    }
                    break;
                case ("Technical User"):
                    AllSensors.Clear();
                    var JsonFoundSensorsTechnicalUser = JsonWebClient.DownloadString(wsUrl + "/findallsensors/" + LoggedUser.CompanyID.ToString());
                    var FoundSensorsTechnicalUser = JsonHelper.Deserialize<List<Sensors>>(JsonFoundSensorsTechnicalUser);

                    if (FoundSensorsTechnicalUser.Count <= 0)
                    {
                        Sensorsddl.Items.Add("No Recorded Sensor");
                        Sensorsddl.Enabled = false;
                    }
                    else
                    {
                        foreach (Sensors Sensor in FoundSensorsTechnicalUser)
                        {
                            Sensorsddl.Items.Add(Sensor.SensorDescription);
                            AllSensors.Add(Sensor);
                        }
                        SensorAddresslbl.Text = AllSensors.First().SensorAddress;
                    }
                    Profilearea3.Attributes.Add("style", "display:none");
                    break;
                case ("Human Resources User"):
                    RegisteredandActivatedCompanyUsers.Clear();
                    var JsonFoundRegisteredandActivatedUsersHR = JsonWebClient.DownloadString(wsUrl + "/findcompanysallactivatednotapprovedusers/" + LoggedUser.CompanyID.ToString());
                    var FoundRegisteredandActivatedUsersHR = JsonHelper.Deserialize<List<Users>>(JsonFoundRegisteredandActivatedUsersHR);

                    if (FoundRegisteredandActivatedUsersHR.Count <= 0 || FoundRegisteredandActivatedUsersHR == null)
                    {
                        RegisteredUsersChklist.Items.Add("No Record of Activated User");
                        RegisteredUsersChklist.Enabled = false;
                    }
                    else
                    {
                        foreach (Users User in FoundRegisteredandActivatedUsersHR)
                        {
                            RegisteredUsersChklist.Items.Add(" " + User.Name + " " + User.Surname + " (" + User.Username + ")");
                            RegisteredandActivatedCompanyUsers.Add(User);
                        }
                    }
                    Profilearea2.Attributes.Add("style", "display:none");
                    break;
                case ("Standard User"):
                    AllSensors.Clear();
                    var JsonFoundSensorsStandardUser = JsonWebClient.DownloadString(wsUrl + "/findallsensors/" + LoggedUser.CompanyID.ToString());
                    var FoundSensorsStandardUser = JsonHelper.Deserialize<List<Sensors>>(JsonFoundSensorsStandardUser);

                    if (FoundSensorsStandardUser.Count <= 0)
                    {
                        Sensorsddl.Items.Add("No Recorded Sensor");
                        Sensorsddl.Enabled = false;
                    }
                    else
                    {
                        foreach (Sensors Sensor in FoundSensorsStandardUser)
                        {
                            Sensorsddl.Items.Add(Sensor.SensorDescription);
                            AllSensors.Add(Sensor);
                        }
                        SensorAddresslbl.Text = AllSensors.First().SensorAddress;
                    }
                    AddSensorbtn.Enabled = false;
                    Profilearea3.Attributes.Add("style", "display:none");
                    break;
                case ("Sensor Type User"):
                    AllSensors.Clear();
                    var JsonFoundSensorsSensorTypeUser = JsonWebClient.DownloadString(wsUrl + "/findcompanysallspecificsensors/" + LoggedUser.CompanyID.ToString());
                    var FoundSensorsSensorTypeUser = JsonHelper.Deserialize<List<Sensors>>(JsonFoundSensorsSensorTypeUser);

                    if (FoundSensorsSensorTypeUser.Count <= 0)
                    {
                        Sensorsddl.Items.Add("No Recorded Sensor");
                        Sensorsddl.Enabled = false;
                    }
                    else
                    {
                        foreach (Sensors Sensor in FoundSensorsSensorTypeUser)
                        {
                            Sensorsddl.Items.Add(Sensor.SensorDescription);
                            AllSensors.Add(Sensor);
                        }
                        SensorAddresslbl.Text = AllSensors.First().SensorAddress;
                    }
                    AddSensorbtn.Enabled = false;
                    Profilearea3.Attributes.Add("style", "display:none"); //For Reachable element adding attribute instead of visible false
                    break;
                default:
                    Session.RemoveAll();
                    Response.Redirect("WelcomePage.aspx");
                    break;
            }
        }
    }

    protected void Sensorsddl_SelectedIndexChanged(object sender, EventArgs e)
    {
        foreach (Sensors Sensor in AllSensors)
        {
            if(Sensorsddl.SelectedValue == Sensor.SensorDescription)
            {
                SensorAddresslbl.Text = Sensor.SensorAddress;
            }
        }
    }

    protected void Changecodebtn_Click(object sender, EventArgs e)
    {
        string PostResult = string.Empty;
        var JsonLoggedUser = JsonHelper.Serialize(LoggedUser);
        JsonWebClient.Headers.Add(HttpRequestHeader.AcceptCharset, "UTF-8");
        JsonWebClient.Headers["Content-type"] = "application/json";
        PostResult = JsonWebClient.UploadString(wsUrl + "/changecode", JsonLoggedUser);

        if (!Convert.ToBoolean(int.Parse(PostResult)))
        {
            Changelbl.Text = "Problem occurred during changing user registration code. Please try again.";
        }
        else
        {
            var JsonLoggedUserDetail = JsonHelper.Serialize(LoggedUserDetail);
            JsonWebClient.Headers.Add(HttpRequestHeader.AcceptCharset, "UTF-8");
            JsonWebClient.Headers["Content-type"] = "application/json";
            PostResult = JsonWebClient.UploadString(wsUrl + "/sendmail", JsonLoggedUserDetail);

            if (!Convert.ToBoolean(int.Parse(PostResult)))
            {
                Changelbl.Text = "Problem occurred during sending user registration code. Please try again.";
            }
            else
            {
                Changelbl.Text = "Your activation code has been changed and your information sent to your specified e-mail address.";
            }
        }
    }

    protected void Analyzebtn_Click(object sender, EventArgs e)
    {
        int TempID = 0;

        foreach (Sensors Sensor in AllSensors)
        {
            if (Sensor.SensorDescription == Sensorsddl.SelectedValue)
            {
                TempID = Sensor.SensorID;
            }
        }
        var JsonSensorDataExistOrNot = JsonWebClient.DownloadString(wsUrl + "/sensordatacontrol/" + TempID.ToString());
        var SensorDataExistOrNot = JsonHelper.Deserialize<bool>(JsonSensorDataExistOrNot);

        if (!SensorDataExistOrNot && LoggedUser.UserType != "System Admin" && LoggedUser.UserType != "Company Owner" && LoggedUser.UserType != "Technical User" && LoggedUser.UserType != "Sensor Type User")
        {
            SensorAddresslbl.Text = "Selected sensor does not have data for analyzing.";
        }
        else
        {
            foreach (Sensors Sensor in AllSensors)
            {
                if (Sensor.SensorDescription == Sensorsddl.SelectedValue)
                {
                    Session["SensorID"] = Sensor.SensorID.ToString();
                    Session["username"] = username.ToString();
                }
            }
            Response.Redirect("MonitorPage.aspx");
        }
    }

    protected void Changepasswordbtn_Click(object sender, EventArgs e)
    {
        Session["username"] = username;
        Response.Redirect("ChangePassword.aspx");
    }


    protected void ApproveBtn_Click(object sender, EventArgs e)
    {

        if (RegisteredUsersChklist.SelectedIndex == -1 && RegisteredUsersChklist.Enabled)
        {
            RegisteredUserslbl.Text = "Please select user/s to be approved.";
        }
        else if(UserTypeRadlist.SelectedIndex == -1)
        {
            RegisteredUserslbl.Text = "Please select user type to make selected user/s to be approved.";
        }
        else
        {
            List<Users> SelectedUsers = new List<Users>();
            List<ListItem> ToBeRemovedItem = new List<ListItem>();
            foreach (ListItem item in RegisteredUsersChklist.Items)
            {
                if (item.Selected)
                {
                    foreach (Users User in RegisteredandActivatedCompanyUsers)
                    {
                        if (item.Text.Contains(User.Username))
                        {
                            SelectedUsers.Add(User);                            
                        }
                    }
                    ToBeRemovedItem.Add(item);
                }
            }

            foreach (Users GonnaBeApprovedUser in SelectedUsers)
            {
                GonnaBeApprovedUser.UserType = UserTypeRadlist.SelectedItem.Text;
                string PostResult = string.Empty;
                var JsonGonnaBeApprovedUser = JsonHelper.Serialize(GonnaBeApprovedUser);
                JsonWebClient.Headers.Add(HttpRequestHeader.AcceptCharset, "UTF-8");
                JsonWebClient.Headers["Content-type"] = "application/json";
                PostResult = JsonWebClient.UploadString(wsUrl + "/approval", JsonGonnaBeApprovedUser);

                if (!Convert.ToBoolean(int.Parse(PostResult)))
                {
                    RegisteredUserslbl.Text = "Problem occurred during saving user data. Please try again.";
                    break;
                }
                else
                {
                    foreach (ListItem item in RegisteredUsersChklist.Items)
                    {
                        if (item.Text.Contains(GonnaBeApprovedUser.Username))
                        {
                            RegisteredUsersChklist.Items.Remove(item);
                            break;
                        }
                    }
                    RegisteredandActivatedCompanyUsers.Remove(GonnaBeApprovedUser);

                    if (SelectedUsers.Count == 1)
                    {
                        RegisteredUserslbl.Text = "Approvement is made successfully.";
                    }
                    else
                    {
                        RegisteredUserslbl.Text = "Approvements are made successfully.";
                    }

                }
            }
            if(RegisteredUsersChklist.Items.Count == 0)
            {
                RegisteredandActivatedCompanyUsers.Clear();
                RegisteredUsersChklist.Items.Add("No Record of Activated User");
                RegisteredUsersChklist.Enabled = false;
            }
        }
    }

    

    protected void RefuseBtn_Click(object sender, EventArgs e)
    {
        if (DeleteUserBtn.Text == "In Deleting Process (Click to Return Approve)")
        {
            if (RegisteredUsersChklist.SelectedIndex == -1 && RegisteredUsersChklist.Enabled)
            {
                RegisteredUserslbl.Text = "Please select user/s to be deleted.";
                DeleteUserBtn.Text = "Obtain Approved Users For Delete";
                UserTypeRadlist.Enabled = true;
                ApproveBtn.Enabled = true;
                DeleteUserBtn.Enabled = true;
                RefuseBtn.Text = "Refuse Selected";
                Userslbl.Text = "Registered and Activated Users";
                RegisteredUsersChklist.Items.Clear();

                if (RegisteredandActivatedCompanyUsers.Count > 0)
                {
                    foreach (Users User in RegisteredandActivatedCompanyUsers)
                    {
                        RegisteredUsersChklist.Items.Add(" " + User.Name + " " + User.Surname + " (" + User.Username + ")");
                    }
                    RegisteredUsersChklist.Enabled = true;

                }
                else
                {
                    RegisteredUsersChklist.Items.Add("No Record of Activated User");
                    RegisteredUsersChklist.Enabled = false;
                }
            }
            else
            {
                List<Users> SelectedUsers = new List<Users>();
                foreach (ListItem item in RegisteredUsersChklist.Items)
                {
                    if (item.Selected)
                    {
                        foreach (Users User in CompanysAllApprovedUsers)
                        {
                            if (item.Text.Contains(User.Username))
                            {
                                SelectedUsers.Add(User);
                            }
                        }
                    }
                }

                foreach (Users GonnaBeDeletedUser in SelectedUsers)
                {
                    string PostResult = string.Empty;
                    JsonWebClient.Headers.Add(HttpRequestHeader.AcceptCharset, "UTF-8");
                    JsonWebClient.Headers["Content-type"] = "application/json";
                    PostResult = JsonWebClient.DownloadString(wsUrl + "/deleteuser/" + GonnaBeDeletedUser.UserID);
                    if (!Convert.ToBoolean(int.Parse(PostResult)))
                    {
                        RegisteredUserslbl.Text = "Problem occurred during deleting user data. Please try again.";
                        break;
                    }
                    else if (SelectedUsers.Count == 1)
                    {
                        RegisteredUserslbl.Text = "Deleting is made successfully.";
                    }
                    else
                    {
                        RegisteredUserslbl.Text = "Deletings are made successfully.";
                    }
                }

                CompanysAllApprovedUsers.Clear(); // will reload with obtain button
                RegisteredUsersChklist.Items.Clear(); // in case one selection of multiple selectable item delete left approved item/s

                if (RegisteredandActivatedCompanyUsers.Count > 0)
                {
                    foreach (Users User in RegisteredandActivatedCompanyUsers)
                    {
                        RegisteredUsersChklist.Items.Add(" " + User.Name + " " + User.Surname + " (" + User.Username + ")");
                    }
                    RegisteredUsersChklist.Enabled = true;

                }
                else
                {
                    RegisteredUsersChklist.Items.Add("No Record of Activated User");
                    RegisteredUsersChklist.Enabled = false;
                }
                UserTypeRadlist.Enabled = true;
                ApproveBtn.Enabled = true;
                DeleteUserBtn.Enabled = true;
                RefuseBtn.Text = "Refuse Selected";
                DeleteUserBtn.Text = "Obtain Approved Users For Delete";
                Userslbl.Text = "Registered and Activated Users";
            }
        }
        else
        {
            if (RegisteredUsersChklist.SelectedIndex == -1 && RegisteredUsersChklist.Enabled)
            {
                RegisteredUserslbl.Text = "Please select user/s to be refused.";
            }
            else
            {
                List<Users> SelectedUsers = new List<Users>();
                List<ListItem> ToBeRemovedItem = new List<ListItem>();
                foreach (ListItem item in RegisteredUsersChklist.Items)
                {
                    if (item.Selected)
                    {
                        foreach (Users User in RegisteredandActivatedCompanyUsers)
                        {
                            if (item.Text.Contains(User.Username))
                            {
                                SelectedUsers.Add(User);
                            }
                        }
                        ToBeRemovedItem.Add(item);
                    }
                }

                foreach (Users GonnaBeRefusedUser in SelectedUsers)
                {
                    string PostResult = string.Empty;
                    JsonWebClient.Headers.Add(HttpRequestHeader.AcceptCharset, "UTF-8");
                    JsonWebClient.Headers["Content-type"] = "application/json";
                    PostResult = JsonWebClient.DownloadString(wsUrl + "/deleteuser/" + GonnaBeRefusedUser.UserID);
                    if (!Convert.ToBoolean(int.Parse(PostResult)))
                    {
                        RegisteredUserslbl.Text = "Problem occurred during deleting user data. Please try again.";
                        break;
                    }
                    else
                    {
                        foreach (ListItem item in RegisteredUsersChklist.Items)
                        {
                            if (item.Text.Contains(GonnaBeRefusedUser.Username))
                            {
                                RegisteredUsersChklist.Items.Remove(item);
                                break;
                            }
                        }
                        RegisteredandActivatedCompanyUsers.Remove(GonnaBeRefusedUser);

                        if (SelectedUsers.Count == 1)
                        {
                            RegisteredUserslbl.Text = "Refusing is made successfully.";
                        }
                        else
                        {
                            RegisteredUserslbl.Text = "Refusings are made successfully.";
                        }

                    }
                }
                if (RegisteredUsersChklist.Items.Count == 0)
                {
                    RegisteredandActivatedCompanyUsers.Clear();
                    RegisteredUsersChklist.Items.Add("No Record of Activated User");
                    RegisteredUsersChklist.Enabled = false;
                }

            }
        }
    }
    
    protected void Logoutbtn_Click(object sender, EventArgs e)
    {
        Session.RemoveAll();
        Response.Redirect("WelcomePage.aspx");
    }

    protected void DeleteUserBtn_Click(object sender, EventArgs e)
    {
        if(DeleteUserBtn.Text != "In Deleting Process (Click to Return Approve)")
        {
            CompanysAllApprovedUsers.Clear();
            var JsonFoundCompanyUsers = JsonWebClient.DownloadString(wsUrl + "/findallcompanysusers/" + LoggedUser.CompanyID);
            var FoundCompanyUsers = JsonHelper.Deserialize<List<Users>>(JsonFoundCompanyUsers);

            if (FoundCompanyUsers == null || FoundCompanyUsers.Count <= 0)
            {
                RegisteredUserslbl.Text = "No record of approved user in company.";
            }
            else
            {
                RegisteredUsersChklist.Enabled = true;
                RegisteredUsersChklist.Items.Clear();
                UserTypeRadlist.Enabled = false;
                DeleteUserBtn.Text = "In Deleting Process (Click to Return Approve)";
                DeleteUserBtn.Enabled = false;
                Userslbl.Text = "Approved Company Users";
                RegisteredUserslbl.Text = "";

                foreach (Users User in FoundCompanyUsers)
                {
                    if(User.Username != LoggedUser.Username)
                    {
                        RegisteredUsersChklist.Items.Add(" " + User.Name + " " + User.Surname + " (" + User.Username + ")");
                        CompanysAllApprovedUsers.Add(User);
                    }
                }

            }
        }
        else
        {
            Session.RemoveAll();
            Session["username"] = username;
            Response.Redirect("ProfilePage.aspx");
        }
    }

    protected void DeleteSensorbtn_Click(object sender, EventArgs e)
    {
        Sensors SelectedSensor = new Sensors();
        foreach(Sensors Sensor in AllSensors)
        {
            if(Sensorsddl.SelectedValue == Sensor.SensorDescription)
            {
                SelectedSensor = Sensor;
                break;
            }
        }

        string PostResult = string.Empty;
        JsonWebClient.Headers.Add(HttpRequestHeader.AcceptCharset, "UTF-8");
        JsonWebClient.Headers["Content-type"] = "application/json";
        PostResult = JsonWebClient.DownloadString(wsUrl + "/deletesensor/" + SelectedSensor.SensorID.ToString());

        if (!Convert.ToBoolean(int.Parse(PostResult)))
        {
            SensorAddresslbl.Text = "Problem occurred during deleting sensor. Please try again.";
        }
        else
        {
            Sensorsddl.Items.Remove(SelectedSensor.SensorDescription);
            AllSensors.Remove(SelectedSensor);

            if (Sensorsddl.Items.Count == 0)
            {
                SensorAddresslbl.Text = "Sensor deleted successfully.";
                Sensorsddl.Items.Add("No Recorded Sensor");
                Sensorsddl.Enabled = false;
            }
            else
            {
                SensorAddresslbl.Text = "Sensor deleted successfully.";
            }
        }
    }
        

    protected void AddSensorbtn_Click(object sender, EventArgs e)
    {
        Session.RemoveAll();
        Session["username"] = username;
        Response.Redirect("SensorAddPage.aspx");
    }
}