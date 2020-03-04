using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FypEvaluationSystem.Models
{
    public class Group
    {
        public int GroupId { get; set; }
        public string Supervisor { get; set; }
        public string CoSupervisor { get; set; }
        public string ProjectTitle { get; set; }
        public string LeaderId { get; set; }

        public List<Student> members { get; set; }


    }
}