using System.Collections.Generic;

namespace BOC.Areas.BaggageMiss.Models
{
    public class PirModel
    {
        public int ResultCode { get; set; }
        public string Message { get; set; }
        public int TotalRecord { get; set; }
        public Passenger  Data { get; set; }
      

    }
    public class Passenger
    {
        public string[] PassengerNameList { get; set; }
        public int BagMiss_ID { get; set; }
        public string Airport { get; set; }
        public string ProfileNo { get; set; }
        public string FullName { get; set; }
        public string FltNo { get; set; }
        public string Date { get; set; }
        public string Dep { get; set; }
        public string Arr { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Remark { get; set; }
    }
}
