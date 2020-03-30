using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APBD03.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace APBD03.Controllers {

    [ApiController]
    [Route("api/enrollments")]
    public class EnrollmentsController : ControllerBase {
        // GET: /<controller>/

        [HttpPost]
        public IActionResult SignUpStudentForStudies(Enrollment enrollment) {
            if(enrollment.IndexNumber != null && enrollment.FirstName != null && enrollment.LastName != null && enrollment.BirthDate != null && enrollment.Studies != null) {
                return Ok(enrollment);
            } else {
                return Ok(400);
            }
        }
    }
}
