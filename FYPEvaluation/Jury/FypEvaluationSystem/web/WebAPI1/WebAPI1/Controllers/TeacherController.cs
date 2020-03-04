using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AdminDataAccess;

namespace WebAPI1.Controllers
{
    public class TeacherController : ApiController
    {
        // GET LIST OF ALL TEACHERS
        public IEnumerable<TeacherView> Get()
        {
            using (MyFirstDBEntities entities = new MyFirstDBEntities())
            {
                return entities.Teacher
                .ToList()
                .Select(p => new TeacherView
                {
                    Teacher_id = p.Teacher_id,
                    Teacher_name = p.Teacher_name,
                    Teacher_email = p.Teacher_email,
                })
                .AsEnumerable();
                //return "All teacher details";
            }
        }

        // GET TEACHER DETAIL BY TEACHER ID
        public IEnumerable<TeacherView> Get(int id)
        {
            using (MyFirstDBEntities entities = new MyFirstDBEntities())
            {
                return entities.Teacher
                .Where(p => p.Teacher_id == id)
                .ToList()
                .Select(p => new TeacherView
                {
                    Teacher_id = p.Teacher_id,
                    Teacher_name = p.Teacher_name,
                    Teacher_email = p.Teacher_email,
                })
                .AsEnumerable();
                //return "All teacher details with ID: " + id.ToString();
            }
        }

        // POST TEACHER LOGIN AUTHENTICATION
        [HttpPost]
        public HttpResponseMessage Login(TeacherViewModel obj)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid data.");
            }
            using (MyFirstDBEntities entities = new MyFirstDBEntities())
            {
                var user = entities.Teacher.FirstOrDefault(p => p.Teacher_id == obj.Teacher_id && p.Password.Equals(obj.Password));
                if (user != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Login successful");
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Invalid credentials");
                }
            }
        }
    }
}
public class TeacherView
{
    public int Teacher_id { get; set; }
    public string Teacher_email { get; set; }
    public string Teacher_name { get; set; }
}
public class TeacherViewModel
{
    public int Teacher_id { get; set; }
    public string Password { get; set; }
}
