using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.IO;
using System.Net;

namespace SensorMonitoringSystemServiceCallerService
{
    public partial class SensorMonitoringSystemServiceCaller : ServiceBase
    {
        Timer timer = new Timer();
        const string wsUrl = "http://localhost:63420/SensorMonitoringSystemService.svc/rest/warningmail";
        private static WebClient WebClient = new WebClient()
        {
            Encoding = System.Text.Encoding.UTF8,
            Headers = new WebHeaderCollection()
        {
            { HttpRequestHeader.AcceptCharset, "UTF-8" },
            { "Content-Type", "application/json" }
        }
        };
        public SensorMonitoringSystemServiceCaller()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            WriteLogFile("Service is started");
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Interval = GetInterval();
            timer.Enabled = true;
            timer.Start();
        }

        protected override void OnStop()
        {
            WriteLogFile("Service is stopped");
        }

        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            string ApiData = new WebClient().DownloadString(wsUrl);
            WriteLogFile($"Web Api called : Api Data {ApiData} ");
            timer.Interval = GetInterval();
            timer.Start();
        }
        public void WriteLogFile(string message)
        {
            StreamWriter sw;
            sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\LogFile.txt", true);
            sw.WriteLine($"{DateTime.Now.ToString()} : {message}");
            sw.Flush();
            sw.Close();
        }
        public static double GetInterval()
        {
            DateTime now = DateTime.Now;
            return ((60 - now.Second) * 1000 - now.Millisecond);
        }
    }
}
