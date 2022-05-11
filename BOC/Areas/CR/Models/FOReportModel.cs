using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BOC.Areas.ControlReport.Models
{
    public class FOReportModel
    {
        public int ResultCode { get; set; }
        public string Message { get; set; }
        public int TotalRecord { get; set; }
        public List<FOReport> Data { get; set; }
    }
    public class FOReport
    {
        public string Email { get; set; }
        public int FlightID { get; set; }
        public int ReportStatus_ID { get; set; }
        public DateTime ReportDate { get; set; }
        public string Event_Location { get; set; }
        public string Event_Time { get; set; }
        public string Event_Date { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Reccomendation { get; set; }
        public string Status { get; set; }
        public string lst_ReportType_ID { get; set; }
        public string lst_FlightStage_ID { get; set; }
        public string lst_RpDivID { get; set; }
        public string lst_Attached_Files { get; set; }
        public string pathSFTP { get; set; }
        public string FileName { get; set; }
        public int ReportID { get; set; }
        public List<IFormFile> Files { get; set; }
        public List<FileAttach> FileAttached { get; set; }
        

    }
    public class FileAttach
    {
        public int ReportID { get; set; } 
        public int FileLoc_ID { get; set; } 
        public string FileName { get; set; }
        public string sysFileName { get; set; }
        public string Status { get; set; }  //OK,XX,Empty


    }

}
