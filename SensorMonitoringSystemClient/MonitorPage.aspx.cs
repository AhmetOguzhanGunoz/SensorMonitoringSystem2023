using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Web.UI.WebControls;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Office2016.Excel;
using System.Activities.Statements;
using System.Collections;
using DocumentFormat.OpenXml;
using System.Runtime.InteropServices.ComTypes;
using System.IO.Packaging;
using System.Web.UI.WebControls.WebParts;

public partial class MonitorPage : System.Web.UI.Page
{
    string SensorID = "";
    string username = "";
    public static Sensors ObtainedSensor = new Sensors();
    public static List<SensorsDatas> ObtainedSensorDatas = new List<SensorsDatas>();
    public static List<SensorsDatas> GonnaTransformingSensorDatas = new List<SensorsDatas>();
    public List<decimal> ChartDecimalValues = new List<decimal>();
    public List<string> ChartDateValues = new List<string>();
    public JavaScriptSerializer jsSerializer = new JavaScriptSerializer();

    const string wsUrl = "http://localhost:63420/SensorMonitoringSystemService.svc/rest";
    private static WebClient JsonWebClient = new WebClient()
    {
        Encoding = System.Text.Encoding.UTF8,
        Headers = new WebHeaderCollection()
        {
            { HttpRequestHeader.AcceptCharset, "UTF-8" },
            { "Content-Type", "application/json" }
        } //Every binary valued variable object post needs adding header collection again?
    };
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack && DataUpload1.PostedFile != null)
        {
            if (DataUpload1.PostedFile.FileName.Length > 0 && !string.IsNullOrEmpty(DataUpload1.PostedFile.FileName))
            {
                var supportedType = "xlsx";
                var fileExt = System.IO.Path.GetExtension(DataUpload1.FileName).Substring(1);
                if (!supportedType.Contains(fileExt))
                {
                    Infolbl.Text = "File extension is invalid. Please upload only ." + supportedType + " extension type file.";
                }
                else if (DataUpload1.PostedFile.ContentLength > 4 * 1024 * 1024)
                {
                    Infolbl.Text = "File is more than 4MB. Please try to select another file.";
                }
                else
                {
                    GonnaTransformingSensorDatas.Clear();
                    string FilePath = Server.MapPath("~/") + Path.GetFileName(DataUpload1.PostedFile.FileName);
                    DataUpload1.SaveAs(FilePath);

                    SpreadsheetDocument ExcelFile = SpreadsheetDocument.Open(FilePath, false);
                    WorkbookPart workbookPart = ExcelFile.WorkbookPart;
                    WorksheetPart JustOneSheet = workbookPart.WorksheetParts.First(); //OpenXML...doesnt contains shared strings here but contains row information
                    SheetData JustOneSheetsData = JustOneSheet.Worksheet.Elements<SheetData>().First(); // contains all rows information // row = FirstCell(Number(NotSharedString)),SecondCell(Text(SharedString) but no value) text indexing there, values in shared string table part
                    //Number of sharedstring and nonsharedstring must be equal firstly.
                    if (workbookPart.WorksheetParts.Count() > 1) 
                    {
                        Infolbl.Text = "File has more than one sheet. Please try to select another file."; // No allow more sheets in file.
                        goto ReadEnd;
                    }
                    else if(JustOneSheetsData.ChildElements.Count() == 0)
                    {
                        Infolbl.Text = "File sheet is empty. Please try to select another file."; // No allow empty sheets in file.
                        goto ReadEnd;
                    }
                    else if(workbookPart.SharedStringTablePart == null)
                    {
                        Infolbl.Text = "File content is not suitable to load data. Please try to select proper file."; //Non allowed file.
                        goto ReadEnd;
                    }
                    else if(JustOneSheetsData.ChildElements.Count != workbookPart.SharedStringTablePart.SharedStringTable.ChildElements.Count)
                    {
                        Infolbl.Text = "File content is not suitable to load data. Please try to select proper file."; //Non allowed file. Broken Inputs.
                        goto ReadEnd;
                    }
                    else if(workbookPart.SharedStringTablePart.SharedStringTable.ChildElements.Distinct().Count() != workbookPart.SharedStringTablePart.SharedStringTable.ChildElements.Count)
                    {
                        Infolbl.Text = "File has different value for same specific time. Please try to select proper file."; //Non allowed file. Broken Inputs.
                        goto ReadEnd;
                    }
                    else
                    {
                        foreach (Row row in JustOneSheetsData.Elements<Row>())
                        {
                            if (row.ChildElements.Count() != 2)
                            {
                                Infolbl.Text = "File content is not suitable to load data. Please try to select proper file.(Column Count: " + row.ChildElements.Count() + ")(Row Number: " + row.RowIndex.Value + ")";
                                goto ReadEnd;
                            }
                            else
                            {
                                decimal ReadValue = decimal.MinValue;
                                DateTime ReadValueTime = DateTime.MinValue;
                                SensorsDatas newData = new SensorsDatas();
                                foreach (Cell cell in row.Elements<Cell>())
                                {
                                    if (!cell.CellReference.Value.Contains("A") && !cell.CellReference.Value.Contains("B"))
                                    {
                                        Infolbl.Text = "File content is not suitable to load data. Please try to select proper file.(Only A&B Columns allowed)(Cell Index : " + cell.CellReference.Value + ")";
                                        goto ReadEnd;
                                    }
                                    else if (cell.CellReference.Value.Contains("A"))
                                    {
                                        if (!decimal.TryParse(cell.CellValue.Text.Replace(".", ","), out ReadValue) || cell.DataType != null)
                                        {
                                            Infolbl.Text = "File content is not suitable to load data(Decimal{5},{2}) (" + cell.CellReference.Value + " " + cell.CellValue.Text + "). Please try to select proper file.";
                                            goto ReadEnd;
                                        }
                                        else if (ReadValue < (decimal)ObtainedSensor.GraphicalMinValue || ReadValue > (decimal)ObtainedSensor.GraphicalMaxValue)
                                        {

                                            Infolbl.Text = "The value is exceed sensor maximum/minimum value (" + cell.CellReference.Value + " " + cell.CellValue.Text + "). Please try again.";
                                            goto ReadEnd;
                                        }
                                        else
                                        {
                                            ReadValue = Math.Round(ReadValue, 2);
                                            newData.ReadValue = ReadValue;
                                        }
                                    }
                                    else if (cell.CellReference.Value.Contains("B"))
                                    {
                                        if (!DateTime.TryParse(workbookPart.SharedStringTablePart.SharedStringTable.ChildElements[Int32.Parse(cell.CellValue.Text)].InnerText, out ReadValueTime)) //Here, Cell Value holds index of sharedstringtable corresponding value,so call this value index of sharedstringtable
                                        {
                                            Infolbl.Text = "File content is not suitable to load data(DateTime{yyyy-mm-dd hh:mm:ss.000) (" + cell.CellReference.Value + " " + cell.CellValue.Text + "). Please try to select proper file.";
                                            goto ReadEnd;
                                        }
                                        else if (ReadValueTime > DateTime.Now)
                                        {
                                            Infolbl.Text = "Adding date cannot be further than now.";
                                            goto ReadEnd;
                                        }
                                        else
                                        {
                                            newData.ReadValueTime = ReadValueTime;
                                        }
                                    }
                                }
                                newData.SensorID = ObtainedSensor.SensorID;
                                GonnaTransformingSensorDatas.Add(newData);
                            }
                        }
                    }

                    var GonnaAddedTimeFirst = GonnaTransformingSensorDatas.Min(t => t.ReadValueTime);
                    var GonnaAddedTimeLast = GonnaTransformingSensorDatas.Max(t => t.ReadValueTime);

                    var JsonGetSensorDatas = JsonWebClient.DownloadString(wsUrl + "/finddatasbetweendates/" + SensorID + "/" + GonnaAddedTimeFirst.Ticks.ToString() + "/" + GonnaAddedTimeLast.Ticks.ToString());
                    var GetSensorDatas = JsonHelper.Deserialize<List<SensorsDatas>>(JsonGetSensorDatas);

                    if(GetSensorDatas.Count > 0)
                    {
                        Infolbl.Text = "Sensor has data value for between time (" + GonnaAddedTimeFirst.ToString() + ") and (" + GonnaAddedTimeLast.ToString() + "). Please try again.";
                        goto ReadEnd;
                    }
                    else
                    {
                        Infolbl.Text = "Datas are ready to upload.";
                        AddFromExcelbtn.Text = "Load Datas to Sensor";
                    }

                ReadEnd:
                    ExcelFile.Dispose();
                    System.IO.File.Delete(FilePath);
                    DataUpload1 = null;
                }
            }
            else
            {
                Infolbl.Text = "Please select file for adding datas to sensor.(.xlsx allowed, file must contains 1 sheet and must be in text format without data headers.)";
            }
        }


        if (!IsPostBack)
        {
            if (Session["SensorID"] == null)
            {
                Response.Redirect("ProfilePage.aspx");
            }
            else
            {
                SensorID = Session["SensorID"].ToString();
                username = Session["username"].ToString();
                Session.RemoveAll();
            }

            var JsonGetSensor = JsonWebClient.DownloadString(wsUrl + "/findsensor/" + SensorID);
            var GetSensor = JsonHelper.Deserialize<Sensors>(JsonGetSensor);
            ObtainedSensor = GetSensor;

            var JsonSensorDataExistOrNot = JsonWebClient.DownloadString(wsUrl + "/sensordatacontrol/" + SensorID.ToString());
            var SensorDataExistOrNot = JsonHelper.Deserialize<bool>(JsonSensorDataExistOrNot);

            if (!SensorDataExistOrNot)
            {
                Infolbl.Text = "Sensor does not have data for analyzing. You can add data by clicking related button.";
                StartDatetxt.Enabled = false;
                EndDatetxt.Enabled = false;
                Displaybtn.Enabled = false;
                WriteDatatoExcelbtn.Enabled = false;
                DeleteSensorDatabtn.Enabled = false;
            }
            else
            {
                ///Weekly displaying since now if sensor has data, else show data as much as possible until a week complete.
                ObtainedSensorDatas.Clear();
                DateTime DateNow = DateTime.Now;
                DateTime FoundDataDate = DateNow.AddDays(-7);

                var JsonGetSensorDatas = JsonWebClient.DownloadString(wsUrl + "/finddatasbetweendates/" + SensorID + "/" + FoundDataDate.Ticks.ToString() + "/" + DateNow.Ticks.ToString());
                var GetSensorDatas = JsonHelper.Deserialize<List<SensorsDatas>>(JsonGetSensorDatas); 
                
                //Database store 1 year sensor data values since entering last data
                if(GetSensorDatas.Count == 0)
                {
                    for (int monthCounter = 0; monthCounter < 12; monthCounter++)
                    {
                        FoundDataDate = FoundDataDate.AddMonths(-1);
                        var JsonGetSensorDataFinding = JsonWebClient.DownloadString(wsUrl + "/finddatasbetweendates/" + SensorID + "/" + FoundDataDate.Ticks.ToString() + "/" + DateNow.Ticks.ToString());
                        var GetSensorDatasFound = JsonHelper.Deserialize<List<SensorsDatas>>(JsonGetSensorDataFinding);
                        if(GetSensorDatasFound.Count > 0)
                        {
                            foreach (SensorsDatas Data in GetSensorDatasFound)
                            {
                                ObtainedSensorDatas.Add(Data);
                            }
                            DateNow = ObtainedSensorDatas.Max(t => t.ReadValueTime);
                            FoundDataDate = ObtainedSensorDatas.Min(t => t.ReadValueTime);

                            if (!(FoundDataDate > DateNow.AddDays(-7)))
                            {
                                FoundDataDate = DateNow.AddDays(-7);
                            }

                            var TempDatas = (from p in ObtainedSensorDatas where p.ReadValueTime >= FoundDataDate && p.ReadValueTime <= DateNow orderby p.ReadValueTime select p).ToList();

                            ObtainedSensorDatas.Clear();
                            foreach(SensorsDatas data in TempDatas)
                            {
                                ObtainedSensorDatas.Add(data);
                                ChartDateValues.Add(data.ReadValueTime.ToString() + "," + data.ReadValueTime.Millisecond.ToString());
                                ChartDecimalValues.Add(data.ReadValue);
                            }
                            break;
                        }
                        DateNow = FoundDataDate;
                    }
                    ObtainedSensorDatas.OrderBy(t => t.ReadValueTime);
                    Infolbl.Text = "Graph shows sensor datas start from: " + ObtainedSensorDatas.First().ReadValueTime.ToString() + " to end: " + ObtainedSensorDatas.Last().ReadValueTime.ToString();
                }
                else
                {
                    foreach (SensorsDatas Data in GetSensorDatas)
                    {
                        ObtainedSensorDatas.Add(Data);
                        ChartDateValues.Add(Data.ReadValueTime.ToString() + "," + Data.ReadValueTime.Millisecond.ToString());
                        ChartDecimalValues.Add(Data.ReadValue);
                    }
                    ObtainedSensorDatas.OrderBy(t => t.ReadValueTime);
                    Infolbl.Text = "Graph shows sensor datas start from: " + ObtainedSensorDatas.First().ReadValueTime.ToString() + " to end: " + ObtainedSensorDatas.Last().ReadValueTime.ToString();
                }
            }
        }
    }
    protected void AddFromExcelbtn_Click(object sender, EventArgs e)
    {
        if(GonnaTransformingSensorDatas.Count >= 1)
        {
            string PostResult = string.Empty;
            foreach (SensorsDatas Data in GonnaTransformingSensorDatas)
            {
                var NewSensorDataJson = JsonHelper.Serialize(Data);
                JsonWebClient.Headers.Add(HttpRequestHeader.AcceptCharset, "UTF-8");
                JsonWebClient.Headers["Content-type"] = "application/json";
                PostResult = JsonWebClient.UploadString(wsUrl + "/addsensorsdatas", NewSensorDataJson);
                if (!Convert.ToBoolean(int.Parse(PostResult)))
                {
                    Infolbl.Text = "Problem occurred during saving sensor. Please try again. (" + Data.ReadValue + " " + Data.ReadValueTime + ")";
                    break;
                }
                else
                {
                    Infolbl.Text = "Datas uploaded successfully.";
                    ObtainedSensorDatas.Clear();
                    ChartDateValues.Clear();
                    ChartDecimalValues.Clear();
                    DateTime DateNow = DateTime.Now;
                    DateTime FoundDataDate = DateNow.AddDays(-7);

                    var JsonGetSensorDatas = JsonWebClient.DownloadString(wsUrl + "/finddatasbetweendates/" + SensorID + "/" + FoundDataDate.Ticks.ToString() + "/" + DateNow.Ticks.ToString());
                    var GetSensorDatas = JsonHelper.Deserialize<List<SensorsDatas>>(JsonGetSensorDatas);

                    //Database store 1 year sensor data values since entering last data
                    if (GetSensorDatas.Count == 0)
                    {
                        for (int monthCounter = 0; monthCounter < 12; monthCounter++)
                        {
                            FoundDataDate = FoundDataDate.AddMonths(-1);
                            var JsonGetSensorDataFinding = JsonWebClient.DownloadString(wsUrl + "/finddatasbetweendates/" + SensorID + "/" + FoundDataDate.Ticks.ToString() + "/" + DateNow.Ticks.ToString());
                            var GetSensorDatasFound = JsonHelper.Deserialize<List<SensorsDatas>>(JsonGetSensorDataFinding);
                            if (GetSensorDatasFound.Count > 0)
                            {
                                foreach (SensorsDatas DataAfterAddition in GetSensorDatasFound)
                                {
                                    ObtainedSensorDatas.Add(DataAfterAddition);
                                }
                                DateNow = ObtainedSensorDatas.Max(t => t.ReadValueTime);
                                FoundDataDate = ObtainedSensorDatas.Min(t => t.ReadValueTime);

                                if (!(FoundDataDate > DateNow.AddDays(-7)))
                                {
                                    FoundDataDate = DateNow.AddDays(-7);
                                }

                                var TempDatas = (from p in ObtainedSensorDatas where p.ReadValueTime >= FoundDataDate && p.ReadValueTime <= DateNow orderby p.ReadValueTime select p).ToList();

                                ObtainedSensorDatas.Clear();
                                foreach (SensorsDatas DataAfterAddition in TempDatas)
                                {
                                    ObtainedSensorDatas.Add(DataAfterAddition);
                                    ChartDateValues.Add(DataAfterAddition.ReadValueTime.ToString() + "," + DataAfterAddition.ReadValueTime.Millisecond.ToString());
                                    ChartDecimalValues.Add(DataAfterAddition.ReadValue);
                                }
                                break;
                            }
                            DateNow = FoundDataDate;
                        }
                        ObtainedSensorDatas.OrderBy(t => t.ReadValueTime);
                        Infolbl.Text = "Graph shows sensor datas start from: " + ObtainedSensorDatas.First().ReadValueTime.ToString() + " to end: " + ObtainedSensorDatas.Last().ReadValueTime.ToString();
                    }
                }
            }
            GonnaTransformingSensorDatas.Clear();
            DataUpload1 = null;
            AddFromExcelbtn.Text = "Add Data from Excel File";
        }
        else
        {
            Infolbl.Text = "Error occurred during uploading datas. Please try again.";
            GonnaTransformingSensorDatas.Clear();
            DataUpload1 = null;
            AddFromExcelbtn.Text = "Add Data from Excel File";
        }
    }

    protected void WriteDatatoExcelbtn_Click(object sender, EventArgs e)
    {
        if (StartDatetxt.Text == string.Empty || EndDatetxt.Text == string.Empty)
        {
            Infolbl.Text = "Please specify proper date";
        }
        else if (DateTime.Parse(EndDatetxt.Text) > DateTime.Now)
        {
            Infolbl.Text = "End date cannot be further than now.";
        }
        else if(DateTime.Compare(DateTime.Parse(StartDatetxt.Text), DateTime.Parse(EndDatetxt.Text)) > 0)
        {
            Infolbl.Text = "Start date must be earlier than end date.";
        }
        else if(DateTime.Compare(DateTime.Parse(StartDatetxt.Text), DateTime.Parse(EndDatetxt.Text)) == 0)
        {
            Infolbl.Text = "End date must be later than start date.";
        }
        else
        {
            var JsonGetSensorDatas = JsonWebClient.DownloadString(wsUrl + "/finddatasbetweendates/" + SensorID + "/" + DateTime.Parse(StartDatetxt.Text).Ticks.ToString() + "/" + DateTime.Parse(EndDatetxt.Text).Ticks.ToString());
            var GetSensorDatas = JsonHelper.Deserialize<List<SensorsDatas>>(JsonGetSensorDatas);

            if (GetSensorDatas.Count == 0)
            {
                Infolbl.Text = "Sensor has no data value for between time (" + DateTime.Parse(StartDatetxt.Text).ToString() + ") and (" + DateTime.Parse(EndDatetxt.Text).ToString() + "). Please try again.";
            }
            else
            {
                GonnaTransformingSensorDatas.Clear();
                foreach(SensorsDatas data in GetSensorDatas)
                {
                    GonnaTransformingSensorDatas.Add(data);
                }

                GonnaTransformingSensorDatas.OrderBy(t => t.ReadValueTime);
                DataTable TempDt = new DataTable();
                TempDt.Columns.Add("SensorDescription", typeof(string));
                TempDt.Columns.Add("ReadValue", typeof(string));
                TempDt.Columns.Add("ReadValueTime", typeof(string));
                foreach (SensorsDatas data in GonnaTransformingSensorDatas)
                {
                    DataRow newRow = TempDt.NewRow();
                    newRow["SensorDescription"] = ObtainedSensor.SensorDescription;
                    newRow["ReadValue"] = data.ReadValue.ToString();
                    newRow["ReadValueTime"] = data.ReadValueTime.ToString() + "." + data.ReadValueTime.Millisecond.ToString();
                    TempDt.Rows.Add(newRow);
                }

                string FileName = ObtainedSensor.SensorDescription.ToString().Replace(" ", "_").Trim(new Char[] { ':', '/', '.', '\\' }) + "_" + GonnaTransformingSensorDatas.Min(t => t.ReadValueTime).ToString().Replace(" ", "_").Replace(":", "-").Replace(".", "-") + "_" + GonnaTransformingSensorDatas.Max(t => t.ReadValueTime).ToString().Replace(" ", "_").Replace(":", "-").Replace(".", "-") + ".xlsx";
                string FilePath = Server.MapPath("~/") + FileName;

                SpreadsheetDocument ExcelFile = SpreadsheetDocument.Create(FilePath, SpreadsheetDocumentType.Workbook);

                WorkbookPart workbookpart = ExcelFile.AddWorkbookPart();
                workbookpart.Workbook = new Workbook();

                WorksheetPart worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
                var sheetData = new SheetData();
                worksheetPart.Worksheet = new Worksheet(sheetData);

                Sheets sheets = ExcelFile.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());
                Sheet sheet = new Sheet() { Id = ExcelFile.WorkbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Sayfa1" };
                sheets.Append(sheet);

                Row headerRow = new Row();
                List<string> columns = new List<string>();

                foreach (DataColumn column in TempDt.Columns)
                {
                    columns.Add(column.ColumnName);

                    Cell cell = new Cell();
                    cell.DataType = CellValues.String;
                    cell.CellValue = new CellValue(column.ColumnName);
                    headerRow.AppendChild(cell);
                }

                sheetData.AppendChild(headerRow);

                foreach (DataRow dsrow in TempDt.Rows)
                {
                    Row newRow = new Row();
                    foreach (string col in columns)
                    {
                        Cell cell = new Cell();
                        cell.DataType = CellValues.String;
                        cell.CellValue = new CellValue(dsrow[col].ToString()); 
                        newRow.AppendChild(cell);
                    }
                    sheetData.AppendChild(newRow);
                }

                workbookpart.Workbook.Save();
                ExcelFile.Dispose();
                Response.AddHeader("Content-Disposition", "inline;filename="+FileName);
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.WriteFile(FileName);
                Response.Flush();
                System.IO.File.Delete(FilePath);
                Response.End();
            }
        }
    }

    protected void DeleteSensorDatabtn_Click(object sender, EventArgs e)
    {
        if (StartDatetxt.Text == string.Empty || EndDatetxt.Text == string.Empty)
        {
            Infolbl.Text = "Please specify proper date";
        }
        else if (DateTime.Compare(DateTime.Parse(StartDatetxt.Text), DateTime.Parse(EndDatetxt.Text)) > 0)
        {
            Infolbl.Text = "Start date must be earlier than end date.";
        }
        else if (DateTime.Compare(DateTime.Parse(StartDatetxt.Text), DateTime.Parse(EndDatetxt.Text)) == 0)
        {
            Infolbl.Text = "End date must be later than start date.";
        }
        else if (DateTime.Parse(EndDatetxt.Text) > DateTime.Now)
        {
            Infolbl.Text = "End date cannot be further than now.";
        }
        else
        {
            var JsonGetSensorDatas = JsonWebClient.DownloadString(wsUrl + "/finddatasbetweendates/" + SensorID + "/" + DateTime.Parse(StartDatetxt.Text).Ticks.ToString() + "/" + DateTime.Parse(EndDatetxt.Text).Ticks.ToString());
            var GetSensorDatas = JsonHelper.Deserialize<List<SensorsDatas>>(JsonGetSensorDatas);

            if (GetSensorDatas.Count == 0)
            {
                Infolbl.Text = "Sensor has no data value for between time (" + DateTime.Parse(StartDatetxt.Text).ToString() + ") and (" + DateTime.Parse(EndDatetxt.Text).ToString() + "). Please try again.";
            }
            else
            {
                string PostResult = JsonWebClient.DownloadString(wsUrl + "/deletedatasbetweendates/" + SensorID + "/" + DateTime.Parse(StartDatetxt.Text).Ticks.ToString() + "/" + DateTime.Parse(EndDatetxt.Text).Ticks.ToString());
                if (!Convert.ToBoolean(int.Parse(PostResult)))
                {
                    Infolbl.Text = "Error occurred during deleting sensor datas. Please try again.";
                }
                else
                {
                    Infolbl.Text = "Specified sensor datas has been deleted.";
                    ObtainedSensorDatas.Clear();
                    ChartDateValues.Clear();
                    ChartDecimalValues.Clear();
                    DateTime DateNow = DateTime.Now;
                    DateTime FoundDataDate = DateNow.AddDays(-7);

                    var JsonGetSensorDatasAfterDelete = JsonWebClient.DownloadString(wsUrl + "/finddatasbetweendates/" + SensorID + "/" + FoundDataDate.Ticks.ToString() + "/" + DateNow.Ticks.ToString());
                    var GetSensorDatasAfterDelete = JsonHelper.Deserialize<List<SensorsDatas>>(JsonGetSensorDatasAfterDelete);

                    //Database store 1 year sensor data values since entering last data
                    if (GetSensorDatasAfterDelete.Count == 0)
                    {
                        for (int monthCounter = 0; monthCounter < 12; monthCounter++)
                        {
                            FoundDataDate = FoundDataDate.AddMonths(-1);
                            var JsonGetSensorDataFinding = JsonWebClient.DownloadString(wsUrl + "/finddatasbetweendates/" + SensorID + "/" + FoundDataDate.Ticks.ToString() + "/" + DateNow.Ticks.ToString());
                            var GetSensorDatasFound = JsonHelper.Deserialize<List<SensorsDatas>>(JsonGetSensorDataFinding);
                            if (GetSensorDatasFound.Count > 0)
                            {
                                foreach (SensorsDatas DataAfterDelete in GetSensorDatasFound)
                                {
                                    ObtainedSensorDatas.Add(DataAfterDelete);
                                }
                                DateNow = ObtainedSensorDatas.Max(t => t.ReadValueTime);
                                FoundDataDate = ObtainedSensorDatas.Min(t => t.ReadValueTime);

                                if (!(FoundDataDate > DateNow.AddDays(-7)))
                                {
                                    FoundDataDate = DateNow.AddDays(-7);
                                }

                                var TempDatas = (from p in ObtainedSensorDatas where p.ReadValueTime >= FoundDataDate && p.ReadValueTime <= DateNow orderby p.ReadValueTime select p).ToList();

                                ObtainedSensorDatas.Clear();
                                foreach (SensorsDatas DataAfterDelete in TempDatas)
                                {
                                    ObtainedSensorDatas.Add(DataAfterDelete);
                                    ChartDateValues.Add(DataAfterDelete.ReadValueTime.ToString() + "," + DataAfterDelete.ReadValueTime.Millisecond.ToString());
                                    ChartDecimalValues.Add(DataAfterDelete.ReadValue);
                                }
                                break;
                            }
                            DateNow = FoundDataDate;
                        }
                        ObtainedSensorDatas.OrderBy(t => t.ReadValueTime);
                        Infolbl.Text = "Graph shows sensor datas start from: " + ObtainedSensorDatas.First().ReadValueTime.ToString() + " to end: " + ObtainedSensorDatas.Last().ReadValueTime.ToString();
                    }
                }
            }
        }
    }

    protected void ReturnProfilebtn_Click(object sender, EventArgs e)
    {
        Session.RemoveAll();
        Session["username"] = username;
        Response.Redirect("ProfilePage.aspx");
    }

    protected void Displaybtn_Click(object sender, EventArgs e)
    {
        if (StartDatetxt.Text == string.Empty || EndDatetxt.Text == string.Empty)
        {
            Infolbl.Text = "Please specify proper date";
        }
        else if (DateTime.Compare(DateTime.Parse(StartDatetxt.Text), DateTime.Parse(EndDatetxt.Text)) > 0)
        {
            Infolbl.Text = "Start date must be earlier than end date.";
        }
        else if (DateTime.Compare(DateTime.Parse(StartDatetxt.Text), DateTime.Parse(EndDatetxt.Text)) == 0)
        {
            Infolbl.Text = "End date must be later than start date.";
        }
        else if(DateTime.Parse(EndDatetxt.Text) > DateTime.Now)
        {
            Infolbl.Text = "End date cannot be further than now.";
        }
        else
        {
            var JsonGetSensorDatas = JsonWebClient.DownloadString(wsUrl + "/finddatasbetweendates/" + SensorID + "/" + DateTime.Parse(StartDatetxt.Text).Ticks.ToString() + "/" + DateTime.Parse(EndDatetxt.Text).Ticks.ToString());
            var GetSensorDatas = JsonHelper.Deserialize<List<SensorsDatas>>(JsonGetSensorDatas);

            if (GetSensorDatas.Count == 0)
            {
                Infolbl.Text = "Sensor has no data value for between time (" + DateTime.Parse(StartDatetxt.Text).ToString() + ") and (" + DateTime.Parse(EndDatetxt.Text).ToString() + "). Please try again.";
            }
            else
            {
                ObtainedSensorDatas.Clear();
                foreach (SensorsDatas Data in GetSensorDatas)
                {
                    ObtainedSensorDatas.Add(Data);
                    ChartDateValues.Add(Data.ReadValueTime.ToString() + "," + Data.ReadValueTime.Millisecond.ToString());
                    ChartDecimalValues.Add(Data.ReadValue);
                }
                ObtainedSensorDatas.OrderBy(t => t.ReadValueTime);
                Infolbl.Text = "Graph shows sensor datas start from: " + ObtainedSensorDatas.First().ReadValueTime.ToString() + " to end: " + ObtainedSensorDatas.Last().ReadValueTime.ToString();
            }
        }
    }
}