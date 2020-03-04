using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FypEvaluationSystem.Models
{
    public class Group2
    {
        public string Supervisor_name { get; set; }
        public string Cosupervisor_name { get; set; }
        public string Project_title { get; set; }
        public List<Student2> Student_list { get; set; }

        public Group2()
        {
            Student_list = new List<Student2>();
        }
        public Group2(string s, string c, string t, List<Student2> stud)
        {
            Supervisor_name = s;
            Cosupervisor_name = c;
            Project_title = t;
            Student_list = stud;
        }
    }
}