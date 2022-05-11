using System;

namespace BOC.Areas.B_DCS.Models
{
    public class DCS_AirportList
    {
       

        public string Airport { get; set; }
        public string AirportName { get; set; }
        public string AirportType { get; set; }
        public string CountryName { get; set; }
        public string CityCode { get; set; }
        public string CityName { get; set; }
        public string GMT { get; set; }

    }
    public class DCS_FlightInfor
    {
        public string FlightDate { get; set; }

    }
}
