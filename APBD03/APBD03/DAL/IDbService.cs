using System;
using System.Collections.Generic;
using APBD03.Models;

namespace APBD03.DAL {

    public interface IDbService {

        public IEnumerable<Student> GetStudents();
    }
}
