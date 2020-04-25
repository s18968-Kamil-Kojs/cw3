using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using APBD03.DTOs.Requests;
using APBD03.Models;
using APBD03.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace APBD03.Controllers {

    [ApiController]
    [Route("api/enrollments")]
    public class EnrollmentsController :ControllerBase {
        string connectionString = "Data Source=db-mssql16.pjwstk.edu.pl;Initial Catalog=s18968;User ID=inzs18968;Password=admin123";
        private IStudentDbService _service;
        public IConfiguration Configuration { get; set; }

        public EnrollmentsController(IStudentDbService service, IConfiguration configuration) {
            this._service = service;
            this.Configuration = configuration;
        }

        [HttpPost]
        [Authorize(Roles = "employee")]
        public IActionResult SignUpStudentForStudies(EnrollStudentRequest request) {
            string response = _service.enrollStudent(request);
            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = "employee")]
        [Route("api/enrollments/promotions")]
        public IActionResult PromoteStudents(PromoteStudentsRequest request) {
            List<Enrollment> list = _service.promoteStudents(request);
            return Ok(list);
        }

        [HttpGet]
        public IActionResult Login(LoginRequest request) {
            /*Token token = _service.login(request);
            return Ok(token);*/

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand()) {
                command.Connection = connection;
                connection.Open();
                var transaction = connection.BeginTransaction();
                command.Transaction = transaction;

                command.CommandText = "select IndexNumber, Password from Student where IndexNumber=@IndexNumber";
                command.Parameters.AddWithValue("IndexNumber", request.Login);

                var dr = command.ExecuteReader();
                if (!dr.Read()) {
                    dr.Close();
                    return BadRequest("Student nie istnieje");
                }
                dr.Read();
                if (!((string) dr["Password"]).Equals(request.Password)) {
                    return BadRequest("Bledne haslo");
                }

                var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Name, (string) dr["IndexNumber"]),
                new Claim(ClaimTypes.Role, "student"),
                };

                //haslo w formie surowej jako tekst -> nie mogę znaleźć na macu zakładki "manage secret keys"
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(/*Configuration["SecretKey"]*/"haslo"));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: "Gakko",
                    audience: "Students",
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(10),
                    signingCredentials: creds
                );

                return Ok(new
                {
                    accessToken = new JwtSecurityTokenHandler().WriteToken(token),
                    refreshToken = Guid.NewGuid()
                });

            }

        }

        [HttpGet("refresh-token/{token}")]
        public IActionResult RefreshToken(LoginRequest request, string refToken) {
            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand()) {
                command.Connection = connection;
                connection.Open();
                var transaction = connection.BeginTransaction();
                command.Transaction = transaction;

                command.CommandText = "select RefreshToken from Student where IndexNumber=@IndexNumber";
                command.Parameters.AddWithValue("IndexNumber", request.Login);

                var dr = command.ExecuteReader();
                if (!dr.Read()) {
                    dr.Close();
                    return BadRequest("Student nie istnieje");
                }
                dr.Read();
                if (!((string) dr["Password"]).Equals(request.Password)) {
                    return BadRequest("Bledne haslo");
                }
                if(((string) dr["RefreshToken"]).Equals(refToken)) {

                }

                var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Name, (string) dr["IndexNumber"]),
                new Claim(ClaimTypes.Role, "student"),
                };

                //haslo w formie surowej jako tekst -> nie mogę znaleźć na macu zakładki "manage secret keys"
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(/*Configuration["SecretKey"]*/"haslo"));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: "Gakko",
                    audience: "Students",
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(10),
                    signingCredentials: creds
                );

                return Ok(new
                {
                    accessToken = new JwtSecurityTokenHandler().WriteToken(token),
                    refreshToken = Guid.NewGuid()
                });

            }
        }
    }
}
