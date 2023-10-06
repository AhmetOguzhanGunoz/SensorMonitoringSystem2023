using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using System.Configuration;
using System.Web.Mail;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Core;
using System.Runtime.Remoting.Contexts;
using System.Data.Entity.Infrastructure;
using System.Web;
using System.Net.Mime;
using System.Collections;
using System.Data.Entity;
using System.Runtime.InteropServices.ComTypes;
using System.Data.Entity.Migrations;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Web.Services;
using static System.Net.Mime.MediaTypeNames;
using System.Web.Services.Description;

namespace SensorMonitoringSystem
{

    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class SensorMonitoringSystemService : ISensorMonitoringSystemService
    {
        public List<Companies> FindAllCompanies()
        {
            SensorMonitoringSystemEntities SMSE = new SensorMonitoringSystemEntities();
            List<Companies> results = new List<Companies>();

            foreach (CompaniesEntity Companies in SMSE.Companies)
            {
                results.Add(new Companies()
                {
                    CompanyID = Companies.CompanyID,
                    CompanyName = Companies.CompanyName,
                });
            }

            return results;
        }

        public bool UsernameControl(string username)
        {
            SensorMonitoringSystemEntities SMSE = new SensorMonitoringSystemEntities();

            var UserEntity = (from p in SMSE.Users where p.Username == username select p).FirstOrDefault();

            if(UserEntity != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int RegisterUser(Users RegisteredUser)
        {
            SensorMonitoringSystemEntities SMSE = new SensorMonitoringSystemEntities();

            bool Activation = false;
            string UserKind = "Waiting For Approval";
            Random rnd = new Random();
            long RandomCodeNumber = rnd.Next(111111, 999999);

            SMSE.Users.Add(new UsersEntity
            {
                CompanyID = RegisteredUser.CompanyID,
                Name = RegisteredUser.Name,
                Surname = RegisteredUser.Surname,
                Username = RegisteredUser.Username,
                Password = RegisteredUser.Password,
                IsActivated = Activation,
                IsApproved = Activation,
                RegistrationCode = RandomCodeNumber,
                UserType = UserKind
            });

            int result = 0;
            try
            {
                result = SMSE.SaveChanges();
                return result;
            }
            catch
            {
                return result;
            }
        }

        public Users FindUser(string username)
        {
            SensorMonitoringSystemEntities SMSE = new SensorMonitoringSystemEntities();

            var UserEntity = (from p in SMSE.Users where p.Username == username select p).FirstOrDefault();

            if (UserEntity != null)
            {
                return new Users
                {
                    UserID = UserEntity.UserID,
                    CompanyID = UserEntity.CompanyID,
                    Name = UserEntity.Name,
                    Surname = UserEntity.Surname,
                    Username = UserEntity.Username,
                    IsActivated = UserEntity.IsActivated,
                    IsApproved = UserEntity.IsApproved,
                    RegistrationCode = UserEntity.RegistrationCode,
                    UserType = UserEntity.UserType
                };
            }
            else
                return null;

        }

        public int RegisterUserDetail(UsersDetails RegisteredUserDetail)
        {
            SensorMonitoringSystemEntities SMSE = new SensorMonitoringSystemEntities();

            SMSE.UsersDetails.Add(new UsersDetailsEntity
            {
                UserID = RegisteredUserDetail.UserID,
                PhoneNumber  = RegisteredUserDetail.PhoneNumber,
                Email = RegisteredUserDetail.Email,
                Country = RegisteredUserDetail.Country,
                City = string.IsNullOrEmpty(RegisteredUserDetail.City) ? "No Recorded City" : RegisteredUserDetail.City, //No cities for country
                District = string.IsNullOrEmpty(RegisteredUserDetail.District) ? "No Recorded District" : RegisteredUserDetail.District, //No districts for city
                Address = RegisteredUserDetail.Address,
                DateOfBirth = RegisteredUserDetail.DateOfBirth                
            });

            int result = 0;
            try
            {
                result = SMSE.SaveChanges();
                return result;
            }
            catch
            {
                return result;
            }
        }

        public bool UserDetailControl(string userid) //Not gonna need
        {
            SensorMonitoringSystemEntities SMSE = new SensorMonitoringSystemEntities();
            int IntID = int.Parse(userid);

            var UserDetailEntity = (from p in SMSE.UsersDetails where p.UserID == IntID select p).FirstOrDefault();

            if (UserDetailEntity != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SendWarningMail()
        {
            SensorMonitoringSystemEntities SMSE = new SensorMonitoringSystemEntities();

            var WarningEntity = (from p in SMSE.CriticValueChecker() orderby p.ActionDate ascending select p).ToList();

            if(WarningEntity != null)
            {
                foreach(var data in WarningEntity)
                {
                    string body = string.Empty;
                    using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/WarningTemplate.html")))
                    {
                        body = reader.ReadToEnd();
                    }

                    body = body.Replace("UserName", data.Username.ToString());
                    body = body.Replace("SensorDescription", data.SensorDescription.ToString());
                    body = body.Replace("ReadValue", data.ReadValue.ToString());
                    body = body.Replace("ActionDate", data.ActionDate.ToString());

                    //Sending Embedded picture in html mail template
                    AlternateView avHtml = AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html);
                    LinkedResource pic1 = new LinkedResource(HttpContext.Current.Server.MapPath("~/logo.jpg"), MediaTypeNames.Image.Jpeg);
                    pic1.ContentId = "Pic1";
                    avHtml.LinkedResources.Add(pic1);

                    var fromAddress = new MailAddress(ConfigurationManager.AppSettings["MailSender"], "TechExpert Account Support Team");
                    var toAddress = new MailAddress(data.Email, data.Name + " " + data.Surname);
                    const string subject = "Sensor Monitoring System Warning Mail";

                    var smtp = new SmtpClient
                    {
                        Host = ConfigurationManager.AppSettings["Host"],
                        Port = int.Parse(ConfigurationManager.AppSettings["Port"]),
                        EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]),
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        Credentials = new NetworkCredential(ConfigurationManager.AppSettings["MailSender"], ConfigurationManager.AppSettings["MailSenderPassword"]),
                        Timeout = 20000
                    };

                    using (var message = new System.Net.Mail.MailMessage(fromAddress, toAddress)
                    {
                        Subject = subject,
                        IsBodyHtml = true,
                        BodyEncoding = Encoding.UTF8,
                        SubjectEncoding = Encoding.UTF8,
                    })
                    {
                        message.AlternateViews.Add(avHtml);
                        try
                        {
                            smtp.Send(message);
                            int IntID = Int32.Parse(data.AuditDataID.ToString());
                            var AuditEntity = (from p in SMSE.SensorsDatas_Audit where p.AuditDataID == IntID select p).FirstOrDefault();
                            if (AuditEntity != null) 
                            {
                                AuditEntity.ActionType = "INSERTWARN";
                                SMSE.SaveChanges();
                            }
                        }
                        catch
                        {

                        }
                    }
                }
            }
        }

        public int SendInfoMail(UsersDetails RegisteredUserDetail)
        {
            SensorMonitoringSystemEntities SMSE = new SensorMonitoringSystemEntities();

            var UserEntity = (from p in SMSE.Users where p.UserID == RegisteredUserDetail.UserID select new {p.Name,p.Surname,p.Username,p.RegistrationCode}).FirstOrDefault();
            var UserDetailEntity = (from p in SMSE.UsersDetails where p.UserID == RegisteredUserDetail.UserID select new {p.Email}).FirstOrDefault();

            string body = string.Empty;
            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/MailTemplate.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("UserName", UserEntity.Username);
            body = body.Replace("RegistrationCode", UserEntity.RegistrationCode.ToString());
            //Sending Embedded picture in html mail template
            AlternateView avHtml = AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html);
            LinkedResource pic1 = new LinkedResource(HttpContext.Current.Server.MapPath("~/logo.jpg"), MediaTypeNames.Image.Jpeg);
            pic1.ContentId = "Pic1";
            avHtml.LinkedResources.Add(pic1);

            var fromAddress = new MailAddress(ConfigurationManager.AppSettings["MailSender"], "TechExpert Account Support Team");
            var toAddress = new MailAddress(UserDetailEntity.Email, UserEntity.Name + " " + UserEntity.Surname);
            const string subject = "Sensor Monitoring System Registration";

            var smtp = new SmtpClient
            {
                Host = ConfigurationManager.AppSettings["Host"],
                Port = int.Parse(ConfigurationManager.AppSettings["Port"]),
                EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(ConfigurationManager.AppSettings["MailSender"], ConfigurationManager.AppSettings["MailSenderPassword"]),
                Timeout = 20000
            };

            using (var message = new System.Net.Mail.MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                IsBodyHtml = true,
                BodyEncoding = Encoding.UTF8,
                SubjectEncoding = Encoding.UTF8,
            })
            {
                int result = 0;
                message.AlternateViews.Add(avHtml);
                try
                {
                    smtp.Send(message);
                    result = 1;
                    return result;
                }
                catch
                {
                    return result;
                }
                
            }
        }

        public int ChangePassword(Users ChangedPasswordUser)
        {
            SensorMonitoringSystemEntities SMSE = new SensorMonitoringSystemEntities();

            var UserEntity = (from p in SMSE.Users where p.UserID == ChangedPasswordUser.UserID select p).FirstOrDefault();

            SMSE.Entry(UserEntity).CurrentValues.SetValues(new UsersEntity()
            {
                UserID = UserEntity.UserID,
                CompanyID = UserEntity.CompanyID,
                Name = UserEntity.Name,
                Surname = UserEntity.Surname,
                Username = UserEntity.Username,
                Password = ChangedPasswordUser.Password,
                IsActivated = UserEntity.IsActivated,
                IsApproved = UserEntity.IsApproved,
                RegistrationCode = UserEntity.RegistrationCode,
                UserType = UserEntity.UserType,
            });

            int result = 0;
            try
            {
                result = SMSE.SaveChanges();
                return result;
            }
            catch
            {
                return result;
            }
        }

        public int Activation(Users ActivatedUser)
        {
            SensorMonitoringSystemEntities SMSE = new SensorMonitoringSystemEntities();

            var UserEntity = (from p in SMSE.Users where p.UserID == ActivatedUser.UserID select p).FirstOrDefault();

            SMSE.Entry(UserEntity).CurrentValues.SetValues(new UsersEntity()
            {
                UserID = UserEntity.UserID,
                CompanyID = UserEntity.CompanyID,
                Name = UserEntity.Name,
                Surname = UserEntity.Surname,
                Username = UserEntity.Username,
                Password = UserEntity.Password,
                IsActivated = true,
                IsApproved = UserEntity.IsApproved,
                RegistrationCode = UserEntity.RegistrationCode,
                UserType = UserEntity.UserType,
            });

            int result = 0;
            try
            {
                result = SMSE.SaveChanges();
                return result;
            }
            catch
            {
                return result;
            }
        }

        public int Approval(Users ApprovedUser)
        {
            SensorMonitoringSystemEntities SMSE = new SensorMonitoringSystemEntities();

            var UserEntity = (from p in SMSE.Users where p.UserID == ApprovedUser.UserID select p).FirstOrDefault();

            SMSE.Entry(UserEntity).CurrentValues.SetValues(new UsersEntity()
            {
                UserID = UserEntity.UserID,
                CompanyID = UserEntity.CompanyID,
                Name = UserEntity.Name,
                Surname = UserEntity.Surname,
                Username = UserEntity.Username,
                Password = UserEntity.Password,
                IsActivated = UserEntity.IsActivated,
                IsApproved = true,
                RegistrationCode = UserEntity.RegistrationCode,
                UserType = ApprovedUser.UserType,
            });

            int result = SMSE.SaveChanges();
            return result;
        }

        public UsersDetails FindUserDetail(string userid)
        {
            SensorMonitoringSystemEntities SMSE = new SensorMonitoringSystemEntities();
            int ID = int.Parse(userid);

            var UserDetailEntity = (from p in SMSE.UsersDetails where p.UserID == ID select p).FirstOrDefault();

            if (UserDetailEntity != null)
            {
                return new UsersDetails
                {
                    DetailID = UserDetailEntity.DetailID,
                    UserID = UserDetailEntity.UserID,
                    PhoneNumber = UserDetailEntity.PhoneNumber,
                    Email = UserDetailEntity.Email,
                    Country  = UserDetailEntity.Country,
                    City = UserDetailEntity.City,
                    District = UserDetailEntity.District,
                    Address = UserDetailEntity.Address,
                    DateOfBirth = UserDetailEntity.DateOfBirth                   
                };
            }
            else
                return null;

        }

        public int ChangeCode(Users ChangedCodeUser)
        {
            SensorMonitoringSystemEntities SMSE = new SensorMonitoringSystemEntities();
            Random rnd = new Random();
            long RandomCodeNumber = rnd.Next(111111, 999999);

            var UserEntity = (from p in SMSE.Users where p.UserID == ChangedCodeUser.UserID select p).FirstOrDefault();

            SMSE.Entry(UserEntity).CurrentValues.SetValues(new UsersEntity()
            {
                UserID = UserEntity.UserID,
                CompanyID = UserEntity.CompanyID,
                Name = UserEntity.Name,
                Surname = UserEntity.Surname,
                Username = UserEntity.Username,
                Password = UserEntity.Password,
                IsActivated = UserEntity.IsActivated,
                IsApproved = UserEntity.IsApproved,
                RegistrationCode = RandomCodeNumber,
                UserType = UserEntity.UserType
            });

            int result = 0;
            try
            {
                result = SMSE.SaveChanges();
                return result;
            }
            catch
            {
                return result;
            }
        }

        public List<Sensors> FindCompaniesAllSensors(string companyid)
        {
            SensorMonitoringSystemEntities SMSE = new SensorMonitoringSystemEntities();
            List<Sensors> results = new List<Sensors>();
            int IntID = Int32.Parse(companyid);

            var SensorsEntity = (from p in SMSE.Sensors where p.CompanyID == IntID select p);

            if (SensorsEntity != null)
            {

                foreach (SensorsEntity Sensors in SensorsEntity)
                {
                    results.Add(new Sensors()
                    {
                        SensorID = Sensors.SensorID,
                        CompanyID = Sensors.CompanyID,
                        SensorDescription = Sensors.SensorDescription,
                        SensorAddress = Sensors.SensorAddress,
                        GraphicalMinValue = Sensors.GraphicalMinValue,
                        GraphicalMaxValue = Sensors.GraphicalMaxValue,
                        LowestCriticalValue = Sensors.LowestCriticalValue,
                        HighestCriticalValue = Sensors.HighestCriticalValue,
                        SensorUnit = Sensors.SensorUnit,
                        IsSpecificSensor = Sensors.IsSpecificSensor,
                    });
                }

                return results;
            }
            else
                return null;
        }

        public bool SensorDataControl(string sensorid)
        {
            SensorMonitoringSystemEntities SMSE = new SensorMonitoringSystemEntities();
            int IntID = Int32.Parse(sensorid);

            var SensorDataEntity = (from p in SMSE.SensorsDatas where p.SensorID == IntID select p).FirstOrDefault();

            if (SensorDataEntity != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Sensors FindSensor(string sensorid)
        {
            SensorMonitoringSystemEntities SMSE = new SensorMonitoringSystemEntities();
            int IntID = Int32.Parse(sensorid);

            var SensorEntity = (from p in SMSE.Sensors where p.SensorID == IntID select p).FirstOrDefault();

            if (SensorEntity != null)
            {
                return new Sensors()
                {
                    SensorID = SensorEntity.SensorID,
                    CompanyID = SensorEntity.CompanyID,
                    SensorDescription = SensorEntity.SensorDescription,
                    SensorAddress = SensorEntity.SensorAddress,
                    GraphicalMinValue = SensorEntity.GraphicalMinValue,
                    GraphicalMaxValue = SensorEntity.GraphicalMaxValue,
                    LowestCriticalValue = SensorEntity.LowestCriticalValue,
                    HighestCriticalValue = SensorEntity.HighestCriticalValue,
                    SensorUnit = SensorEntity.SensorUnit,
                    IsSpecificSensor = SensorEntity.IsSpecificSensor,
                };
            }
            else
                return null;
        }

        public List<SensorsDatas> FindAllSensorDatas(string sensorid)
        {
            SensorMonitoringSystemEntities SMSE = new SensorMonitoringSystemEntities();
            List<SensorsDatas> results = new List<SensorsDatas>();
            int IntID = Int32.Parse(sensorid);

            var SensorDatasEntity = (from p in SMSE.SensorsDatas where p.SensorID == IntID select p);

            if (SensorDatasEntity != null)
            {
                foreach (SensorsDatasEntity SensorDatas in SensorDatasEntity)
                {
                    results.Add(new SensorsDatas()
                    {
                        DataID = SensorDatas.DataID,
                        SensorID = SensorDatas.SensorID,
                        ReadValue = SensorDatas.ReadValue,
                        ReadValueTime = SensorDatas.ReadValueTime
                    });
                }

                return results;
            }
            else
                return null;
        }

        public List<SensorsDatas> FindDatasBetweenDates(string sensorid, string startdate, string enddate)
        {
            SensorMonitoringSystemEntities SMSE = new SensorMonitoringSystemEntities();
            List<SensorsDatas> results = new List<SensorsDatas>();

            int SensorID = Int32.Parse(sensorid);
            DateTime StartDate = new DateTime(long.Parse(startdate));
            DateTime EndDate = new DateTime(long.Parse(enddate));

            var SensorDatasEntity = (from p in SMSE.DatasBetweenDates(SensorID, StartDate, EndDate) orderby p.ReadValueTime ascending select p);

            if (SensorDatasEntity != null)
            {
                foreach (var datas in SensorDatasEntity)
                {
                    results.Add(new SensorsDatas()
                    {
                        DataID = datas.DataID,
                        SensorID = datas.SensorID,
                        ReadValue = datas.ReadValue,
                        ReadValueTime = datas.ReadValueTime
                    });
                }

                return results;
            }
            else
                return null;
        }

        public List<Countries> FindAllCountries()
        {
            SensorMonitoringSystemEntities SMSE = new SensorMonitoringSystemEntities();
            List<Countries> results = new List<Countries>();

            foreach (CountriesEntity Countries in SMSE.Countries)
            {
                results.Add(new Countries()
                {
                    CountryID = Countries.CountryID,
                    CountryName = Countries.CountryName,
                    CountryPhoneCode = Countries.CountryPhoneCode
                });
            }

            return results;

        }

        public List<Cities> FindCitiesOfCountry(string countryid)
        {
            SensorMonitoringSystemEntities SMSE = new SensorMonitoringSystemEntities();
            List<Cities> results = new List<Cities>();
            int IntID = Int32.Parse(countryid);

            var CitiesEntity = (from p in SMSE.Cities where p.CountryID == IntID select p);

            if (CitiesEntity != null)
            {

                foreach (CitiesEntity Cities in CitiesEntity)
                {
                    results.Add(new Cities()
                    {
                        CityID = Cities.CityID,
                        CountryID = Cities.CountryID,
                        CityName = Cities.CityName,
                        DistrictRelationID = Cities.DistrictRelationID
                    });
                }

                return results;
            }
            else
                return null;
        }

        public List<Districts> FindDistrictsOfCity(string districtrelationid)
        {
            SensorMonitoringSystemEntities SMSE = new SensorMonitoringSystemEntities();
            List<Districts> results = new List<Districts>();
            int IntID = Int32.Parse(districtrelationid);

            var DistrictsEntity = (from p in SMSE.Districts where p.DistrictRelationID == IntID select p);

            if (DistrictsEntity != null)
            {

                foreach (DistrictsEntity Districts in DistrictsEntity)
                {
                    results.Add(new Districts()
                    {
                        DistrictID = Districts.DistrictID,
                        DistrictRelationID = Districts.DistrictRelationID,
                        DistrictName = Districts.DistrictName
                    });
                }

                return results;
            }
            else
                return null;
        }

        public bool UserLogin(string username, string password)
        {
            SensorMonitoringSystemEntities SMSE = new SensorMonitoringSystemEntities();
            var UsernameBytes = Convert.FromBase64String(username);
            var UsernameReal = System.Text.Encoding.UTF8.GetString(UsernameBytes);
            var PasswordBytes = Convert.FromBase64String(password);
            var PasswordReal = System.Text.Encoding.UTF8.GetString(PasswordBytes);

            int result = 0;
            try 
            {
                var LoginResult = SMSE.UserLogin(UsernameReal, Encoding.UTF8.GetBytes(PasswordReal)).FirstOrDefault().Value;
                return Convert.ToBoolean(LoginResult);
            }
            catch
            {
                return Convert.ToBoolean(result);
            }
        }

        public int AddSensortoCompany(Sensors RegisteredSensor)
        {
            SensorMonitoringSystemEntities SMSE = new SensorMonitoringSystemEntities();

            int result = 0;
            try
            {
                result = SMSE.SensorRegistration(RegisteredSensor.CompanyID, RegisteredSensor.SensorDescription, RegisteredSensor.SensorAddress,
                    RegisteredSensor.GraphicalMinValue, RegisteredSensor.GraphicalMaxValue, RegisteredSensor.LowestCriticalValue, RegisteredSensor.HighestCriticalValue,
                    RegisteredSensor.SensorUnit, RegisteredSensor.IsSpecificSensor).FirstOrDefault().Value;
                return (result > 0) ? (1) : result;
            }
            catch
            {
                return result;
            }
        }

        public int AddSensorsDatastoCompanysSensors(SensorsDatas RegisteredSensorsDatas)
        {
            SensorMonitoringSystemEntities SMSE = new SensorMonitoringSystemEntities();

            int result = 0;
            try
            {
                result = SMSE.DataRegistration(RegisteredSensorsDatas.ReadValue, RegisteredSensorsDatas.ReadValueTime, RegisteredSensorsDatas.SensorID).FirstOrDefault().Value;
                return (result > 0) ? (1) : result;
            }
            catch
            {
                return result;
            }
        }
        public List<DirtyWords> FindAllDirtyWords()
        {
            SensorMonitoringSystemEntities SMSE = new SensorMonitoringSystemEntities();
            List<DirtyWords> results = new List<DirtyWords>();

            foreach (DirtyWordsEntity DirtyWords in SMSE.DirtyWords)
            {
                results.Add(new DirtyWords()
                {
                    DirtyWordID = DirtyWords.DirtyWordID,
                    DirtyWord = DirtyWords.DirtyWord
                });
            }

            return results;

        }

        public int DeleteUser(string UserID)
        {
            SensorMonitoringSystemEntities SMSE = new SensorMonitoringSystemEntities();
            int IntID = Int32.Parse(UserID);

            var UserEntity = (from p in SMSE.Users where p.UserID == IntID select p).FirstOrDefault();

            int result = 0;

            if (Convert.ToBoolean(DeleteUserDetail(UserID)) && UserEntity != null)
            {
                try
                {
                    SMSE.Users.Remove(UserEntity);
                    result = SMSE.SaveChanges();
                    return result;
                }
                catch
                {
                    return result;
                }
            }
            else
            {
                return result;
            }
        }

        public int DeleteUserDetail(string UserID)
        {
            SensorMonitoringSystemEntities SMSE = new SensorMonitoringSystemEntities();
            int IntID = Int32.Parse(UserID);

            var UserDetailEntity = (from p in SMSE.UsersDetails where p.UserID == IntID select p).FirstOrDefault();

            int result = 0;

            if (UserDetailEntity != null)
            {
                try
                {
                    SMSE.UsersDetails.Remove(UserDetailEntity);
                    result = SMSE.SaveChanges();
                    return result;
                }
                catch
                {
                    return result;
                }
            }
            else
            {
                return result;
            }
        }

        public List<UserTypes> FindAllUserTypes()
        {
            SensorMonitoringSystemEntities SMSE = new SensorMonitoringSystemEntities();
            List<UserTypes> results = new List<UserTypes>();

            foreach (UserTypesEntity UserTypes in SMSE.UserTypes)
            {
                if(UserTypes.UserTypeName != "System Admin")
                {
                    results.Add(new UserTypes()
                    {
                        UserTypeID = UserTypes.UserTypeID,
                        UserTypeName = UserTypes.UserTypeName,
                    });
                }
            }
            return results;
        }

        public List<Users> FindCompanysAllActivatednotApprovedUsers(string CompanyID)
        {
            SensorMonitoringSystemEntities SMSE = new SensorMonitoringSystemEntities();
            List<Users> results = new List<Users>();
            int IntID = Int32.Parse(CompanyID);

            var UsersEntity = (from p in SMSE.Users where p.CompanyID == IntID && p.IsActivated == true && p.IsApproved == false && p.UserType != "System Admin" select p);

            if (UsersEntity != null)
            {
                foreach (UsersEntity Users in UsersEntity)
                {
                    results.Add(new Users()
                    {
                        UserID = Users.UserID,
                        CompanyID = Users.CompanyID,
                        Name = Users.Name,
                        Surname = Users.Surname,
                        Username = Users.Username,
                        IsActivated = Users.IsActivated,
                        IsApproved = Users.IsApproved,
                        RegistrationCode = Users.RegistrationCode,
                        UserType = Users.UserType
                    });
                }
                return results;
            }
            else
                return null;
        }

        public List<Sensors> FindCompanysAllSpecificSensors(string CompanyID)
        {
            SensorMonitoringSystemEntities SMSE = new SensorMonitoringSystemEntities();
            List<Sensors> results = new List<Sensors>();
            int IntID = Int32.Parse(CompanyID);

            var SensorEntity = (from p in SMSE.Sensors where p.CompanyID == IntID && p.IsSpecificSensor == true select p);

            if (SensorEntity != null)
            {
                foreach (SensorsEntity Sensors in SensorEntity)
                {
                    results.Add(new Sensors()
                    {
                        SensorID = Sensors.SensorID,
                        CompanyID = Sensors.CompanyID,
                        SensorDescription = Sensors.SensorDescription,
                        SensorAddress = Sensors.SensorAddress,
                        GraphicalMinValue = Sensors.GraphicalMinValue,
                        GraphicalMaxValue = Sensors.GraphicalMaxValue,
                        LowestCriticalValue = Sensors.LowestCriticalValue,
                        HighestCriticalValue = Sensors.HighestCriticalValue,
                        SensorUnit = Sensors.SensorUnit,
                        IsSpecificSensor = Sensors.IsSpecificSensor
                    });
                }
                return results;
            }
            else
                return null;
        }

        public List<Users> FindAllCompanysUsers(string CompanyID)
        {
            SensorMonitoringSystemEntities SMSE = new SensorMonitoringSystemEntities();
            List<Users> results = new List<Users>();
            int IntID = Int32.Parse(CompanyID);

            var UsersEntity = (from p in SMSE.Users where p.CompanyID == IntID && p.IsActivated == true && p.IsApproved == true && p.UserType != "System Admin" select p);

            if (UsersEntity != null)
            {
                foreach (UsersEntity Users in UsersEntity)
                {
                    results.Add(new Users()
                    {
                        UserID = Users.UserID,
                        CompanyID = Users.CompanyID,
                        Name = Users.Name,
                        Surname = Users.Surname,
                        Username = Users.Username,
                        IsActivated = Users.IsActivated,
                        IsApproved = Users.IsApproved,
                        RegistrationCode = Users.RegistrationCode,
                        UserType = Users.UserType
                    });
                }
                return results;
            }
            else
                return null;
        }

        public int DeleteSensorData(string SensorID)
        {
            SensorMonitoringSystemEntities SMSE = new SensorMonitoringSystemEntities();
            int IntID = Int32.Parse(SensorID);

            var SensorDataEntity = (from p in SMSE.SensorsDatas where p.SensorID == IntID select p).ToList();

            int result = 0;

            if (SensorDataEntity != null && SensorDataEntity.Count != 0)
            {
                foreach (SensorsDatasEntity SensorData in SensorDataEntity)
                {
                    try
                    {
                        SMSE.SensorsDatas.Remove(SensorData);
                        result = SMSE.SaveChanges();
                    }
                    catch
                    {
                        result = 0;
                        return result;
                    }
                }
                return result;
            }
            else
            {
                return result;
            }
        }

        public int DeleteSensor(string SensorID)
        {
            SensorMonitoringSystemEntities SMSE = new SensorMonitoringSystemEntities();
            int IntID = Int32.Parse(SensorID);
            int result = 0;
            var SensorEntity = (from p in SMSE.Sensors where p.SensorID == IntID select p).FirstOrDefault();
            var SensorDataEntity = (from p in SMSE.SensorsDatas where p.SensorID == IntID select p).ToList();

            if(SensorDataEntity.Count != 0) 
            {
                if (SensorEntity != null)
                {
                    try
                    {
                        SMSE.SensorsDatas.RemoveRange(SensorEntity.SensorsDatas);
                        SMSE.Sensors.Remove(SensorEntity);
                        result = SMSE.SaveChanges();
                        return result;
                    }
                    catch
                    {
                        result = 0;
                        return result;
                    }
                }
                else
                {
                    return result;
                }
            }
            else
            {
                if(SensorEntity != null)
                {
                    try
                    {
                        SMSE.Sensors.Remove(SensorEntity);
                        result = SMSE.SaveChanges();
                        return result;
                    }
                    catch
                    {
                        result = 0;
                        return result;
                    }
                }
                else
                {
                    return result;
                }
            }
        }

        public int DeleteSensorDataByID(string DataID)
        {
            SensorMonitoringSystemEntities SMSE = new SensorMonitoringSystemEntities();
            int IntID = Int32.Parse(DataID);

            var SensorDataEntity = (from p in SMSE.SensorsDatas where p.DataID == IntID select p).FirstOrDefault();

            int result = 0;

            if (SensorDataEntity != null)
            {
                try
                {
                    SMSE.SensorsDatas.Remove(SensorDataEntity);
                    result = SMSE.SaveChanges();
                    return result;
                }
                catch
                {
                    return result;
                }
            }
            else
            {
                return result;
            }
        }

        public int DeleteDatasBetweenDates(string sensorid, string startdate, string enddate)
        {
            SensorMonitoringSystemEntities SMSE = new SensorMonitoringSystemEntities();
            List<SensorsDatas> results = new List<SensorsDatas>();

            int SensorID = Int32.Parse(sensorid);
            DateTime StartDate = new DateTime(long.Parse(startdate));
            DateTime EndDate = new DateTime(long.Parse(enddate));

            var SensorDatasEntity = (from p in SMSE.SensorsDatas where p.SensorID == SensorID && p.ReadValueTime >= StartDate && p.ReadValueTime <= EndDate select p);

            int result = 0;

            if (SensorDatasEntity != null)
            {
                SMSE.SensorsDatas.RemoveRange(SensorDatasEntity);
                result = SMSE.SaveChanges();
                return (result > 0) ? (1) : result;
            }
            else
                return result;
        }
    }
}
