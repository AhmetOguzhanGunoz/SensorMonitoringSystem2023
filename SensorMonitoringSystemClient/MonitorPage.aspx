<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MonitorPage.aspx.cs" Inherits="MonitorPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="ClientStyle.css" rel="stylesheet" />
    <title>Analyze Sensor Datas over Sensor Monitoring System Site</title>
    <meta http-equiv="Refresh" content="120" />
    <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
    <script type="text/javascript">
        function DisableRequestButtonsandShowLabeltobeReadTimer()
        {
            var RequestButtonsTimeLeft = 10;
            var LabelMessageTimeLeft = 4;

            var WriteDatatoExcelbtn = document.getElementById("<%=WriteDatatoExcelbtn.ClientID%>");
            var DeleteSensorDatabtn = document.getElementById("<%=DeleteSensorDatabtn.ClientID%>");
            var Displaybtn = document.getElementById("<%=Displaybtn.ClientID%>");
            var StartDatetxt = document.getElementById("<%=StartDatetxt.ClientID%>");

            if (!StartDatetxt.disabled)
            {
                WriteDatatoExcelbtn.disabled = true;
                DeleteSensorDatabtn.disabled = true;
                Displaybtn.disabled = true;
            }

            var Infolbl = document.getElementById("<%=Infolbl.ClientID%>");
            var TextInInfolbl = Infolbl.innerText;

            if (Infolbl.innerHTML !== "")
            {
                var LabelMessageTimer = setInterval(function () {
                    if (LabelMessageTimeLeft <= 0) {
                        clearInterval(LabelMessageTimer);
                        var RequestButtonstimer = setInterval(function () {
                            if (!StartDatetxt.disabled)
                            {
                                Infolbl.innerHTML = "Please wait " + RequestButtonsTimeLeft.toString() + " seconds for activation of display, write and delete data button.";
                            }
                            if (RequestButtonsTimeLeft <= 0) {
                                clearInterval(RequestButtonstimer);
                                if (!StartDatetxt.disabled)
                                {
                                    WriteDatatoExcelbtn.disabled = false;
                                    DeleteSensorDatabtn.disabled = false;
                                    Displaybtn.disabled = false;
                                }
                                Infolbl.innerHTML = TextInInfolbl;
                            }
                            RequestButtonsTimeLeft--;
                        }, 1000);
                    }
                    LabelMessageTimeLeft--;
                }, 400);
            }
            else
            {
                var LabelMessageTimer = setInterval(function () {
                    if (LabelMessageTimeLeft <= 0) {
                        clearInterval(LabelMessageTimer);
                        var RequestButtonstimer = setInterval(function () {
                            if (!StartDatetxt.disabled) {
                                Infolbl.innerHTML = "Please wait " + RequestButtonsTimeLeft.toString() + " seconds for activation of display, write and delete data button.";
                            }
                            if (RequestButtonsTimeLeft <= 0) {
                                clearInterval(RequestButtonstimer);
                                if (!StartDatetxt.disabled)
                                {
                                    WriteDatatoExcelbtn.disabled = false;
                                    DeleteSensorDatabtn.disabled = false;
                                    Displaybtn.disabled = false;
                                }
                                Infolbl.innerHTML = TextInInfolbl;
                            }
                            RequestButtonsTimeLeft--;
                        }, 1000);
                    }
                    LabelMessageTimeLeft--;
                }, 400);
            }
        }

        function LoadingScreen()
        {
            var Loading = document.getElementById('loading');
            var Banner = document.getElementById('banner');
            var Content = document.getElementById('content');
            var Footer = document.getElementById('footer');
            Banner.style.opacity = 0.5;
            Content.style.opacity = 0.5;
            Footer.style.opacity = 0.5;
            Loading.style.display = "block";
            setTimeout(
                function () {
                    Banner.style.opacity = 1;
                    Content.style.opacity = 1;
                    Footer.style.opacity = 1;
                    Loading.style.display = "none";
                }, 20000
            );
        }

        function DataUploadProcess()
        {
            var DataUpload1 = document.getElementById('DataUpload1');
            var Infolbl = document.getElementById("<%=Infolbl.ClientID%>");
            var AddFromExcelbtn = document.getElementById("<%=AddFromExcelbtn.ClientID%>");
            if (AddFromExcelbtn.value != "Load Datas to Sensor")
            {
                Infolbl.innerText = "Please select file for adding datas to sensor.(.xlsx allowed, file must contains 1 sheet and must be in text format without data headers.)";
                DataUpload1.click();
                return false;
            }
            else
            {
                return true;
            }
        }

        function confirmationWrite()
        {
            var Infolbl = document.getElementById("<%=Infolbl.ClientID%>");
            var result = confirm("Are you sure to make this process?");
            if (result)
            {
                Infolbl.innerText = "File created and sent to your downloaded folder.";
                return true;
            }
            else
            {
                return false;
            }
        }

        function confirmationDelete()
        {
            return confirm("Are you sure to make this process?");
        }


    </script>
</head>
<body onload="DisableRequestButtonsandShowLabeltobeReadTimer();">
    <form id="form1" runat="server" onsubmit="LoadingScreen();">
        <div id="loading" style="display:none"></div>
            <div id="banner">
                <div id="bannerpart1">
                    <img src="logo.png" alt="techexpertlogo"/>
                </div>
                <div id="bannerpart2">
                    <h3>Sensor Monitoring System</h3>
                    <p>depends to trustworthy sensor datas</p>
                </div>
            </div>

            <div id="content">
                <br />
                <asp:Label ID="Infolbl" Width="100%" style="font-size: calc(0.6vw + 0.4vh);" runat="server" Text=""></asp:Label>
                <br />
                <br />
                <div id="DataSelectionArea">
                    <asp:Table ID="DateSelectionTable1" runat="server" Width="100%" CellPadding="0" CellSpacing="10">
                        <asp:TableRow>
                            <asp:TableCell Width="10%"><asp:Label ID="StartDatelbl" style="font-size: calc(0.6vw + 0.4vh);"  runat="server" Text="StartDate: "></asp:Label></asp:TableCell>
                            <asp:TableCell Width="40%"><asp:TextBox ID="StartDatetxt" TextMode="DateTimeLocal" CssClass="inputclass" Width="100%" style="font-size:calc(0.6vw + 0.4vh);" runat="server"></asp:TextBox></asp:TableCell>
                            <asp:TableCell Width="10%"><asp:Label ID="EndDatelbl" style="font-size: calc(0.6vw + 0.4vh);" runat="server" Text="EndDate:"></asp:Label></asp:TableCell>
                            <asp:TableCell Width="40%"><asp:TextBox ID="EndDatetxt" TextMode="DateTimeLocal" CssClass="inputclass" Width="100%" style="font-size: calc(0.6vw + 0.4vh);" runat="server"></asp:TextBox></asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
                <div id="DataChangingArea">
                    <asp:Table ID="ChangeButtonsTable1" runat="server" Width="100%" CellPadding="0" CellSpacing="10" style="align-self:center; margin:auto;">
                        <asp:TableRow>
                            <asp:TableCell Width="20%"><asp:Button ID="Displaybtn" CssClass="buttonclass" style="font-size: calc(0.5vw + 0.3vh); width:calc(6vw + 4vh);"  runat="server" Text="Display Selected Data" OnClick="Displaybtn_Click"/></asp:TableCell>
                            <asp:TableCell Width="20%"><asp:Button ID="AddFromExcelbtn" CssClass="buttonclass" style="font-size: calc(0.5vw + 0.3vh); width:calc(6vw + 4vh);"  runat="server" Text="Add Data from Excel File" OnClientClick="return DataUploadProcess();" OnClick="AddFromExcelbtn_Click"/></asp:TableCell>
                            <asp:TableCell Width="20%"><asp:Button ID="WriteDatatoExcelbtn" CssClass="buttonclass" style="font-size: calc(0.5vw + 0.3vh); width:calc(6vw + 5.5vh);" runat="server" Text="Write Selected Data to File" OnClientClick="return confirmationWrite();" OnClick="WriteDatatoExcelbtn_Click"/></asp:TableCell>
                            <asp:TableCell Width="20%"><asp:Button ID="DeleteSensorDatabtn" CssClass="buttonclass" style="font-size: calc(0.5vw + 0.3vh); width:calc(6vw + 4vh);" runat="server" Text="Delete Selected Data" OnClientClick="return confirmationDelete();" OnClick="DeleteSensorDatabtn_Click"/></asp:TableCell>
                            <asp:TableCell Width="20%"><asp:Button ID="ReturnProfilebtn" CssClass="buttonclass" style="font-size: calc(0.5vw + 0.3vh); width:calc(6vw + 4vh);" runat="server" Text="Return to Profile Page" OnClick="ReturnProfilebtn_Click" OnClientClick="ReturnProfilebtn_Click"/></asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>                        
                </div>
                <div id="chart_div"></div>
                <asp:FileUpload ID="DataUpload1" ClientIDMode="Static" runat="server" style="display:none;" onchange="this.form.submit()"/>
            </div>
            <div id="footer">
                <p>TechExpert Information Technologies and Automation Engineering Trade Limited Company <br/> 
                    This site was made by Ahmet Oğuzhan Günöz - Copyright © 2017 - Modified 2023 - All Rights Reserved</p>
            </div>
    <script type="text/javascript">

        var SensorName = <%= jsSerializer.Serialize(ObtainedSensor.SensorDescription)%>;
        var SensorUnit = <%= jsSerializer.Serialize(ObtainedSensor.SensorUnit)%>;
        var GraphicalMinValue = <%= jsSerializer.Serialize(ObtainedSensor.GraphicalMinValue)%>;
        var GraphicalMaxValue = <%= jsSerializer.Serialize(ObtainedSensor.GraphicalMaxValue)%>;
        var LowestCriticalValue = <%= jsSerializer.Serialize(ObtainedSensor.LowestCriticalValue)%>;
        var HighestCriticalValue = <%= jsSerializer.Serialize(ObtainedSensor.HighestCriticalValue)%>;
        var SensorReadValueTimes = <%= jsSerializer.Serialize(ChartDateValues)%>;
        var SensorReadValues = <%= jsSerializer.Serialize(ChartDecimalValues)%>;

        var SensorTimelineText = 'Timeline (Hour:Minute:Seconds)';
        var SensorLowestCriticalText = 'Lowest Critical Value';
        var SensorHighestCriticalText = 'Highest Critical Value';

        var DayMonthYear, HoursMinutesSeconds, Day, Month, Year, Hours, Minutes, Seconds, Milliseconds;
        var SplittedWhiteSpace, SplittedDayMonthYear, SplittedHoursMinutesSeconds;

        var DateArray = new Array();
        var Combined = new Array();
        for (var i = 0; i < SensorReadValueTimes.length; i++)
        {
            SplittedWhiteSpace = SensorReadValueTimes[i].split(" ");

            DayMonthYear = SplittedWhiteSpace[0];
            HoursMinutesSeconds = SplittedWhiteSpace[1];

            SplittedDayMonthYear = DayMonthYear.split(".");
            Day = SplittedDayMonthYear[0];
            Month = SplittedDayMonthYear[1];
            Year = SplittedDayMonthYear[2];

            SplittedHoursMinutesSeconds = HoursMinutesSeconds.split(":");
            Hours = SplittedHoursMinutesSeconds[0];
            Minutes = SplittedHoursMinutesSeconds[1];
            Seconds = SplittedHoursMinutesSeconds[2];

            Seconds = Seconds.split(",");
            Milliseconds = Seconds[1];
            Seconds = Seconds[0];
            //Javascript date month indexes starts from 0. So 1 substracted from month to show exact date
            DateArray.push([new Date(parseInt(Year), parseInt(Month - 1), parseInt(Day), parseInt(Hours), parseInt(Minutes), parseInt(Seconds))]);
            //Since javascript date variable erase milliseconds, i keep complete date as milliseconds number(ticks), these values will be created in google datatables that has datetime displays
            Combined[i] = [new Date(DateArray[i]).setMilliseconds(Milliseconds), SensorReadValues[i], LowestCriticalValue, HighestCriticalValue];
        }

        for (var a = 0; a < DateArray.length; a++) //to find minimum and maximum date value, need to cast each value to typeof date again for using math max/min function
        {
            DateArray[a] = new Date(DateArray[a]);
        }

        var minDate = new Date(Math.min.apply(null, DateArray));
        var maxDate = new Date(Math.max.apply(null, DateArray));

        google.charts.load("current", { packages: ["corechart"] });
        google.charts.setOnLoadCallback(drawChart);

        function drawChart()
        {
            var data = new google.visualization.DataTable();
            data.addColumn('datetime', SensorTimelineText);
            data.addColumn('number', SensorUnit);
            data.addColumn('number', SensorLowestCriticalText);
            data.addColumn('number', SensorHighestCriticalText);
            for (var l = 0; l < Combined.length; l++)
            {
                data.addRow([new Date(Combined[l][0]), Combined[l][1], Combined[l][2], Combined[l][3]]);
            }

            var dateFormatter = new google.visualization.DateFormat({ pattern: 'MMM d, yyyy aa hh:mm:ss.SSS' });
            dateFormatter.format(data, 0); 
            var options = {
                title: SensorName,
                hAxis: {
                    title: SensorTimelineText,
                    viewWindow: {
                        min: minDate,
                        max: maxDate
                    },
                    format: 'MMM d, hh:mm:ss'
                },
                vAxis: {
                    title: SensorUnit,
                    viewWindow: {
                        max: GraphicalMaxValue,
                        min: GraphicalMinValue
                    },
                },
                seriesType: 'line',
                series: {
                    1: { lineDashStyle: [6, 6] },
                    2: { lineDashStyle: [6, 6] },
                },
                colors: ['black', 'blue', 'red'],
                pointSize: 3
            };

            var chart = new google.visualization.LineChart(document.getElementById('chart_div'));
            chart.draw(data, options);

        }
        window.onresize = drawChart;
    </script>
    </form>
</body>
</html>
