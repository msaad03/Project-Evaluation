using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using FypEvaluationSystem.Models;
using Newtonsoft.Json;

namespace FypEvaluationSystem.Controllers
{
    public class LoginAdminController : Controller
    {
        // GET: LoginJury
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(AdminLogin login)
        {
            if (!ModelState.IsValid)
            {

                return View();
            }

            if (login.Admin_email == null || login.Password == null)
            {
                ModelState.AddModelError("", "ID or password is Invalid");
                return View();
            }
            AdminViewModel obj = new AdminViewModel();
            obj.Admin_Email = login.Admin_email;
            obj.Password = login.Password;

            using (WebClient client = new WebClient())
            {
                client.BaseAddress = "http://localhost:59782/api/";
                var url = "Admin/Login";
                client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                string data = JsonConvert.SerializeObject(obj);
                var response = client.UploadString(url, data);

                var result = JsonConvert.DeserializeObject<List<AdminView>>(response);

                if (result.Count != 0)
                {
                    FormsAuthentication.SetAuthCookie(login.Admin_email, false);
                    return RedirectToAction("Index", "Admin");
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
        public class AdminView
        {
            public string Admin_id { get; set; }
            public string Admin_name { get; set; }
            public string Admin_email { get; set; }
        }
        public class AdminViewModel
        {
            public string Admin_Email { get; set; }
            public string Password { get; set; }
        }

    }
}