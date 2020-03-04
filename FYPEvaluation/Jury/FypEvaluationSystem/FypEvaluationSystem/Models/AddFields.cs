using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FypEvaluationSystem.Models
{
    public class AddFields
    {

        
        public int FormId { get; set; }
        [Required]
        public string FormTitle { get; set; }

        [Required(ErrorMessage ="Assesment Criteria Required")]
        public string Hidden { get; set;  }
        [Required(ErrorMessage = "Fields Required")]
        public string Hidden2 { get; set; }

        public List<AssesmentCriteria> AssesmentCriteria { get; set; }
        public List<string> Fields { get; set; }
        public PreFormInfo preFormInfo { get; set; }


    }
}