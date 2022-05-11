using BOC.Areas.B_DCS.Models;
using BOC.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;

namespace BOC.Areas.B_DCS.Controllers
{
    [Area("B-DCS")]
    public class FlightController : Controller
    {
        private IHostingEnvironment Environment;
        public UriConfig UriConfig { get; }
        public FlightController(Microsoft.Extensions.Options.IOptions<UriConfig> _UriConfig, IHostingEnvironment Environment)
        {
            UriConfig = _UriConfig.Value;
            this.Environment = Environment;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult M_Index(string Airport, 
                                     string FlightDate)
        {
            if (!string.IsNullOrEmpty(Airport)||
                !string.IsNullOrEmpty(FlightDate))
            {
                ViewBag.Airport = new DataAPI().ReturnValueFromString(Airport, "_");
                ViewBag.FlightDate = new DataAPI().ReturnValueFromString(FlightDate, "_");
            }
          
            return View();  
        }
        public IActionResult M_FlightList(string FlightDate)
        {
            ViewBag.FlightDate =new DataAPI().ReturnValueFromString(FlightDate,"_");    
            string uri_Get_B_DCS_AirportList = UriConfig.uri_B_DCS_AirportList;
            IEnumerable<DCS_AirportList> Get_B_DCS_AirportList = new DataAPI().GetListAPIWithoutParams<DCS_AirportList>(UriConfig.uri_B_DCS_AirportList,
                                                                      HttpContext.Session.GetString("Token"), System.Net.Http.HttpMethod.Post, false, "Data").Result;
            return View(Get_B_DCS_AirportList);
        }
        public IActionResult M_SearchList()
        {
            return View();
        }
        [HttpPost]  
        public IActionResult M_SearchList(string Airports,
                                          string FlightDate,
                                          string currentPage,
                                          string next,
                                          string previous)
        {
           
            string uri_DCS_Flight_Get = UriConfig.uri_B_DCS_SearchList;
            List<DCS_Flight_Get> DCS_Get_Search_List = new DataAPI().GetListOjectAPI<DCS_Flight_Get>(uri_DCS_Flight_Get,
                                                          HttpContext.Session.GetString("Token"),
                                                          HttpMethod.Post, false, "Data",
                                                          "FlightDate",
                                                           (FlightDate!=null? FlightDate:string.Empty),
                                                           "Airports",
                                                           (Airports!=null?Airports:string.Empty)).Result;
                var rs_Searchlist_Pagination_DCS = new DataAPI().list_Pagination_DCS<DCS_Flight_Get>(currentPage,
                                                            next,
                                                            previous,
                                                            10,
                                                            DCS_Get_Search_List, 
                                                            out int ViewBagcurrentpage,
                                                            out int ViewBagnumSize);
            ViewBag.currentpage = ViewBagcurrentpage;
            ViewBag.numSize = ViewBagnumSize;   
            return View(rs_Searchlist_Pagination_DCS.OrderBy(m => m.FlightID).ToList());
        }
        public IActionResult M_Passenger(string FlightID,string FlightInfor)
        {
            string _FlightID = new DataAPI().ReturnValueFromString(FlightID, " ");
            string _FlightInfor = new DataAPI().ReturnValueFromString(FlightInfor, "_");
            HttpContext.Session.SetString("FlightID",_FlightID);
            HttpContext.Session.SetString("FlightInfor", _FlightInfor);
            return View();
        }
        public IActionResult M_PassengerList()
        {
            return View();
        }

        [HttpPost]
        public IActionResult M_PassengerList(string ifly_Pax_ID,
                                              string PNR,
                                              string KeySearch,
                                              string PassengerName, 
                                              string currentPage,
                                              string next,
                                              string previous)
        {
            
            string uri_DCS_Passenger_Get = UriConfig.uri_B_DCS_Passenger_Get;
            List<DCS_Passenger_Get> DCS_Get_Passenger_List = new DataAPI().GetListOjectAPI<DCS_Passenger_Get>(uri_DCS_Passenger_Get,
                                                          HttpContext.Session.GetString("Token"),
                                                          HttpMethod.Post, false, "Data",
                                                          "FlightID",
                                                          (HttpContext.Session.GetString("FlightID") != null ? 
                                                           HttpContext.Session.GetString("FlightID").ToString() : "124853"),
                                                          "ifly_Pax_ID",
                                                           (ifly_Pax_ID != null ? ifly_Pax_ID : "0"),
                                                           "PNR",
                                                           (PNR != null ? PNR : string.Empty),
                                                           "KeySearch",
                                                           (KeySearch!=null? KeySearch:string.Empty),
                                                           "PassengerName",
                                                           (PassengerName != null ? PassengerName : string.Empty)).Result;
            var rs_list_Passenger_Pagination_DCS = new DataAPI().list_Pagination_DCS<DCS_Passenger_Get>(currentPage,
                                                           next,
                                                           previous,
                                                           10,
                                                           DCS_Get_Passenger_List,
                                                           out int ViewBagcurrentpage,
                                                           out int ViewBagnumSize);
            ViewBag.currentpage = ViewBagcurrentpage;
            ViewBag.numSize = ViewBagnumSize;
            return View(rs_list_Passenger_Pagination_DCS.OrderBy(m => m.FlightID).ToList());
           
        }
        public IActionResult M_Comment(string ifly_Pax_ID,
                                       string Pax_name,
                                       string SEQ_Seat_ST,
                                       string PNR_Ticket,
                                       string Class_SSR,
                                       string Remark)
        {
            ViewBag.ifly_Pax_ID = new DataAPI().ReturnValueFromString(ifly_Pax_ID, "_");
            ViewBag.Pax_name = new DataAPI().ReturnValueFromString(Pax_name, "_");
            ViewBag.SEQ_Seat_ST = new DataAPI().ReturnValueFromString(SEQ_Seat_ST, "_");
            ViewBag.PNR_Ticket = new DataAPI().ReturnValueFromString(PNR_Ticket, "_");
            ViewBag.Class_SSR = new DataAPI().ReturnValueFromString(Class_SSR, "_");
            ViewBag.Remark = new DataAPI().ReturnValueFromString(Remark, "_");
            HttpContext.Session.SetString("ifly_Pax_ID", ifly_Pax_ID);
            return View();
        }
        [HttpPost]
        public IActionResult M_Comment()
        {
            ViewBag.NewComment = "New";
            return View();
        }
        public IActionResult M_Comment_Create_Modify (string ifly_Pax_ID,
                                                      string Pax_name,
                                                      string SEQ_Seat_ST,
                                                      string PNR_Ticket,
                                                      string Class_SSR,
                                                      string Remark)
        {

            ViewBag.ifly_Pax_ID = new DataAPI().ReturnValueFromString(ifly_Pax_ID,"_");
            ViewBag.Pax_name = new DataAPI().ReturnValueFromString(Pax_name,"_");
            ViewBag.SEQ_Seat_ST = new DataAPI().ReturnValueFromString(SEQ_Seat_ST, "_");
            ViewBag.PNR_Ticket = new DataAPI().ReturnValueFromString(PNR_Ticket,"_");
            ViewBag.Class_SSR = new DataAPI().ReturnValueFromString(Class_SSR,"_");
            ViewBag.Remark = new DataAPI().ReturnValueFromString(Remark,"_");
            ViewBag.FlightInfo = HttpContext.Session.GetString("FlightInfor");
            return View(new SelectBoxViewModel
            {
                Items = new List<string>()
                {
                    "Active",
                    "Delete"
                }

            }); 
        }
        public IActionResult M_Input_SSR(string ifly_Pax_ID,
                                         string Pax_name,
                                         string SEQ_Seat_ST,
                                         string PNR_Ticket,
                                         string Class_SSR,
                                         string Remark)
        {

            if (Class_SSR != null)
            {
                string[] _Class_SSR = new DataAPI().ReturnValueFromString(Class_SSR, "_").Split("/");
                string ssr = _Class_SSR[1];
                if (!string.IsNullOrEmpty(ssr))
                {
                    string[] arr_ssr = ssr.Split(' ');
                    ViewBag.arr_ssr = arr_ssr;
                }
            }
            ViewBag.FlightInfo = HttpContext.Session.GetString("FlightInfor");
            ViewBag.Pax_name = new DataAPI().ReturnValueFromString(Pax_name,"_");
            return View();
        }
        public IActionResult M_Input_Baggage(string ifly_Pax_ID,
                                                      string Pax_name,
                                                      string SEQ_Seat_ST,
                                                      string PNR_Ticket,
                                                      string Class_SSR,
                                                      string Remark)
        {
            ViewBag.ifly_Pax_ID = new DataAPI().ReturnValueFromString(ifly_Pax_ID, "_");
            ViewBag.Pax_name = new DataAPI().ReturnValueFromString(Pax_name, "_");
            ViewBag.SEQ_Seat_ST = new DataAPI().ReturnValueFromString(SEQ_Seat_ST, "_");
            ViewBag.PNR_Ticket = new DataAPI().ReturnValueFromString(PNR_Ticket, "_");
            ViewBag.Class_SSR = new DataAPI().ReturnValueFromString(Class_SSR, "_");
            ViewBag.Remark = new DataAPI().ReturnValueFromString(Remark, "_");
            ViewBag.FlightInfo = HttpContext.Session.GetString("FlightInfor");
            return View();
        }
        public IActionResult M_OffLoad(string ifly_Pax_ID,
                                       string Pax_name,
                                       string SEQ_Seat_ST,
                                       string PNR_Ticket,
                                       string Class_SSR,
                                       string Remark)
        {
            ViewBag.ifly_Pax_ID = new DataAPI().ReturnValueFromString(ifly_Pax_ID, "_");
            ViewBag.Pax_name = new DataAPI().ReturnValueFromString(Pax_name, "_");
            ViewBag.SEQ_Seat_ST = new DataAPI().ReturnValueFromString(SEQ_Seat_ST, "_");
            ViewBag.PNR_Ticket = new DataAPI().ReturnValueFromString(PNR_Ticket, "_");
            ViewBag.Class_SSR = new DataAPI().ReturnValueFromString(Class_SSR, "_");
            ViewBag.Remark = new DataAPI().ReturnValueFromString(Remark, "_");
            ViewBag.FlightInfo = HttpContext.Session.GetString("FlightInfor");
            ViewData["Seat"] = new DataAPI().ReturnValueFromString(SEQ_Seat_ST, "_").Split("/")[1].ToString();
            return View();
        }
        public IActionResult M_SeatMap(string Pax_name,
                                       string SEQ_Seat_ST,
                                       string _Action)
        {
            ViewBag.Pax_name = new DataAPI().ReturnValueFromString(Pax_name, "_");
            ViewBag.Action = new DataAPI().ReturnValueFromString(_Action, "_");
            ViewBag.SEQ_Seat_ST = new DataAPI().ReturnValueFromString(SEQ_Seat_ST, "_");
            return View();
        }
    }
}
