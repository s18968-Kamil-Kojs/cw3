using System;
using System.IdentityModel.Tokens.Jwt;

namespace APBD03.Models {

    public class Token {
        public string TokenItem { get; set; }
        public Guid RefreshToken { get; set; }

        public Token() {
        }
    }
}
