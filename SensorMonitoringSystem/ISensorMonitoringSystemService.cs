using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Web.Services;

namespace SensorMonitoringSystem
{
    [ServiceContract]
    public interface ISensorMonitoringSystemService
    {
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/findallcompanies", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<Companies> FindAllCompanies();

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/usernamecontrol/{username}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        bool UsernameControl(string username);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/registeruser", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        int RegisterUser(Users RegisteredUser);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/finduser/{username}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Users FindUser(string username);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/registeruserdetail", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        int RegisterUserDetail(UsersDetails RegisteredUserDetail);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/userdetailcontrol/{userid}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        bool UserDetailControl(string userid);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/sendmail", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        int SendInfoMail(UsersDetails RegisteredUserDetail);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/changepassword", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        int ChangePassword(Users ChangedPasswordUser);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/activation", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        int Activation(Users ActivatedUser);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/finduserdetail/{userid}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        UsersDetails FindUserDetail(string userid);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/changecode", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        int ChangeCode(Users ChangedCodeUser);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/findallsensors/{companyid}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<Sensors> FindCompaniesAllSensors(string companyid);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/sensordatacontrol/{sensorid}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        bool SensorDataControl(string sensorid);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/findsensor/{sensorid}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        Sensors FindSensor(string sensorid);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/findallsensordatas/{sensorid}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<SensorsDatas> FindAllSensorDatas(string sensorid);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/finddatasbetweendates/{sensorid}/{startdate}/{enddate}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<SensorsDatas> FindDatasBetweenDates(string sensorid, string startdate, string enddate);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/findallcountries", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<Countries> FindAllCountries();

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/findcities/{countryid}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<Cities> FindCitiesOfCountry(string countryid);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/finddistricts/{districtrelationid}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<Districts> FindDistrictsOfCity(string districtrelationid);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/login/{username}/{password}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        bool UserLogin(string username, string password);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/addsensor", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        int AddSensortoCompany(Sensors RegisteredSensor);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/addsensorsdatas", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        int AddSensorsDatastoCompanysSensors(SensorsDatas RegisteredSensorsDatas);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/findalldirtywords", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<DirtyWords> FindAllDirtyWords();

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/approval", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        int Approval(Users ApprovedUser);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/deleteuser/{UserID}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        int DeleteUser(string UserID);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/deleteuserdetail/{UserID}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        int DeleteUserDetail(string UserID);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/findallusertypes", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<UserTypes> FindAllUserTypes();

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/findcompanysallactivatednotapprovedusers/{companyid}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<Users> FindCompanysAllActivatednotApprovedUsers(string CompanyID);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/findcompanysallspecificsensors/{companyid}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<Sensors> FindCompanysAllSpecificSensors(string CompanyID);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/findallcompanysusers/{companyid}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<Users> FindAllCompanysUsers(string CompanyID);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/deletesensor/{SensorID}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        int DeleteSensor(string SensorID);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/deletesensorsdatas/{SensorID}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        int DeleteSensorData(string SensorID);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/deletesensorsdatasbyid/{DataID}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        int DeleteSensorDataByID(string DataID);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/deletedatasbetweendates/{sensorid}/{startdate}/{enddate}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        int DeleteDatasBetweenDates(string sensorid, string startdate, string enddate);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/warningmail", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        void SendWarningMail();

    }

    [DataContract]
    public class Companies
    {
        [DataMember]
        public int CompanyID { get; set; }
        [DataMember]
        public string CompanyName { get; set; }
    }

    [DataContract]
    public class Users
    {
        [DataMember]
        public int UserID { get; set; }
        [DataMember]
        public int CompanyID { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Surname { get; set; }
        [DataMember]
        public string Username { get; set; }
        [DataMember]
        public byte[] Password { get; set; }
        [DataMember]
        public bool IsActivated { get; set; }
        [DataMember]
        public bool IsApproved { get; set; }
        [DataMember]
        public long RegistrationCode { get; set; }
        [DataMember]
        public System.Guid PasswordKey { get; set; }
        [DataMember]
        public string UserType { get; set; }

    }

    [DataContract]
    public class UsersDetails
    {
        [DataMember]
        public int DetailID { get; set; }
        [DataMember]
        public int UserID { get; set; }
        [DataMember]
        public string PhoneNumber { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Country { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string District { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public DateTime DateOfBirth { get; set; }

    }

    [DataContract]
    public class Sensors
    {
        [DataMember]
        public int SensorID { get; set; }
        [DataMember]
        public int CompanyID { get; set; }
        [DataMember]
        public string SensorDescription { get; set; }
        [DataMember]
        public string SensorAddress { get; set; }
        [DataMember]
        public int GraphicalMinValue { get; set; }
        [DataMember]
        public int GraphicalMaxValue { get; set; }
        [DataMember]
        public decimal LowestCriticalValue { get; set; }
        [DataMember]
        public decimal HighestCriticalValue { get; set; }
        [DataMember]
        public string SensorUnit { get; set; }
        [DataMember]
        public bool IsSpecificSensor { get; set; }

    }

    [DataContract]
    public class SensorsDatas
    {
        [DataMember]
        public int DataID { get; set; }
        [DataMember]
        public int SensorID { get; set; }
        [DataMember]
        public decimal ReadValue { get; set; }
        [DataMember]
        public DateTime ReadValueTime { get; set; }
    }

    [DataContract]
    public class Countries
    {
        [DataMember]
        public int CountryID { get; set; }
        [DataMember]
        public string CountryName { get; set; }
        [DataMember]
        public string CountryPhoneCode { get; set; }
    }

    [DataContract]
    public class Cities
    {
        [DataMember]
        public int CityID { get; set; }
        [DataMember]
        public int CountryID { get; set; }
        [DataMember]
        public string CityName { get; set; }
        [DataMember]
        public int DistrictRelationID { get; set; }
    }

    [DataContract]
    public class Districts
    {
        [DataMember]
        public int DistrictID { get; set; }
        [DataMember]
        public int DistrictRelationID { get; set; }
        [DataMember]
        public string DistrictName { get; set; }
    }

    [DataContract]
    public class DirtyWords
    {
        [DataMember]
        public int DirtyWordID { get; set; }
        [DataMember]
        public string DirtyWord { get; set; }
    }

    [DataContract]
    public class UserTypes
    {
        [DataMember]
        public int UserTypeID { get; set; }
        [DataMember]
        public string UserTypeName { get; set; }
    }
}
