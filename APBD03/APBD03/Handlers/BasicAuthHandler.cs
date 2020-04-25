﻿using System;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace APBD03.Handlers {

    public class BasicAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>{

        public BasicAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> oprions, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(oprions, logger, encoder, clock){

        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync() {
            if (!Request.Headers.ContainsKey("Authorization")) {
                return AuthenticateResult.Fail("Missing authorization header");
            }

            //"Authorization: Basic asdasd" => bajty -> "jan123:haslo"
            var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
            var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
            var credentials = Encoding.UTF8.GetString(credentialBytes).Split(":");

            if(credentials.Length != 2) {
                return AuthenticateResult.Fail("Incorrect authentication header value");
            }

            //TODO check credentials in DB



            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Name, "jan123"),
                new Claim(ClaimTypes.Role, "admin"),
                new Claim(ClaimTypes.Role, "student"),
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }
}
