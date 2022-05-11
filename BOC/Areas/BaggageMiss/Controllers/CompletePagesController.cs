using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BOC.Models;
using System;

namespace BOC.Areas.BaggageMiss.Controllers
{
    [Area("BaggageMiss")]
    public class CompletePagesController : Controller
    {
        public IActionResult Index()
        {
            //SaveLog.WriteLog("Start");
            string lang = HttpContext.Request.Query["t_flag"].ToString();
            ViewBag.Lang= lang;
            //SaveLog.WriteLog(lang);
            string bagmiss_id = HttpContext.Request.Query["t_bagmiss_id"].ToString();
            ViewBag.Bagmiss_Id= bagmiss_id;
            return View();
        }
    }
}
