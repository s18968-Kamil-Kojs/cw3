using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APBD03.DAL;
using APBD03.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace APBD03.Controllers {

    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase {
        private readonly IDbService dbService;

        public StudentsController(IDbService dbService) {
            this.dbService = dbService;
        }

        [HttpGet]
        public IActionResult GetStudents(string orderBy) {
            return Ok(dbService.GetStudents());
        }

        [HttpGet("{id}")]
        public IActionResult GetStudent(int id) {
            if(id == 1) {
                return Ok("Ania");
            } else if(id == 2){
                return Ok("Kowalski");
            }
            return NotFound("Nie znaleziono obiektu");
        }

        [HttpPost]
        public IActionResult CreateStudent(Student student) {
            student.IndexNumber = $"s{new Random().Next(1, 20000)}";
            return Ok(student);
        }

        [HttpPut("{id}")]
        public IActionResult PutStudent(int id, Student student) {
            student.IdStudent = id;
            student.IndexNumber = $"s{new Random().Next(1, 20000)}";
            return Ok(200 + " Aktualizacja dokonczona");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id) {
            return Ok(200 + " Usuwanie dokonczone");
        }
    }
}
