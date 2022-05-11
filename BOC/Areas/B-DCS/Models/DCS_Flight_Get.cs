namespace BOC.Areas.B_DCS.Models
{
    public class DCS_Flight_Get
    {
        public string FlightID { get; set; }
        public string FlightDate { get; set; }
        public string FltNo { get; set; }
        public string Dep { get; set; }
        public string Arr { get; set; }
        public string Gate { get; set; }
        public string Parking { get; set; }

        public string STD { get; set; }
        public string ETD { get; set; }
        public string BDT { get; set; }
        public string STA { get; set; }
        public string ETA { get; set; }

        public string CrewInfo { get; set; }
        public string CrewBag { get; set; }
        public string Co_Mat { get; set; }


        public string PassengerInfo { get; set; }
        public string PassengerBag { get; set; }

        public string Aircraft { get; set; }
        public string RegisterNo { get; set; }
        public string Status { get; set; }
        public string RecDate { get; set; }

        //public string margin_top { get; set; }


    }
}
