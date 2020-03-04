using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FypEvaluationSystem.Models
{
    public class AdminLogin
    {

        [Required(ErrorMessage = "Email is required")]
        //[RegularExpression("^[a-zA-Z.]{1,}@nu.edu.pk$", ErrorMessage = "Please enter valid email Address")]
        public string Admin_email { get; set; }
        [Required(ErrorMessage ="Password is required")]
        public string Password { get; set; }
    }
}