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
        public IEnumerable<TeacherView> Login(TeacherViewModel obj)
        {
            if (!ModelState.IsValid)
            {
                //return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid data.");
                return null;
            }
            using (MyFirstDBEntities entities = new MyFirstDBEntities())
            {
                var user = entities.Teacher
                .Where(p => p.Teacher_email == obj.Teacher_email && p.Password == obj.Password)
                .ToList()
                .Select(p => new TeacherView
                {
                    Teacher_id = p.Teacher_id,
                    Teacher_name = p.Teacher_name,
                    Teacher_email = p.Teacher_email,
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

        [HttpPost]
        public HttpResponseMessage Add(List<Teacher> obj)
        {
            if (!ModelState.IsValid)
            {
                //return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid data.");
                return null;
            }
            using (MyFirstDBEntities entities = new MyFirstDBEntities())
            {
                for (int i = 0; i < obj.Count; i++)
                {
                    AdminDataAccess.Teacher o1 = new AdminDataAccess.Teacher();
                    o1.Teacher_name = obj[i].name;
                    o1.Teacher_email = obj[i].email;
                    o1.Password = obj[i].password;

                    entities.Teacher.Add(o1);
                    entities.SaveChanges();
                }
            }
            return Request.CreateResponse(HttpStatusCode.OK, "Teachers Added");
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
    public string Teacher_email { get; set; }
    public string Password { get; set; }
}
public class Teacher
{
    public string name { get; set; }
    public string email { get; set; }
    public string password { get; set; }

    public Teacher()
    {
    }

    public Teacher(string name, string email, string password)
    {
        this.name = name;
        this.email = email;
        this.password = password;
    }
}
