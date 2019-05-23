using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppEF6
{
    [Table("Student")]
    public class Student
    {
        public virtual int Id { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual DateTime DateOfBirth { get; set; }
        public virtual int ClassId { get; set; }
    }

    public class SchoolDBContext : DbContext
    {
        private const string ConnectionString =
            @"Data Source=.;User ID=sa;Password=P@$$w0rd;Initial Catalog=SchoolDB;";

        public SchoolDBContext() : base(ConnectionString)
        { }

        public virtual DbSet<Student> Students { get; set; }
    }

    class Program
    {
        static void GetData(int? id = null, string firstName = null)
        {
            List<Student> students;
            using (var db = new SchoolDBContext())
            {
                IQueryable<Student> query = db.Students;

                if(id != null)
                {
                    query = query.Where(s => s.Id == id);
                }
                if (firstName != null)
                {
                    query = query.Where(s => s.FirstName == firstName);
                }

                var orderedQuery = query.OrderBy(s => s.FirstName)
                                        .ThenBy(s => s.LastName);

                students = orderedQuery.ToList();
            }

            foreach (var student in students)
            {
                Console.WriteLine("Student Details:" +
                                  $"Id: {student.Id}" +
                                  $"Name: {student.LastName}, {student.FirstName}",
                                  $"Date of Birth: {student.DateOfBirth}");
            }

        }

        static void Main(string[] args)
        {
            GetData();
            GetData(10);
            GetData(firstName: "Kevin");
        }
    }
}
