using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace FypEvaluationSystem.Models
{
    public class Jury
    {
        
        public string Teacher_ID { get; set; }

        public string Teacher_Name { get; set; }

        public string Teacher_Email { get; set; }

        public string Password { get; set; }
        
    }
}