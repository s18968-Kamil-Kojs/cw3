using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace APBD03.Middlewares {

    public class LoggingMiddleware {
        private readonly RequestDelegate _next;
        
        public LoggingMiddleware(RequestDelegate next) {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext) {
            //Our code
            await _next(httpContext);
        }
    }
}
