using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace EducationTrainSystem.Areas.APIv2.Controllers
{
    public class UserController : Controller
    {

        public JsonResult CheckLogin()
        {
            return Json(new { Loged = User.Identity.IsAuthenticated, Name = User.Identity.Name }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SetLogin()
        {
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, "hello", DateTime.Now, DateTime.Now.AddDays(7), true, "custom");
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
            Response.Cookies.Add(cookie);
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }
    }
}