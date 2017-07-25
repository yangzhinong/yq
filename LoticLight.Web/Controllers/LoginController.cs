using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LoticLight.Web.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            string msg = "";
            bool b = Business.Sys_UserService.VerifyLogin(username, password, out msg);
            if (!b)
            {
                ModelState.AddModelError("error", msg);
                return View();
            }
            return Redirect("/Home/Index");
        }

        public ActionResult LoginOut()
        {
            LoticLight.Session.WebSession.RemoveSession();
            return Json(null);
        }
    }
}