using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FypEvaluationSystem.Models
{
    public class GroupInfo
    {
        public string Supervisor_name { get; set; }
        public string Cosupervisor_name { get; set; }

        public string Project_title { get; set; }

        public int Group_id { get; set; }
        public string Leader_id { get; set; }

        public List<StudentInfo> Student_list { get; set; }


    }
}