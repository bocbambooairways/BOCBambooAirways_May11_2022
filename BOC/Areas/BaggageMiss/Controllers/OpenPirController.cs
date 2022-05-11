using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using BOC.Models;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using BOC.Areas.BaggageMiss.Models;
using Nancy.Json;

namespace BOC.Areas.BaggageMiss.Controllers
{
    [Area("BaggageMiss")]
    public class OpenPirController : Controller
    {
        
        public IActionResult Index(string t_bagmissdetail_id)
        {
            string strMess = string.Empty;
            //Get Session Token
            var t_token = HttpContext.Session.GetString("Token") == null ? GetToken() : HttpContext.Session.GetString("Token");
            var t_PNR = HttpContext.Session.GetString("PNR");
            var t_FltNo = HttpContext.Session.GetString("FltNo");
            var t_bagmiss_id = HttpContext.Session.GetString("BagMiss_ID");
            string Content = CallAPI.MisBagProfileGet(t_PNR, t_FltNo, t_bagmiss_id, t_token);
            var oData = Newtonsoft.Json.JsonConvert.DeserializeObject<PirModel>(Content);
           
 
            string choosecolor = CallAPI.MissBagDescriptionGet(Int32.Parse(t_bagmissdetail_id), t_token);
            BaggageMissDesc desc = Newtonsoft.Json.JsonConvert.DeserializeObject<BaggageMissDesc>(choosecolor);

            ViewBag.OtherComments = desc.Data.Item.ToString() + desc.Data.Remark.ToString();
            List<BagDesc> BagDesc = desc.Data.BagDesc;
            foreach (BagDesc b in BagDesc)
            {
                if (b.UserCheck == true)
                {
                    ViewBag.ColorType += b.BagDetailCode.ToString();
                }
            }
         
            Int32 _Result = Int32.Parse(desc.ResultCode.ToString());
            string Message = desc.Message.ToString();
            if (_Result != 0)
            {
                strMess = Message.ToString();
                return Json(new { mess = strMess });
            }
            else
            {
                return View(oData);
            }
            


                

           
        }
        public string GetToken()
        {
            //Get path url api AUTO SYSTEM LOGIN AND GET TOKEN
            Url login = new Url();
            string uri = login.Get("Login");
            HttpClient SysLogin = new HttpClient();
            var nvc = new List<KeyValuePair<string, string>>();
            nvc.Add(new KeyValuePair<string, string>("UserCode", "AUTO_WEB"));
            nvc.Add(new KeyValuePair<string, string>("Password", "-_5#4eT6AF'*B6ey78#P"));
            nvc.Add(new KeyValuePair<string, string>("DeviceID", "C11FCC37-16D6-11EB-BADE-000C29D93A49"));
            var reqsys = new HttpRequestMessage(HttpMethod.Post, uri) { Content = new FormUrlEncodedContent(nvc) };

            string ContentSys;
            HttpResponseMessage res_sys;
            res_sys = SysLogin.SendAsync(reqsys).Result;
            ContentSys = res_sys.Content.ReadAsStringAsync().Result;
            var oDataSys = JObject.Parse(ContentSys);
            // Save  Session Token
            var token = oDataSys["Data"]["Token"].ToString();
            return token;

        }
    }
}



