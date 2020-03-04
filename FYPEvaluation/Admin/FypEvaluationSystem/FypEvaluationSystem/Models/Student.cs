using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FypEvaluationSystem.Models
{
    public class Student
    {
        public string ID { get; set; }
        public string Name { get; set; }

        public Student(string id, string name)
        {

            this.ID = id;
            this.Name = name;
        }

    }
}