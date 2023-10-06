using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

public class Cities
{
    public int CityID { get; set; }
    public int CountryID { get; set; }
    public string CityName { get; set; }
    public int DistrictRelationID { get; set; }
}