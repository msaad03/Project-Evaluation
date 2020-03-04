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

        [Required]
        [Range(0,100)]
        public double TotalMarks { get; set; }

    }
}