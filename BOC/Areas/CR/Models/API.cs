using BOC.Models;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;

namespace BOC.Areas.ControlReport.Models
{
    public class API
    {
        public static void UploadSFTPFile(string host, string username, string password, string sourcefile, string destinationpath, int port)
        {
            try
            {
                using (SftpClient client = new SftpClient(host, port, username, password))
                {
                    client.Connect();
                    client.ChangeDirectory(destinationpath);
                    using (FileStream fs = new FileStream(sourcefile, FileMode.Open))
                    {
                        client.BufferSize = 4 * 1024;
                        client.UploadFile(fs, Path.GetFileName(sourcefile));
                    }
                }
            }
            catch (Exception ex)
            {
                //SaveLog.WriteLog(ex.Message);
                throw ex;
               
            }
        }
        public static string FO_ReportType_Get(string t_token)
        {
            //Tạo FO_sFTP_FileName_Get để lấy tên file và upload lên sftp
            Url sftplocation = new Url();
            string url = sftplocation.Get("FO_ReportType_Get");
            HttpClient ClientFTP = new HttpClient();
            var nnvc = new List<KeyValuePair<string, string>>();
            ClientFTP.DefaultRequestHeaders.Add("Authorization", t_token);
            var _req = new HttpRequestMessage(HttpMethod.Post, url) { Content = new FormUrlEncodedContent(nnvc) };

            string _Content;
            HttpResponseMessage _res;
            _res = ClientFTP.SendAsync(_req).Result;
            _Content = _res.Content.ReadAsStringAsync().Result;
            return _Content;
        }
        public static string FO_FlightStage_Get(string t_token)
        {
            //Tạo FO_sFTP_FileName_Get để lấy tên file và upload lên sftp
            Url sftplocation = new Url();
            string url = sftplocation.Get("FO_FlightStage_Get");
            HttpClient ClientFTP = new HttpClient();
            var nnvc = new List<KeyValuePair<string, string>>();
            ClientFTP.DefaultRequestHeaders.Add("Authorization", t_token);
            var _req = new HttpRequestMessage(HttpMethod.Post, url) { Content = new FormUrlEncodedContent(nnvc) };

            string _Content;
            HttpResponseMessage _res;
            _res = ClientFTP.SendAsync(_req).Result;
            _Content = _res.Content.ReadAsStringAsync().Result;
            return _Content;
        }
        
        public static string FO_Division_Get(string t_token)
        {
            //Tạo FO_sFTP_FileName_Get để lấy tên file và upload lên sftp
            Url sftplocation = new Url();
            string url = sftplocation.Get("FO_Division_Get");
            HttpClient ClientFTP = new HttpClient();
            var nnvc = new List<KeyValuePair<string, string>>();
            ClientFTP.DefaultRequestHeaders.Add("Authorization", t_token);
            var _req = new HttpRequestMessage(HttpMethod.Post, url) { Content = new FormUrlEncodedContent(nnvc) };

            string _Content;
            HttpResponseMessage _res;
            _res = ClientFTP.SendAsync(_req).Result;
            _Content = _res.Content.ReadAsStringAsync().Result;
            return _Content;
        }
        public static string FO_sFTP_FileName_Get(string t_filename, string t_token)
        {
            //Tạo FO_sFTP_FileName_Get để lấy tên file và upload lên sftp
            Url sftplocation = new Url();
            string url = sftplocation.Get("FO_sFTP_FileName_Get");
            HttpClient ClientFTP = new HttpClient();
            var nvc = new List<KeyValuePair<string, string>>();
            nvc.Add(new KeyValuePair<string, string>("FileName", t_filename));
            ClientFTP.DefaultRequestHeaders.Add("Authorization", t_token);
            var _req = new HttpRequestMessage(HttpMethod.Post, url) { Content = new FormUrlEncodedContent(nvc) };

            string _Content;
            HttpResponseMessage _res;
            _res = ClientFTP.SendAsync(_req).Result;
            _Content = _res.Content.ReadAsStringAsync().Result;
            return _Content;
        }
        
        public static string Crew_Duty_onFlight_Get(string t_fromDate, string t_toDate, string t_token)
        {
            
            Url sftplocation = new Url();
            string url = sftplocation.Get("Crew_Duty_onFlight_Get");
            HttpClient ClientFTP = new HttpClient();
            var nvc = new List<KeyValuePair<string, string>>();
            nvc.Add(new KeyValuePair<string, string>("FromDate", t_fromDate));
            nvc.Add(new KeyValuePair<string, string>("ToDate", t_toDate));
            ClientFTP.DefaultRequestHeaders.Add("Authorization", t_token);
            var _req = new HttpRequestMessage(HttpMethod.Post, url) { Content = new FormUrlEncodedContent(nvc) };

            string _Content;
            HttpResponseMessage _res;
            _res = ClientFTP.SendAsync(_req).Result;
            _Content = _res.Content.ReadAsStringAsync().Result;
            return _Content;
        }

        public static string FO_Report_Update(int t_FlightID, int t_ReportStatus_ID,string t_ReportDate,string t_Event_Location,string t_Event_Time, string t_Event_Date ,string t_Description,string t_Content,string t_Reccomendation,string t_Status,string lst_ReportType, string lst_Division,string lst_FlightStage,string lst_Attached_Files,int t_ReportID, string t_token)
        {
            lst_Attached_Files = lst_Attached_Files == null ? "" : lst_Attached_Files;
            t_Reccomendation = t_Reccomendation == null ? "" : t_Reccomendation;

            Url misbag = new Url();
            string uri = misbag.Get("FO_Report_Update");
            HttpClient Client = new HttpClient();
            var nvc = new List<KeyValuePair<string, string>>();
            nvc.Add(new KeyValuePair<string, string>("FlightID", t_FlightID.ToString()));
            nvc.Add(new KeyValuePair<string, string>("ReportStatus_ID", t_ReportStatus_ID.ToString()));
            nvc.Add(new KeyValuePair<string, string>("ReportDate", t_ReportDate.ToString()));
            nvc.Add(new KeyValuePair<string, string>("Event_Location", t_Event_Location));
            nvc.Add(new KeyValuePair<string, string>("Event_Time", t_Event_Time));
            nvc.Add(new KeyValuePair<string, string>("Event_Date", t_Event_Date));
            nvc.Add(new KeyValuePair<string, string>("Description", t_Description));
            nvc.Add(new KeyValuePair<string, string>("Content", t_Content));
            nvc.Add(new KeyValuePair<string, string>("Reccomendation", t_Reccomendation));
            nvc.Add(new KeyValuePair<string, string>("Status", t_Status));
            nvc.Add(new KeyValuePair<string, string>("list_ReportType", lst_ReportType));
            nvc.Add(new KeyValuePair<string, string>("list_Division", lst_Division));
            nvc.Add(new KeyValuePair<string, string>("list_FlightStage", lst_FlightStage));
            nvc.Add(new KeyValuePair<string, string>("list_Attached_Files", lst_Attached_Files));
            nvc.Add(new KeyValuePair<string, string>("ReportID", t_ReportID.ToString()));
            Client.DefaultRequestHeaders.Add("Authorization", t_token);
            var req = new HttpRequestMessage(HttpMethod.Post, uri) { Content = new FormUrlEncodedContent(nvc) };
            string Content;
            HttpResponseMessage res;
            res = Client.SendAsync(req).Result;
            Content = res.Content.ReadAsStringAsync().Result;
            return Content;
        }
    }
}
