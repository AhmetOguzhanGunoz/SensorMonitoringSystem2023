
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace SensorMonitoringSystem
{

using System;
    using System.Collections.Generic;
    
public partial class SensorsDatas_AuditEntity
{

    public int AuditDataID { get; set; }

    public int DataID { get; set; }

    public int SensorID { get; set; }

    public decimal ReadValue { get; set; }

    public System.DateTime ReadValueTime { get; set; }

    public Nullable<int> OldDataID { get; set; }

    public Nullable<int> OldSensorID { get; set; }

    public Nullable<decimal> OldReadValue { get; set; }

    public Nullable<System.DateTime> OldReadValueTime { get; set; }

    public string ActionFrom { get; set; }

    public Nullable<System.DateTime> ActionDate { get; set; }

    public string ActionType { get; set; }

}

}
