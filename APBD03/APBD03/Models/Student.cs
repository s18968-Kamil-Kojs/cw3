using System;
using Microsoft.AspNetCore.Mvc;

namespace APBD03.Models {

    public class Student {
        public int IdStudent { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IndexNumber { get; set; }

        public Student() {
        
        }
    }
}
