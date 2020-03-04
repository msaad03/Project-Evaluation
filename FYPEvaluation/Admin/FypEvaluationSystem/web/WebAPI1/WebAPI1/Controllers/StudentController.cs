using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AdminDataAccess;

namespace WebAPI1.Controllers
{
    public class StudentController : ApiController
    {
        // GET LIST OF ALL STUDENTS
        public IEnumerable<StudentView> Get()
        {
            using (MyFirstDBEntities entities = new MyFirstDBEntities())
            {
                return entities.Student
                .ToList()
                .Select(p => new StudentView
                {
                    Student_id = p.Student_id,
                    Group_id = p.Group_id,
                    Student_email = p.Student_email,
                    Student_name = p.Student_name
                })
                .AsEnumerable();
                //return "All student details";
            }
        }

        // GET STUDENT DETAIL BY STUDENT ID
        [Route("api/student/Get/{test}")]
        public IEnumerable<StudentView> Get(string test)
        {
            using (MyFirstDBEntities entities = new MyFirstDBEntities())
            {
                return entities.Student
                .Where(p => p.Student_id.Equals(test))
                .ToList()
                .Select(p => new StudentView
                {
                    Student_id = p.Student_id,
                    Group_id = p.Group_id,
                    Student_email = p.Student_email,
                    Student_name = p.Student_name
                })
                .AsEnumerable();
                //return "Student details with ID: " + test;
            }
        }

        // POST STUDENT LOGIN AUTHENTICATION
        [HttpPost]
        public HttpResponseMessage Login(StudentViewModel obj)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid data.");
            }
            using (MyFirstDBEntities entities = new MyFirstDBEntities())
            {
                var user = entities.Student.FirstOrDefault(p => p.Student_id.Equals(obj.Student_id) && p.Password.Equals(obj.Password));
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
public class StudentView
{
    public string Student_id { get; set; }
    public string Student_email { get; set; }
    public string Student_name { get; set; }
    public int Group_id { get; set; }
}

public class StudentViewModel
{
    public string Student_id { get; set; }
    public string Password { get; set; }
}
