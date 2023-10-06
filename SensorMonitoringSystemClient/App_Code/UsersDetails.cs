using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

public class UsersDetails
{
    public int DetailID { get; set; }
    public int UserID { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string District { get; set; }
    public string Address { get; set; }
    public DateTime DateOfBirth { get; set; }
}