using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FypEvaluationSystem.Models
{
    public class Statistics
    {
        public string Status { get; set; }
        public string Count { get; set; }

        public Statistics(string status, string count)
        {
            this.Status = status;
            this.Count = count;
        }
    }
}