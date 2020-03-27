using System;
using Microsoft.AspNetCore.Mvc;

namespace APBD03.Models {

    public class Student {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BirthDate { get; set; }
        public string StudiesName { get; set; }
        public string SemesterNumber { get; set; }

        public Student() {
        
        }
    }
}
