﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Security;
using EducationTrainSystem.Library;
using EducationTrainSystem.Models;

namespace EducationTrainSystem.APIv1
{
    public class UserController : ApiController
    {
        private EducationTrain model = new EducationTrain();

        public bool GetIsLogin()
        {
            return User.Identity.IsAuthenticated;
        }
        public Utils.RichMessage GetLogIn(string code, string password)
        {
            var queryUser = model.Users.FirstOrDefault(u => u.Code == code && u.Password == password);
            var message = new Utils.RichMessage(false, "");
            if (queryUser == null) { message.message = "not found"; return message; }
            var ticket = new FormsAuthenticationTicket(1, queryUser.Code, DateTime.Now, DateTime.Now.AddDays(2), true, "");
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
            HttpContext.Current.Response.AppendCookie(cookie);
            message.success = true;
            return message;
        }

        [HttpGet]
        [Route("Apiv1/User/SignOut")]
        public bool SignOut() {
            FormsAuthentication.SignOut();
            return true;
        }
    }
}
