using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace FypEvaluationSystem.Models
{
    public class PreFormInfo
    {

        public string Campus { get; set; }
        public string Department { get; set; }
        public string Project { get; set; }
        public string Semester { get; set; }

        [RegularExpression("[0-9]{4}", ErrorMessage = "Please enter year in format 2000")]
        [Required(ErrorMessage ="Year is required")]
        public string Year { get; set; }

        [Required]
        [Range(0, 100)]
        [Display(Name = "Total Weightage")]
        public double TotalMarks { get; set; }

    }
}