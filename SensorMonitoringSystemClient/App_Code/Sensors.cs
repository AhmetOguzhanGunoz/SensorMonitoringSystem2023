using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Web;

public class Sensors
{
    public int SensorID { get; set; }
    public int CompanyID { get; set; }
    public string SensorDescription { get; set; }
    public string SensorAddress { get; set; }
    public int GraphicalMinValue { get; set; }
    public int GraphicalMaxValue { get; set; }
    public decimal LowestCriticalValue { get; set; }
    public decimal HighestCriticalValue { get; set; }
    public string SensorUnit { get; set; }
    public bool IsSpecificSensor {  get; set; }
}