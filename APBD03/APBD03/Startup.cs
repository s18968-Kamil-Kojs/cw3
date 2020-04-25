using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APBD03.DAL;
using APBD03.Handlers;
using APBD03.Middlewares;
using APBD03.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace APBD03 {
    public class Startup {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            //services.AddAuthentication("AuthenticationBasic")
            //        .AddScheme<AuthenticationSchemeOptions, BasicAuthHandler>("AuthenticationBasic", null);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options => {
                        options.TokenValidationParameters = new TokenValidationParameters {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidIssuer = "Gakko",
                            ValidAudience = "Students",
                            //haslo w formie surowej jako tekst -> nie mogę znaleźć na macu zakładki "manage secret keys"
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(/*Configuration["SecretKey"]*/"haslo"))
                        };
                    });
            services.AddSingleton<IDbService, MockDbService>();
            services.AddTransient<IStudentDbService, SqlServerStudentDbService>();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            //app.UseMiddleware<LoggingMiddleware>();

            /*app.Use(async (context, next) => {
                if (!context.Request.Headers.ContainsKey("Index")) {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Nie podałeś indeksu");
                    return;
                }

                string index = context.Request.Headers["Index"].ToString();
                //check in db
                string connectionString = "Data Source=db-mssql16.pjwstk.edu.pl;Initial Catalog=s18968;User ID=inzs18968;Password=admin123";

                using (var connection = new SqlConnection(connectionString))
                using (var command = new SqlCommand()) {
                    command.Connection = connection;
                    connection.Open();
                   
                    //Check if student exists
                    command.CommandText = "select * from Student where IndexNumber="+index;

                    var dr = command.ExecuteReader();
                    if (!dr.Read()) {
                        await context.Response.WriteAsync("Student o podanym indeksie nie istnieje");
                        return;
                    }
                }


                await next();
            });*/

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
