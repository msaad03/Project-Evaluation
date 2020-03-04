using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using AdminDataAccess;

namespace WebAPI1.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AdminController : ApiController
    {
        // GET LIST OF ALL ADMINS
        public IEnumerable<AdminView> Get()
        {
            using (MyFirstDBEntities entities = new MyFirstDBEntities())
            {
                return entities.Admin
                .ToList()
                .Select(p => new AdminView
                {
                    Admin_id = p.Admin_id,
                    Admin_name = p.Admin_name,
                    Admin_email = p.Admin_email,
                })
                .AsEnumerable();
            }
        }

        // GET ADMIN DETAILS BY ID
        public IEnumerable<AdminView> Get(int id)
        {
            using (MyFirstDBEntities entities = new MyFirstDBEntities())
            {
                return entities.Admin
                .Where(p => p.Admin_id == id)
                .ToList()
                .Select(p => new AdminView
                {
                    Admin_id = p.Admin_id,
                    Admin_name = p.Admin_name,
                    Admin_email = p.Admin_email,
                })
                .AsEnumerable();
            }
        }

        // POST ADMIN LOGIN AUTHENTICATION
        [HttpPost]
        public IEnumerable<AdminView> Login(AdminViewModel obj)
        {
            if (!ModelState.IsValid)
            {
                //return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid data.");
                return null;
            }
            using (MyFirstDBEntities entities = new MyFirstDBEntities())
            {
                //var user = entities.Admin.FirstOrDefault(p => p.Admin_id == obj.Admin_id && p.Password.Equals(obj.Password));
                var user = entities.Admin
                .Where(p => p.Admin_email == obj.Admin_email && p.Password == obj.Password)
                .ToList()
                .Select(p => new AdminView
                {
                    Admin_id = p.Admin_id,
                    Admin_name = p.Admin_name,
                    Admin_email = p.Admin_email,
                })
                .AsEnumerable();
                if (user != null)
                {
                    return user;
                    //return Request.CreateResponse(HttpStatusCode.OK, "Login successful");
                }
                else
                {
                    return null;
                    //return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Invalid credentials");
                }
            }
        }
    }
}
public class AdminView
{
    public int Admin_id { get; set; }
    public string Admin_name { get; set; }
    public string Admin_email { get; set; }
}
public class AdminViewModel
{
    public string Admin_email { get; set; }
    public string Password { get; set; }
}