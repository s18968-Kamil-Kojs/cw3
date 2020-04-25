using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using APBD03.DTOs.Requests;
using APBD03.Models;
using Microsoft.AspNetCore.Mvc;

namespace APBD03.Services {

    public interface IStudentDbService {

        public string enrollStudent(EnrollStudentRequest request);
        public List<Enrollment> promoteStudents(PromoteStudentsRequest request);
    }
}
