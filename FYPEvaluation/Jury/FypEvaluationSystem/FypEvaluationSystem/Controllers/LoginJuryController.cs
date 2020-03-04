using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FypEvaluationSystem.Models;
using System.Web.Security;
using System.Net;
using Newtonsoft.Json;

namespace FypEvaluationSystem.Controllers
{
    public class LoginJuryController : Controller
    {
        // GET: LoginJury
        
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(JuryLogin login)
        {
            if (login.Teacher_email == null || login.Password == null)
            {
                ModelState.AddModelError("", "ID or password is Invalid");
                return View();
            }
            TeacherViewModel obj = new TeacherViewModel();
            obj.Teacher_email = login.Teacher_email;
            obj.Password = login.Password;

            using (WebClient client = new WebClient())
            {
                client.BaseAddress = "http://localhost:59782/api/";
                var url = "Teacher/Login";
                client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                string data = JsonConvert.SerializeObject(obj);
                var response = client.UploadString(url, data);

                var result = JsonConvert.DeserializeObject<List<TeacherView>>(response);

                if (result.Count != 0)
                {
                    FormsAuthentication.SetAuthCookie(result[0].Teacher_id, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "ID or password is Invalid");
                    return View();
                }
            }
        }

        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
        public class TeacherViewModel
        {
            public string Teacher_email { get; set; }
            public string Password { get; set; }
        }
        public class TeacherView
        {
            public string Teacher_id { get; set; }
            public string Teacher_email { get; set; }
            public string Teacher_name { get; set; }
        }
    }
}