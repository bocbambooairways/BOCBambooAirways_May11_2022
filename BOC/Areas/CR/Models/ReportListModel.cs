using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BOC.Areas.ControlReport.Models
{
    public class ReportListModel
    {
        public int ResultCode { get; set; }
        public string Message { get; set; }
        public int TotalRecord { get; set; }
        public List<ReportList> Data { get; set; }
    }
    public class ReportList
    {
        public int No { get; set; }
        public DateTime ReportDate { get; set; }
        public string TypeOfReport { get; set; }
        public string IncidientCode { get; set;}
        public DateTime DateOfIncidient { get; set; }
        public string FlightInformation { get; set; }
        public string FlightPhase { get; set; }
        public int File { get; set; }
        public string ReportStattus { get; set; }
        public string ResponseContent { get; set; }
        public string Respondent { get; set; }  
        public DateTime ResponseDate { get; set; }
        public DateTime LastUpdate { get; set; }
        public string UserUpdate { get; set; }  

    }
}
