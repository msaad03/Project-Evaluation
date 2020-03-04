using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FypEvaluationSystem.Models
{
    public class StudentRegistration
    {


        //[Required]
        //[Display(Name="Leader ID")]
        //[RegularExpression("^[k|l|f|i|p|K|L|F|I|P][0-9]{6}$", ErrorMessage = "Id Must be in the k163782 format")]
        
        [Required]
        public List<Student> Members { get; set; }

        [Required]
        public string Supervisor { get; set; }

        [Required]
        [Display(Name ="Co-Supervisor")]
        public string CoSupervisor { get; set; }

        [Required]
        [Display(Name = "Project-Title")]
        public string ProjectTitle { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }


        [DataType(DataType.Password)]
        [Display(Name="ConfirmPassword")]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}