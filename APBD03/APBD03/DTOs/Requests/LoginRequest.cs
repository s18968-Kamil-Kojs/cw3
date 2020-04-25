using System;
using System.ComponentModel.DataAnnotations;

namespace APBD03.DTOs.Requests {

    public class LoginRequest {

        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
