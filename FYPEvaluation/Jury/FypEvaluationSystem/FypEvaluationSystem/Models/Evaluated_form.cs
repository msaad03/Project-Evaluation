using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FypEvaluationSystem.Models
{
    public class Evaluated_form
    {
        public string Leader_id { get; set; }
        public string Leader_name { get; set; }

        public string Form_title { get; set; }
        public string Supervisor_name { get; set; }
        public string Cosupervisor_name { get; set; }
        public string Project_title { get; set; }
        public string Form_status { get; set; }
        public int Form_id { get; set; }
        public int Form_template_id { get; set; }
        public string Department { get; set; }
        public string Campus { get; set; }
        public double Form_weightage { get; set; }
        public int Year { get; set; }
        public string Semester { get; set; }
        public int Project_no { get; set; }

        public List<Evaluated_criteria_template> criteria_template { get; set; }
        public List<Evaluated_criteria_marks> criteria_marks { get; set; }
        public List<Evaluated_field_template> field_template { get; set; }
        public List<Evaluated_field_text> field_text { get; set; }
        public Evaluated_form()
        {
            criteria_template = new List<Evaluated_criteria_template>();
            field_template = new List<Evaluated_field_template>();
            criteria_marks = new List<Evaluated_criteria_marks>();
            field_text = new List<Evaluated_field_text>();
        }
    }
    public class Evaluated_criteria_marks
    {
        public string Student_id { get; set; }
        public double Obtained_marks { get; set; }
    }
    public class Evaluated_field_text
    {
        public string Field_text1 { get; set; }
    }
    public class Evaluated_criteria_template
    {
        public string Criteria_title { get; set; }
        public double Total_marks { get; set; }
    }
    public class Evaluated_field_template
    {
        public string Field_title { get; set; }
    }

}