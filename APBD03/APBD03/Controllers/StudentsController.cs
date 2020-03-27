﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APBD03.DAL;
using APBD03.Models;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace APBD03.Controllers {

    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase {
        private readonly IDbService dbService;
        private ICollection<Student> _students;
        string connectionString = "Data Source=db-mssql16.pjwstk.edu.pl;Initial Catalog=s18968;User ID=inzs18968;Password=admin123";

        public StudentsController(IDbService dbService) {
            this.dbService = dbService;
            this._students = (List<Student>) dbService.GetStudents();
        }

        [HttpGet]
        public IActionResult GetStudents(string orderBy) {

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand()) {
                command.Connection = connection;
                command.CommandText = "select s.FirstName, s.LastName, s.BirthDate, e.Semester, st.Name from Student s, Enrollment e, Studies st where s.IdEnrollment = e.IdEnrollment and e.IdStudy = st.IdStudy;";

                connection.Open();
                var dr = command.ExecuteReader();
                while (dr.Read()) {
                    var student = new Student {
                        FirstName = dr["FirstName"].ToString(),
                        LastName = dr["LastName"].ToString(),
                        BirthDate = dr["BirthDate"].ToString(),
                        SemesterNumber = dr["Semester"].ToString(),
                        StudiesName = dr["Name"].ToString()
                    };
                    _students.Add(student);
                }
                return Ok(_students);
            }
        }

        [HttpGet("{indexNumber}")]
        public IActionResult GetStudent(string indexNumber) {
            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand()) {
                command.Connection = connection;
                command.CommandText = "select s.FirstName, s.LastName, e.Semester, e.StartDate from Student s, Enrollment e where s.IdEnrollment = e.IdEnrollment and s.IndexNumber = @index";
                command.Parameters.AddWithValue("index", indexNumber);
                connection.Open();

                var dr = command.ExecuteReader();
                while (dr.Read()) {
                    var student = new Student {
                        FirstName = dr["FirstName"].ToString(),
                        LastName = dr["LastName"].ToString(),
                        SemesterNumber = dr["Semester"].ToString()
                    };
                }
                return Ok(_students);
            }

            return NotFound("Nie znaleziono obiektu");
        }

        [HttpPost]
        public IActionResult CreateStudent(Student student) {
            //student.IndexNumber = $"s{new Random().Next(1, 20000)}";
            return Ok(student);
        }

        [HttpPut("{id}")]
        public IActionResult PutStudent(int id, Student student) {
            //student.IdStudent = id;
            //student.IndexNumber = $"s{new Random().Next(1, 20000)}";
            return Ok(200 + " Aktualizacja dokonczona");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id) {
            return Ok(200 + " Usuwanie dokonczone");
        }
    }
}
