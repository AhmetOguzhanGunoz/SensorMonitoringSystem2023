using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SensorAddPage : System.Web.UI.Page
{
    public static List<Countries> AllCountries = new List<Countries>();
    public static List<Cities> SelectedCities = new List<Cities>();
    public static List<Districts> SelectedDistricts = new List<Districts>();
    public static Users LoggedUser = new Users();
    public static string username = "";

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
        AllCountries.Clear();

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

        }
    }

    protected void Countryddl_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Countryddl.SelectedValue != "0")
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
        if (Cityddl.SelectedValue != "0")
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

    protected void Backbtn_Click(object sender, EventArgs e)
    {
        Session["username"] = username;
        Response.Redirect("ProfilePage.aspx");
    }

    protected void Submitbtn_Click(object sender, EventArgs e)
    {

        var num1 = decimal.Parse(MaxValuetxt.Text);
        var num2 = decimal.Parse(HighestCriticalValuetxt.Text);

        if ( decimal.Parse(MaxValuetxt.Text) <= decimal.Parse(MinValuetxt.Text) ||
            decimal.Parse(HighestCriticalValuetxt.Text) <= decimal.Parse(LowestCriticalValuetxt.Text) || 
            decimal.Parse(LowestCriticalValuetxt.Text) < decimal.Parse(MinValuetxt.Text) ||
            decimal.Parse(MaxValuetxt.Text) < decimal.Parse(HighestCriticalValuetxt.Text)
          )
        {
            Successlbl.Text = "Error occurred during saving sensor values. Please type proper numbers.";
        }
        else
        {
            string PostResult = string.Empty;

            Sensors NewSensor = new Sensors()
            {
                CompanyID = LoggedUser.CompanyID,
                SensorDescription = SensorDescriptiontxt.Text,
                SensorAddress = Countryddl.SelectedValue + " / " + Cityddl.SelectedValue + " / " + Districtddl.SelectedValue + " / Entered Address: " + SensorAddresstxt.Text,
                GraphicalMinValue = Int32.Parse(MinValuetxt.Text),
                GraphicalMaxValue = Int32.Parse(MaxValuetxt.Text),
                LowestCriticalValue = decimal.Parse(LowestCriticalValuetxt.Text),
                HighestCriticalValue = decimal.Parse(HighestCriticalValuetxt.Text),
                SensorUnit = SensorUnittxt.Text,
                IsSpecificSensor = SensorSpecificbtnlist.SelectedItem.Text == "Yes" ? true : false,
            };

            var NewSensorJson = JsonHelper.Serialize(NewSensor);
            JsonWebClient.Headers.Add(HttpRequestHeader.AcceptCharset, "UTF-8");
            JsonWebClient.Headers["Content-type"] = "application/json";
            PostResult = JsonWebClient.UploadString(wsUrl + "/addsensor", NewSensorJson);

            if (!Convert.ToBoolean(int.Parse(PostResult)))
            {
                Successlbl.Text = "Problem occurred during saving sensor. Please try again.";               
            }
            else
            {
                Successlbl.Text = "Sensor added successfully.";
            }
        }
    }
}