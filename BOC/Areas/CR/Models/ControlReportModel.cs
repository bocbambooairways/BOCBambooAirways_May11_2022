using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BOC.Areas.ControlReport.Models
{
    public class ControlReportModel
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string ErrorMessage { get; set; }
        public List<FlightInfo> DataInfo { get; set; }
    }
    public class ControlFlight
    {
        public int ResultCode { get; set; }
        public string Message { get; set; }
        public int TotalRecord { get; set; }
        public List<FlightInfo> Data { get; set; }
    }
    public class FlightInfo
    {
        public int No { get; set; }
        public int GDID { get; set; }
        public int BAV_ID { get; set; }
        public int FlightID { get; set; }
        public string CrewName { get; set; }
        public string Pos { get; set; }
        public string FltNo { get; set; }
        public string RegisterNo { get; set; }
        public string Dep { get; set; }
        public string Arr { get; set; }
        public string Aircraft { get; set; }
        public string FlightType { get; set; }
        public string FlightType_Description { get; set; }
        //public string FlightDate { get; set; }
        public DateTime FlightDate { get; set; }
        public string STD { get; set; }
        public string ETD { get; set; }
        public string ATD { get; set; }
        public string STA { get; set; }
        public string ETA { get; set; }
        public string ATA { get; set; }

    }
    public class ReportType
    {
        public int ResultCode { get; set; }
        public string Message { get; set; }
        public int TotalRecord { get; set; }
        public List<RT> Data { get; set; }
    }
    public class RT
    {
        public int ReportType_ID { get; set; }
        public string ReportType_Code { get; set; }
        public string Description { get; set; }
    }
    public class FlightPhase
    {
        public int ResultCode { get; set; }
        public string Message { get; set; }
        public int TotalRecord { get; set; }
        public List<FS> Data { get; set; }
    }
    public class FS
    {
        public int FlightStage_ID { get; set; } 
        public string FlightStage_Code { get; set; }
        public string Description { get; set; }
    }
    public class Division
    {
        public int ResultCode { get; set; }
        public string Message { get; set; }
        public int TotalRecord { get; set; }
        public List<DV> Data { get; set; }
    }
    public class DV
    {
        public int RpDivID { get; set; }
        public string DivisionCode { get; set; }
        public string DivisionName { get; set; }
    }
}
