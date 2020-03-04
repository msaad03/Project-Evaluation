using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FypEvaluationSystem.Models
{
    public class Student2
    {
        public string Student_id { get; set; }
        public string Student_email { get; set; }
        public string Student_name { get; set; }
        public string Password { get; set; }
        public Student2(string id, string email, string name, string password)
        {
            Student_id = id;
            Student_email = email;
            Student_name = name;
            Password = password;
        }
    }
}