using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FypEvaluationSystem.Models
{
    public class AssesmentCriteria
    {
        [Required]
        public string Field { get; set; }
        
        [Required]
        public double Marks { get; set; }
    }
}