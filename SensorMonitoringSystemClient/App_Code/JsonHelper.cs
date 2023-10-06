using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;


public class JsonHelper
{
    public static string Serialize<T>(T obj)
    {
        var serializer = new DataContractJsonSerializer(obj.GetType());
        var ms = new MemoryStream();
        serializer.WriteObject(ms, obj);
        string retVal = Encoding.UTF8.GetString(ms.ToArray());
        ms.Dispose();
        return retVal;
    }

    public static T Deserialize<T>(string json)
    {
        T obj = Activator.CreateInstance<T>();
        var ms = new MemoryStream(Encoding.Unicode.GetBytes(json));
        var serializer = new DataContractJsonSerializer(obj.GetType());
        obj = (T)serializer.ReadObject(ms);
        ms.Close();
        ms.Dispose();
        return obj;
    }
}