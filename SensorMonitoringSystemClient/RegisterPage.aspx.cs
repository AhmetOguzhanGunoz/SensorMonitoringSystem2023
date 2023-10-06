using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Timers;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class RegisterPage : System.Web.UI.Page
{
    public static List<Companies> AllCompanies = new List<Companies>();
    public static List<Countries> AllCountries = new List<Countries>();
    public static List<DirtyWords> AllDirtyWords = new List<DirtyWords>();
    public static List<Cities> SelectedCities = new List<Cities>();
    public static List<Districts> SelectedDistricts = new List<Districts>();

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
        AllCompanies.Clear();
        AllCountries.Clear();
        AllDirtyWords.Clear();

        var SerializedJsonAllCompanies = JsonWebClient.DownloadString(wsUrl + "/findallcompanies");
        var DeserializedJsonAllCompanies = JsonHelper.Deserialize<List<Companies>>(SerializedJsonAllCompanies);

        foreach (Companies Company in DeserializedJsonAllCompanies)
        {
            if(!IsPostBack)
            {
                Companyddl.Items.Add(Company.CompanyName);
            }
            AllCompanies.Add(Company);
        }

        var SerializedJsonAllCountries = JsonWebClient.DownloadString(wsUrl + "/findallcountries");
        var DeserializedJsonAllCountries = JsonHelper.Deserialize<List<Countries>>(SerializedJsonAllCountries);

        foreach (Countries Country in DeserializedJsonAllCountries)
        {
            if (!IsPostBack)
            {
                Countryddl.Items.Add(Country.CountryName);
            }
            AllCountries.Add(Country);
        }

        var SerializedJsonAllDirtyWords = JsonWebClient.DownloadString(wsUrl + "/findalldirtywords");
        var DeserializedJsonAllDirtyWords = JsonHelper.Deserialize<List<DirtyWords>>(SerializedJsonAllDirtyWords);

        foreach (DirtyWords DirtyWord in DeserializedJsonAllDirtyWords)
        {
            AllDirtyWords.Add(DirtyWord);
        }

        if (this.IsPostBack) //Keep password value after postback
        {
            PasswordRgstxt.Attributes["value"] = PasswordRgstxt.Text;
        }
    }

    protected void Submitbtn_Click(object sender, EventArgs e)
    {
        string ProfanityResult, PostResult = string.Empty;
        if (!string.IsNullOrEmpty(ProfanityResult = CheckProfanity(UsernameRgstxt.Text)))
        {
            Successlbl.Text = "You entered non-valid username. Please enter another username. (" + ProfanityResult + ") found!";
        }
        else if (!string.IsNullOrEmpty(ProfanityResult = CheckProfanity(Addresstxt.Text)))
        {
            Successlbl.Text = "You entered non-valid address. Please enter proper address. (" + ProfanityResult + ") found!";
        }
        else
        {

            var JsonUsernameExistOrNot = JsonWebClient.DownloadString(wsUrl + "/usernamecontrol/" + UsernameRgstxt.Text);
            var UsernameExistOrNot = JsonHelper.Deserialize<bool>(JsonUsernameExistOrNot);

            if (UsernameExistOrNot)
            {
                Successlbl.Text = "Username already exist";
            }
            else
            {

                Users NewUser = new Users()
                {
                    Name = Nametxt.Text,
                    Surname = Surnametxt.Text,
                    Username = UsernameRgstxt.Text,
                    Password = Encoding.UTF8.GetBytes(PasswordRgstxt.Text)
                };

                foreach (Companies Company in AllCompanies)
                {
                    if (Company.CompanyName == Companyddl.SelectedValue)
                    {
                        NewUser.CompanyID = Company.CompanyID;
                    }
                }

                //Save New User Data
                var NewUserJson = JsonHelper.Serialize(NewUser);
                JsonWebClient.Headers.Add(HttpRequestHeader.AcceptCharset, "UTF-8");
                JsonWebClient.Headers["Content-type"] = "application/json";
                PostResult = JsonWebClient.UploadString(wsUrl + "/registeruser", NewUserJson);

                if (!Convert.ToBoolean(int.Parse(PostResult)))
                {
                    Successlbl.Text = "Problem occurred during saving user data. Please try again.";
                }
                else
                {
                    var JsonGetRegisteredUser = JsonWebClient.DownloadString(wsUrl + "/finduser/" + UsernameRgstxt.Text);
                    var GetRegisteredUser = JsonHelper.Deserialize<Users>(JsonGetRegisteredUser);

                    if (GetRegisteredUser == null)
                    {
                        Successlbl.Text = "Problem occurred during getting saved user data. Please try again.";
                    }
                    else
                    {
                        UsersDetails NewUserDetail = new UsersDetails()
                        {
                            UserID = GetRegisteredUser.UserID,
                            PhoneNumber = Phonetxt.Text,
                            Email = Emailtxt.Text,
                            Country = Countryddl.SelectedValue,
                            City = Cityddl.SelectedValue,
                            District = Districtddl.SelectedValue,
                            Address = Addresstxt.Text,
                            DateOfBirth = DateTime.Parse(DOBtxt.Text)
                        };

                        var NewUserDetailJson = JsonHelper.Serialize(NewUserDetail);
                        JsonWebClient.Headers.Add(HttpRequestHeader.AcceptCharset, "UTF-8");
                        JsonWebClient.Headers["Content-type"] = "application/json";
                        PostResult = JsonWebClient.UploadString(wsUrl + "/registeruserdetail", NewUserDetailJson);

                        if (!Convert.ToBoolean(int.Parse(PostResult)))
                        {
                            Successlbl.Text = "Problem occurred during saving user detail data. Please try again.";
                            JsonWebClient.Headers.Add(HttpRequestHeader.AcceptCharset, "UTF-8");
                            JsonWebClient.Headers["Content-type"] = "application/json";
                            PostResult = JsonWebClient.UploadString(wsUrl + "/deleteuser/", GetRegisteredUser.UserID.ToString());
                        }
                        else
                        {
                            JsonWebClient.Headers.Add(HttpRequestHeader.AcceptCharset, "UTF-8");
                            JsonWebClient.Headers["Content-type"] = "application/json";
                            PostResult = JsonWebClient.UploadString(wsUrl + "/sendmail", NewUserDetailJson);
                            if (!Convert.ToBoolean(int.Parse(PostResult)))
                            {
                                Successlbl.Text = "Sending activation code is failed. Please visit account activation page for obtainment of activation code or contact with techexpertmonitoringsystem@gmail.com";
                            }
                            else
                            {
                                Successlbl.Text = "Registration success. Check specified e-mail address for obtainment of activation code. Activate your account through activation page."
                                + "If any problem occurred with your code please try again within account activation page or contact with techexpertmonitoringsystem@gmail.com";
                            }
                        }
                    }
                }
            }
        }
    }

    protected void Backbtn_Click(object sender, EventArgs e)
    {
        Response.Redirect("WelcomePage.aspx");
    }

    protected void Countryddl_SelectedIndexChanged(object sender, EventArgs e)
    {
        if(Countryddl.SelectedValue != "0")
        {
            string CountryID = "";
            Cityddl.Items.Clear();
            SelectedCities.Clear();
            Districtddl.Items.Clear();
            SelectedDistricts.Clear();
            foreach (Countries Country in AllCountries)
            {
                if (Country.CountryName == Countryddl.SelectedValue)
                {
                    CountryID = Country.CountryID.ToString();
                }
            }

            var SerializedJsonSelectedCities = JsonWebClient.DownloadString(wsUrl + "/findcities/" + CountryID);
            var DeserializedJsonSelectedCities = JsonHelper.Deserialize<List<Cities>>(SerializedJsonSelectedCities);

            foreach (Cities City in DeserializedJsonSelectedCities)
            {
                Cityddl.Items.Add(City.CityName);
                SelectedCities.Add(City);
            }

            if (SelectedCities.Count > 0)
            {
                Cityddl.Items.Insert(0, new ListItem("Select City", "0"));
                Districtddl.Items.Insert(0, new ListItem("Select District", "0"));
                Cityddl.SelectedIndex = 0;
                Districtddl.SelectedIndex = 0;
            }
        }

    }

    protected void Cityddl_SelectedIndexChanged(object sender, EventArgs e)
    {
        if(Cityddl.SelectedValue != "0")
        {
            string DistrictRelationID = "";
            Districtddl.Items.Clear();
            SelectedDistricts.Clear();
            foreach (Cities City in SelectedCities)
            {
                if (City.CityName == Cityddl.SelectedValue)
                {
                    DistrictRelationID = City.DistrictRelationID.ToString();
                }
            }
            if (Cityddl.SelectedItem.Value != "0")
            {
                var SerializedJsonSelectedDistricts = JsonWebClient.DownloadString(wsUrl + "/finddistricts/" + DistrictRelationID);
                var DeserializedJsonSelectedDistricts = JsonHelper.Deserialize<List<Districts>>(SerializedJsonSelectedDistricts);

                foreach (Districts District in DeserializedJsonSelectedDistricts)
                {
                    Districtddl.Items.Add(District.DistrictName);
                    SelectedDistricts.Add(District);
                }

                if (SelectedDistricts.Count > 0)
                {
                    Districtddl.Items.Insert(0, new ListItem("Select District", "0"));
                    Districtddl.SelectedIndex = 0;
                }
            }
        }
    }

    private string CheckProfanity(string CheckText)
    {
        string result = string.Empty;

        foreach (DirtyWords DirtyWords in AllDirtyWords)
        {
            if (CheckText == DirtyWords.DirtyWord)
            {
                result = DirtyWords.DirtyWord;
                break;
            }
            else if (CheckText.ToLower().Contains(DirtyWords.DirtyWord.ToLower()))
            {
                string DirtyWordPattern = "(^|\\s)" + DirtyWords.DirtyWord.ToLower() + "($|\\s)";

                if (System.Text.RegularExpressions.Regex.IsMatch(CheckText.ToLower(), DirtyWordPattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                {
                    result = DirtyWords.DirtyWord;
                    break;
                }
            }
        }

        return result;
    }
}