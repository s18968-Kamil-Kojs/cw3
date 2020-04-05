using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using APBD03.DTOs.Requests;
using APBD03.Models;
using APBD03.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBD03.DAL {
    public class SqlServerStudentDbService : IStudentDbService{
        string connectionString = "Data Source=db-mssql16.pjwstk.edu.pl;Initial Catalog=s18968;User ID=inzs18968;Password=admin123";

        public SqlServerStudentDbService() {

        }

        public string enrollStudent(EnrollStudentRequest request) {
            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand()) {
                command.Connection = connection;
                connection.Open();
                var transaction = connection.BeginTransaction();
                command.Transaction = transaction;

                //Check if studies exist
                command.CommandText = "select IdStudy from Studies where name=@name";
                command.Parameters.AddWithValue("name", request.Studies);

                var dr = command.ExecuteReader();
                if (!dr.Read()) {
                    dr.Close();
                    transaction.Rollback();
                    return "Studia nie istnieja";
                }
                int idStudies = (int) dr["IdStudy"];

                //Check is student's id is unique
                command.CommandText = "select IndexNumber from Student where IndexNumber=@IndexNumber";
                command.Parameters.AddWithValue("IndexNumber", request.IndexNumber);

                dr.Close();
                dr = command.ExecuteReader();
                if (dr.Read()) {
                    dr.Close();
                    transaction.Rollback();
                    return "Student o takim indeksie juz istnieje";
                }

                //Check if enrollment exists
                int IdEnrollment;
                command.CommandText = "select * from Enrollment where IdStudy=@IdStudies and Semester = 1";
                command.Parameters.AddWithValue("IdStudies", idStudies);

                dr.Close();
                dr = command.ExecuteReader();
                if (!dr.Read()) {
                    //Get new max IdEnrollment
                    command.CommandText = "select max(IdEnrollment) from Enrollment";
                    var reader = command.ExecuteReader();
                    IdEnrollment = (int) reader["IdEnrollment"];
                    IdEnrollment++;

                    //Create new enrollment
                    command.CommandText = "Insert into Enrollment(IdEnrollment, Semester, IdStudy, StartDate) values(@IdEnrollment, 1, @IdStudies, GETDATE())";
                    command.Parameters.AddWithValue("IdEnrollment", IdEnrollment);
                    command.Parameters.AddWithValue("IdStudies", idStudies);
                    command.ExecuteReader();
                }
                IdEnrollment = (int) dr["IdEnrollment"];
                command.CommandText = "Insert into Student(IndexNumber, FirstName, LastName, BirthDate, IdEnrollment) values(@IndexNumber, @FirstName, @Lastname, @BirthDate, @IdEnrollment)";
                command.Parameters.Clear();
                command.Parameters.AddWithValue("IndexNumber", request.IndexNumber);
                command.Parameters.AddWithValue("FirstName", request.FirstName);
                command.Parameters.AddWithValue("LastName", request.LastName);
                command.Parameters.AddWithValue("BirthDate", request.BirthDate);
                command.Parameters.AddWithValue("IdEnrollment", IdEnrollment);

                dr.Close();
                command.ExecuteNonQuery();
                dr.Close();

                transaction.Commit();
                dr.Close();
                return "Student dodany do bazy danych";
            }
        }

        public List<Enrollment> promoteStudents(PromoteStudentsRequest request) {
            List<Enrollment> list = new List<Enrollment>();
            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand()) {
                command.Connection = connection;
                connection.Open();
                var transaction = connection.BeginTransaction();
                command.Transaction = transaction;

                //execute procedure
                command.CommandText = "exec PromoteStudents @StudiesName, @Semester";
                command.Parameters.AddWithValue("StudiesName", request.Studies);
                command.Parameters.AddWithValue("Semester", request.Semester);

                //update enrollment
                command.ExecuteNonQuery();
                //get new enrollment
                command.CommandText = "Select * from Enrollment";
                var dr = command.ExecuteReader();
                while (dr.Read()) {
                    Enrollment enrollment = new Enrollment();
                    enrollment.IdEnrollment = (string) dr["IdEnrollment"];
                    enrollment.Semester = (string) dr["Semester"];
                    enrollment.IdStudy = (string) dr["IdStudy"];
                    enrollment.StartDate = (string) dr["StartDate"];
                    list.Add(enrollment);
                }
            }
            return list;
        }
    }
}
