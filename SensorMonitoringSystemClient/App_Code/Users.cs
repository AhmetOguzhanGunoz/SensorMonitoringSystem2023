using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Users
{
    public int UserID { get; set; }
    public int CompanyID { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Username { get; set; }
    public byte[] Password { get; set; }
    public bool IsActivated { get; set; }
    public bool IsApproved { get; set; }
    public long RegistrationCode { get; set; }
    public System.Guid PasswordKey { get; set; }
    public string UserType { get; set; }
}