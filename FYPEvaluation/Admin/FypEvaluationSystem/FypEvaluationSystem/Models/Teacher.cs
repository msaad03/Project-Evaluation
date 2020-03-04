﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FypEvaluationSystem.Models
{
    public class Teacher
    {
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }

        public Teacher()
        {
        }

        public Teacher(string name, string email, string password)
        {
            this.name = name;
            this.email = email;
            this.password = password;
        }
    }
}