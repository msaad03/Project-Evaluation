using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace FypEvaluationSystem.Models
{
    public class JuryLogin
    {

        [Display(Name = "Email")]
        //[RegularExpression("^[a-zA-Z.]{1,}@nu.edu.pk$", ErrorMessage = "Please enter valid email Address")]
        [Required(ErrorMessage = "Email is Required")]
        public string Teacher_email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}