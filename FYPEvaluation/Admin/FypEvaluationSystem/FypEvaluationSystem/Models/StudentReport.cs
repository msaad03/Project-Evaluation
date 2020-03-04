using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FypEvaluationSystem.Models
{
    public class StudentReport
    {
        public string Evaluation_type { get; set; }
        public double Obtained_marks { get; set; }
        public double Total_marks { get; set; }
        public int Project_no { get; set; }
    }
}