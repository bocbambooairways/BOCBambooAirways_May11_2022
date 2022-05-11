using Microsoft.AspNetCore.Mvc;
using BOC.Models;
using BOC.Areas.ControlReport.Models;
using System;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Nancy.Json;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using System.IO;
using System.Threading.Tasks;
using Renci.SshNet;

namespace BOC.Areas.ControlReport.Controllers
{
    [Area("CR")]
    public class ControlReportMobileController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration configuration;
        private readonly IFileProvider fileProvider;
        public ControlReportMobileController(IWebHostEnvironment webHostEnvironment, IConfiguration configuration, IFileProvider fileProvider)
        {
            _webHostEnvironment = webHostEnvironment;
            this.fileProvider = fileProvider;
            this.configuration = configuration;


        }
        public IActionResult Index()
        {
            return View();
        }
        public List<RT> GetReportType()
        {

            var token = HttpContext.Session.GetString("Token");
            string Content = API.FO_ReportType_Get(token);
            var oData = JObject.Parse(Content);
            //Bind Json To List 
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<RT> lst = ser.Deserialize<List<RT>>(oData["Data"].ToString());//str is JSON string.
            return lst;
        }
        public List<FS> GetFlightStage()
        {

            var token = HttpContext.Session.GetString("Token");
            string Content = API.FO_FlightStage_Get(token);
            var oData = JObject.Parse(Content);
            //Bind Json To List 
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<FS> lst = ser.Deserialize<List<FS>>(oData["Data"].ToString());//str is JSON string.
            return lst;
        }

        public List<DV> GetDivision()
        {
            var token = HttpContext.Session.GetString("Token");
            string Content = API.FO_Division_Get(token);
            var oData = JObject.Parse(Content);
            //Bind Json To List 
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<DV> lst = ser.Deserialize<List<DV>>(oData["Data"].ToString());//str is JSON string.
            return lst;
        }
        public IActionResult CreateReport(string t_obj)
        {
            string[] obj = t_obj.Split(',');
            
            ViewBag.FlightDate = obj[0];
            ViewBag.FlightNo = obj[1];
            ViewBag.Route = obj[2];
            ViewBag.FlightID = obj[3];
            //Save Session FlightID
            HttpContext.Session.SetString("FlightID", obj[3]);
            ViewBag.Reg = obj[4];
            ViewBag.Aircraft = obj[5];
            ViewBag.ReportType = GetReportType();
            ViewBag.FlightStage = GetFlightStage();
            ViewBag.Division = GetDivision();
            return View();
        }
        [HttpPost]
        public IActionResult CreateReport(FOReport model)
        {
            string strMess = string.Empty;
            var eventdate = DateTime.ParseExact(model.Event_Date, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
            string report_type = model.lst_ReportType_ID;
            string[] typereport = report_type.Split(',');
            // Create list
            var myList = new List<string>();
            foreach (string type in typereport)
            {


                string rp = "{" + '"' + "ReportType_ID" + '"' + ":" + type + "}";
                // Add items to the list
                myList.Add(rp);
            }
            //Tạo chuỗi json Report Type
            var arr = myList.ToArray();
            var json_report_type = JsonConvert.SerializeObject(arr);
            json_report_type = json_report_type.Replace(@"\", "").Replace(@"""{", "{").Replace(@"}""", "}");
            string division = model.lst_RpDivID;
            string[] dv = division.Split(',');
            // Create list
            var lstdiv = new List<string>();
            foreach (string d in dv)
            {
                string div = "{" + '"' + "RpDivID" + '"' + ":" + d + "}";
                // Add items to the list
                lstdiv.Add(div);
            }
            //Tạo chuỗi json Division
            var dvs = lstdiv.ToArray();
            var json_division = JsonConvert.SerializeObject(dvs);
            json_division = json_division.Replace(@"\", "").Replace(@"""{", "{").Replace(@"}""", "}");


            string flightphase = model.lst_FlightStage_ID;
            string[] fp = flightphase.Split(',');
            // Create list
            var lstfp = new List<string>();
            foreach (string f in fp)
            {
                string fs = "{" + '"' + "FlightStage_ID" + '"' + ":" + f + "}";
                // Add items to the list
                lstfp.Add(fs);
            }
            //Tạo chuỗi json Flight Phase
            var fps = lstfp.ToArray();
            var json_flightphase = JsonConvert.SerializeObject(fps);
            json_flightphase = json_flightphase.Replace(@"\", "").Replace(@"""{", "{").Replace(@"}""", "}");

            //Get Session Token
            var token = HttpContext.Session.GetString("Token");
            //Get Session FlightID
            model.FlightID = Int32.Parse(HttpContext.Session.GetString("FlightID"));
            //Get Session Object Attach File
            var _attachfileList = SessionHelper.GetObjectFromJson<List<FileAttach>>(HttpContext.Session, "FileAttach");
            // Create list
            var lstfa = new List<string>();
            foreach (var _attachfile in _attachfileList)
            {
                if (_attachfile.Status == "OK")
                {
                    string fileattached = "{" + '"' + "FileLoc_ID" + '"' + ":" + _attachfile.FileLoc_ID + "}";
                    // Add items to the list
                    lstfa.Add(fileattached);
                }
            }
            //Tạo chuỗi json Attached File Upload
            var fas = lstfa.ToArray();
            var json_fa = JsonConvert.SerializeObject(fas);
            json_fa = json_fa.Replace(@"\", "").Replace(@"""{", "{").Replace(@"}""", "}");

            //Call api to update Report
            string Content = API.FO_Report_Update(model.FlightID, 1, model.ReportDate.ToString("yyyy-MM-dd"), model.Event_Location, model.Event_Time,
                eventdate, model.Description, model.Content, model.Reccomendation, "OK", json_report_type, json_division, json_flightphase, json_fa, 0, token);
            var oData = JObject.Parse(Content);
            if (oData["ResultCode"].ToString() == "0")
            {
                return Json(new { mess = "OK" });

            }
            else
            {
                strMess = oData["Message"].ToString();
            }
            return Json(new { mess = strMess });
        }

        [HttpPost]
        public async Task<IActionResult> UploadFileAttach(FOReport model)
        {
            var host = configuration["SFTP:Host"];
            var port = configuration["SFTP:Port"];
            var username = configuration["SFTP:Username"];
            var password = configuration["SFTP:Password"];
            string strMess = string.Empty;
            var token = HttpContext.Session.GetString("Token");


            model.FileAttached = new List<FileAttach>();
            foreach (var file in model.Files)
            {
                //Call api to get BagHS_Found
                string Content = API.FO_sFTP_FileName_Get(file.FileName.ToString(), token);
                var oData = JObject.Parse(Content);
                var FileLoc_ID = oData["Data"]["ID"].ToString();
                model.pathSFTP = oData["Data"]["Folder"].ToString();
                model.FileName = oData["Data"]["FileName"].ToString();
                model.lst_Attached_Files += FileLoc_ID + ",";
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/data/ControlReport/", model.FileName);
                //Upload file len web server
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    try
                    {
                        await file.CopyToAsync(stream);
                    }
                    catch (Exception ex)
                    {
                        strMess = ex.Message;
                        return Json(new { mess = strMess });
                    }
                }
                //Upload File To SFTP
                using (var client = new SftpClient(host, Int16.Parse(port), username, password))
                {
                    client.Connect();
                    if (client.IsConnected)
                    {
                        try
                        {
                            //Call Api Upload SFTP File
                            API.UploadSFTPFile(host, username, password, path, model.pathSFTP, Int32.Parse(port));

                        }
                        catch (Exception ex)
                        {
                            strMess = ex.Message;
                            return Json(new { mess = strMess });
                        }



                    }
                }

                FileAttach fd = new FileAttach();
                fd.ReportID = 0;
                fd.FileLoc_ID = Int32.Parse(FileLoc_ID.ToString());
                fd.FileName = file.FileName.ToString();
                fd.sysFileName = model.FileName;
                fd.Status = "OK";
                model.FileAttached.Add(fd);


            }
            //SAVE Session object AttachedFile
            SessionHelper.SetObjectAsJson(HttpContext.Session, "FileAttach", model.FileAttached);
            var lst_attached = Newtonsoft.Json.JsonConvert.SerializeObject(model.FileAttached);

            return Json(new { obj = lst_attached, mess = "OK" });

        }
        public IActionResult FlightGet()
        {
            return View();
        }
        [HttpPost]
        public IActionResult FlightGet(ControlReportModel model)
        {
            DateTime fd = DateTime.ParseExact(model.FromDate, "dd/MM/yyyy", null);

            DateTime td = DateTime.ParseExact(model.ToDate, "dd/MM/yyyy", null);
            if (fd <= td)
            {

                String fromDate = fd.ToString("yyyy-MM-dd");
                String toDate = fd.ToString("yyyy-MM-dd");
                var token = HttpContext.Session.GetString("Token");

                string Content = API.Crew_Duty_onFlight_Get(fromDate, toDate, token);

                JToken _FltItem;
                JObject ser = JObject.Parse(Content);
                Int32 _Result = (int)ser.SelectToken("ResultCode");

                List<FlightInfo> lst = new List<FlightInfo>();
                var ser2 = ser.SelectToken("Data");
                List<JToken> data = new List<JToken>(ser2.Children());

                foreach (var item in data)
                {
                    FlightInfo Flt = new FlightInfo();

                    item.CreateReader();

                    Int32 _GDID = Int32.Parse(item.SelectToken("GDID").ToString());
                    Flt.GDID = _GDID;

                    Int32 _BAV_ID = Int32.Parse(item.SelectToken("BAV_ID").ToString());
                    Flt.GDID = _BAV_ID;

                    Int32 _FlightID = Int32.Parse(item.SelectToken("FlightID").ToString());
                    Flt.FlightID = _FlightID;


                    _FltItem = item.SelectToken("CrewName");
                    Flt.CrewName = (string)_FltItem;


                    _FltItem = item.SelectToken("Pos");
                    Flt.Pos = (string)_FltItem;


                    _FltItem = item.SelectToken("FltNo");
                    Flt.FltNo = (string)_FltItem;

                    _FltItem = item.SelectToken("RegisterNo");
                    Flt.RegisterNo = (string)_FltItem;

                    _FltItem = item.SelectToken("Dep");
                    Flt.Dep = (string)_FltItem;

                    _FltItem = item.SelectToken("Arr");
                    Flt.Arr = (string)_FltItem;

                    _FltItem = item.SelectToken("Aircraft");
                    Flt.Aircraft = (string)_FltItem;

                    _FltItem = item.SelectToken("FlightType");
                    Flt.FlightType = (string)_FltItem;

                    _FltItem = item.SelectToken("FlightType_Description");
                    Flt.FlightType_Description = (string)_FltItem;

                    _FltItem = item.SelectToken("FlightDate");
                    DateTime dt = DateTime.Parse(_FltItem.ToString());
                    Flt.FlightDate = dt;


                    _FltItem = item.SelectToken("STD");
                    Flt.STD = (string)_FltItem;

                    _FltItem = item.SelectToken("ETD");
                    Flt.ETD = (string)_FltItem;

                    _FltItem = item.SelectToken("ATD");
                    Flt.ATD = (string)_FltItem;

                    _FltItem = item.SelectToken("STA");
                    Flt.STA = (string)_FltItem;

                    _FltItem = item.SelectToken("ETA");
                    Flt.ETA = (string)_FltItem;

                    _FltItem = item.SelectToken("ATA");
                    Flt.ATA = (string)_FltItem;

                    lst.Add(Flt);
                }
                for (int i = 0; i < lst.Count; i++)
                {
                    lst[i].No = i + 1;
                }
                model.DataInfo = lst;
                //SAVE Session object List<FlightInfo>
                SessionHelper.SetObjectAsJson(HttpContext.Session, "DataInfo", model.DataInfo);


                return Json(new { mess = "OK" });
            }
            else
            {
                model.ErrorMessage = "Ngày đi phải bằng hoặc nhỏ hơn Ngày đến!/FromDate must be equal or smaller than ToDate!";
                return Json(new { mess = model.ErrorMessage });
            }

            //return View(model);
           
        }
        [HttpPost]
        public IActionResult RemoveAttachFile(string t_fname)
        {
            string strMess = string.Empty;
            string contentRootPath = _webHostEnvironment.ContentRootPath;
            var filePath = Path.Combine(contentRootPath, "wwwroot/data/ControlReport/" + t_fname);



            System.IO.DirectoryInfo di = new DirectoryInfo(filePath);

            // Check if file exists with its full path    
            if (System.IO.File.Exists(filePath))
            {
                // If file found, delete it    
                System.IO.File.Delete(filePath);
                strMess = "OK";
                //Get Session Token
                string token = HttpContext.Session.GetString("Token");

                //Goi lai Session Object List Attached
                List<FileAttach> _attchfileList = SessionHelper.GetObjectFromJson<List<FileAttach>>(HttpContext.Session, "FileAttach");
                foreach (var item in _attchfileList)
                {
                    if (item.sysFileName == t_fname)
                    {
                        //string Content = API.FO_AttachedFile_Update(Int32.Parse(ReportID.ToString()), item.FileLoc_ID.ToString(), "XX", token);
                        item.Status = "XX";
                    }
                }
                //SAVE Session object AttachedFile
                SessionHelper.SetObjectAsJson(HttpContext.Session, "FileAttach", _attchfileList);
                var data = Newtonsoft.Json.JsonConvert.SerializeObject(_attchfileList);

                return Json(new { mess = strMess, obj = data });


            }
            else
            {
                strMess = "Delete attach file fail!";
            }
            return Json(new { mess = strMess });
        }

        public IActionResult FlightList(ControlReportModel model)
        {
            //Get Session Object List<FlightInfo>
            model.DataInfo = SessionHelper.GetObjectFromJson<List<FlightInfo>>(HttpContext.Session, "DataInfo");
            return View(model);
        }
        public IActionResult ReportList()
        {
            return View();
        }
    }
}
