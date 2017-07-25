using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;

namespace LoticLight.Web.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {  
           if (LoticLight.Session.WebSession.Current == null)
            {
                return Redirect("/Login/Login");
             }
           ViewBag.UserName = LoticLight.Session.WebSession.Current.Account.User.UserName;
            return View();
        }
        public ActionResult Default()
        {
            return View();
        }
        public ActionResult MenusData()
        {
            var ms = LoticLight.Session.WebSession.CurrentAccount.initdata;
            return Json(ms);
        }
        public ActionResult inint()
        {
            string data = @"";
            return Content(data);
        }



    }
}