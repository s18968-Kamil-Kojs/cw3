using System;
using System.Collections.Generic;
using APBD03.Models;

namespace APBD03.DAL {

    public class MockDbService : IDbService{
        private static IEnumerable<Student> _students;

        public MockDbService() {
            /*_students = new List<Student> {
                new Student{IdStudent = 1, FirstName = "Jan", LastName = "Morwin"},
                new Student{IdStudent = 2, FirstName = "Anna", LastName = "Sidon"},
                new Student{IdStudent = 3, FirstName = "Kasia", LastName = "Aless"}
            };*/
            _students = new List<Student>();
        }

        public IEnumerable<Student> GetStudents() {
            return _students;
        }
    }
}
