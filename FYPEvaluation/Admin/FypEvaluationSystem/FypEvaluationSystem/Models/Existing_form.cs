using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FypEvaluationSystem.Models
{
    public class Existing_form
    {
        public int Form_id { get; set; }
    	public double Form_weightage { get; set; }
    	public string Form_title { get; set; }
    	public string Department { get; set; }
    	public string Campus { get; set; }
    	public int Year { get; set; }
    	public string Semester { get; set; }
    	public int Project_no { get; set; }

        public List<Existing_criteria_template> criteria_template { get; set; }
        public List<Existing_field_template> field_template { get; set; }
        public Existing_form()
        {
            criteria_template = new List<Existing_criteria_template>();
            field_template = new List<Existing_field_template>();
        }
    }
    public class Existing_criteria_template
    {
        public int Criteria_template_id { get; set; }
        public string Criteria_title { get; set; }
        public double Total_marks { get; set; }
    }
    public class Existing_field_template
    {
        public int Field_template_id { get; set; }
        public string Field_title { get; set; }
    }
}