using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using APBD03.DTOs.Requests;
using APBD03.Models;
using APBD03.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace APBD03.Controllers {

    [ApiController]
    [Route("api/enrollments")]
    public class EnrollmentsController :ControllerBase {
        string connectionString = "Data Source=db-mssql16.pjwstk.edu.pl;Initial Catalog=s18968;User ID=inzs18968;Password=admin123";
        private IStudentDbService _service;

        public EnrollmentsController(IStudentDbService service) {
            this._service = service;
        }

        [HttpPost]
        public IActionResult SignUpStudentForStudies(EnrollStudentRequest request) {
            string response = _service.enrollStudent(request);
            return Ok(response);
        }

        [HttpPost]
        [Route("api/enrollments/promotions")]
        public IActionResult PromoteStudents(PromoteStudentsRequest request) {
            List<Enrollment> list = _service.promoteStudents(request);
            return Ok(list);
        }
    }
}
