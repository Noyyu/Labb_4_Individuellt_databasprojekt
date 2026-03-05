using Labb_3__Anropa_databasen.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Labb_3__Anropa_databasen
{

    internal class DbManager
    {
        private static readonly string _connectionString = @"Server = localhost;Database = School;Integrated Security = true;Trust Server Certificate = true;";

        public static void AddStaff(string name, string lastName, int titleId, float salary)
        {
            using (var context = new SchoolContext())
            {
                var newStaff = new Staff
                {
                    Name = name,
                    LastName = lastName,
                    TitleId = titleId,
                    Salary = (decimal)salary
                };

                context.Staff.Add(newStaff);
                context.SaveChanges();
                Console.WriteLine($"{newStaff.Name} has been added to the Staff Table");
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to continue..");
            Console.ReadKey();
            Console.Clear();
        }
        public static void PrintStaffDetails()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string staffDetailsSql = "SELECT * FROM dbo.StaffDetails";
                SqlCommand command = new SqlCommand(staffDetailsSql, connection);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    Console.Clear();
                    Console.WriteLine(new string('-', 60));
                    Console.WriteLine($"{"NAME",-25}{"TITLE",-10}{"YEARS OF SERVICE",-10}");
                    Console.WriteLine(new string('-', 60));

                    while (reader.Read())
                    {
                        Console.WriteLine("{0,-25}{1,-10}{2,-10}",
                            reader["Name"],
                            reader["Title"],
                            reader["YearsOfService"]);
                    }
                }
                Console.WriteLine(new string('-', 60));
                Console.WriteLine();
                Console.WriteLine("Press any key to continue..");
                Console.ReadKey();
                Console.Clear();
            }
        }

        public static void GiveStudentGrade()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                int studentId = -1;
                int staffId = -1;
                int courseId = -1;
                DateTime date;
                char grade = 'O';

                string studentCountSql = "SELECT COUNT(*) FROM dbo.Students";
                string staffCountSql = "SELECT COUNT(*) FROM dbo.Staff";
                string courseCountSql = "SELECT COUNT(*) FROM dbo.Courses";

                SqlCommand getStudentCount = new SqlCommand(studentCountSql, connection);
                SqlCommand getStaffCount = new SqlCommand(staffCountSql, connection);
                SqlCommand getCourseCount = new SqlCommand(courseCountSql, connection);

                connection.Open();

                int studentCount = (int)getStudentCount.ExecuteScalar();
                int staffCount = (int)getStaffCount.ExecuteScalar();
                int courseCount = (int)getCourseCount.ExecuteScalar();

                Console.Clear();
                Console.WriteLine("STUDENT ID: 1-" + studentCount);
                while (!int.TryParse(Console.ReadLine(), out studentId) || studentId <= 0 || studentId > studentCount)
                {
                    Console.WriteLine("Please enter a valid student ID");
                }

                Console.WriteLine();
                Console.WriteLine("STAFF ID: 1-" + staffCount);
                while (!int.TryParse(Console.ReadLine(), out staffId) || staffId <= 0 || staffId > staffCount)
                {
                    Console.WriteLine("Please enter a valid staff ID");
                }

                Console.WriteLine();
                Console.WriteLine("COURSE ID: 1-" + courseCount);
                while (!int.TryParse(Console.ReadLine(), out courseId) || courseId <= 0 || courseId > courseCount)
                {
                    Console.WriteLine("Please enter a valid course ID");
                }

                Console.WriteLine();
                Console.WriteLine("DATE: yyyy-mm-dd");
                while (!DateTime.TryParse(Console.ReadLine(), out date))
                {
                    Console.WriteLine("Please enter a valid date");
                }

                Console.WriteLine();
                Console.WriteLine("GRADE: A - F");
                while (!char.TryParse(Console.ReadLine(), out grade) && (char.ToUpper(grade) == 'A' || char.ToUpper(grade) == 'B' || char.ToUpper(grade) == 'C' || char.ToUpper(grade) == 'D' || char.ToUpper(grade) == 'E' || char.ToUpper(grade) == 'G'))
                {
                    Console.WriteLine("Please enter a valid grade");
                }

                try
                {
                    SqlCommand command = new SqlCommand("dbo.GiveStudentGrade", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure; //Berättar att det är en store procedure
                    command.Parameters.AddWithValue("@StudentId", studentId); //Stoppar in parametern
                    command.Parameters.AddWithValue("@CourseId", courseId);
                    command.Parameters.AddWithValue("@teacherId", staffId);
                    command.Parameters.AddWithValue("@Date", date);
                    command.Parameters.AddWithValue("@Grade", char.ToUpper(grade));
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("SUDENT HAVE RECIVED A GRADE");
                        PrintStudentInfo(studentId);
                    }
                    else
                    {
                        Console.WriteLine("The command didn't run. No data was added");
                    }
                }
                catch (SqlException ex) //Om något går åt helvete, typ om servern är nere eller något annat dumt
                {
                    Console.WriteLine("The database is dead or something");
                }

                Console.WriteLine(new string('-', 60));
                Console.WriteLine();
                Console.WriteLine("Press any key to continue..");
                Console.ReadKey();
                Console.Clear();
            }
        }
        public static bool PrintStudentInfo(int studentID)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                //Hämta antalet studenter
                string studentCountSql = "SELECT COUNT(*) FROM dbo.Students";
                SqlCommand getStudentCount = new SqlCommand(studentCountSql, connection);
                connection.Open();
                int studentCount = (int)getStudentCount.ExecuteScalar();

                if (studentID > studentCount) //Kastar ut användaren att försöka igen
                {
                    return false;
                }

                SqlCommand command = new SqlCommand("dbo.SeeStudentInfo", connection);
                command.CommandType = System.Data.CommandType.StoredProcedure; //Berättar att det är en store procedure
                command.Parameters.AddWithValue("@StudentId", studentID); //Stoppar in parametern

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    bool headerPrinted = false;

                    while (reader.Read())
                    {
                        if (!headerPrinted)
                        {
                            //INFO
                            Console.Clear();
                            Console.WriteLine($"STUDENT INFO");
                            Console.WriteLine(new string('-', 40));
                            Console.WriteLine($"Name:       {reader["Student"]}");
                            Console.WriteLine($"Birth Date: {Convert.ToDateTime(reader["BirthDate"]):yyyy-MM-dd}");
                            Console.WriteLine($"Class:      {reader["ClassName"]}");
                            Console.WriteLine(new string('-', 110));

                            // BETYG
                            Console.WriteLine($"{"COURSE",-40} {"GRADE",-10} {"TEACHER",-25} {"DATE",-15}");
                            Console.WriteLine(new string('-', 110));
                            headerPrinted = true;
                        }

                        // Om studenten har betyg
                        if (reader["Grade"].ToString() != "-") //Vi har gjort att tomma betyg är "-"
                        {
                            DateTime gradeDate = Convert.ToDateTime(reader["Date"]);
                            Console.WriteLine("{0,-40} {1,-10} {2,-25} {3,-15}",
                                reader["Course"],
                                reader["Grade"],
                                reader["Teacher"],
                                gradeDate.ToString("yyyy-MM-dd"));
                        }
                        else
                        {
                            Console.WriteLine("This student has no grades registered in the system.");
                        }
                    }
                }

                Console.WriteLine();
                Console.WriteLine("Press any key to continue..");
                Console.ReadKey();
                Console.Clear();
                return true;
            }
        }

        public static void PrintCourses()
        {
            //Lägg till vem som satte betygen och när

            using (var context = new SchoolContext())
            {

                for (int i = 1; i <= context.Courses.Count(); i++)
                {                   
                    var courseData = context.Grades.
                        Where(c => c.CourseId == i).
                        OrderBy(s => s.Grade1).
                        Select(c => new
                        {
                            StudentName = c.Student.Name,
                            StudentLastName = c.Student.LastName,
                            Grade = c.Grade1,
                            Course = c.Course.Title
                        })
                         .ToList();

                    Console.WriteLine();
                    Console.WriteLine(new string('-', 50));
                    Console.WriteLine($"{"COURSE: "}{courseData.FirstOrDefault()?.Course}");
                    Console.WriteLine($"{"STUDENTS: "}{ courseData.Count, -5}");                
                    Console.WriteLine(new string('-', 50));
                    Console.WriteLine($"{"NAME",-15} {"LAST NAME",-15} {"GRADE",-10}");
                    Console.WriteLine(new string('-', 50));

                    foreach (var student in courseData)
                    {
                        Console.WriteLine($"{student.StudentName,-15} {student.StudentLastName,-15} - {student.Grade,-10}");
                    }
                }

                Console.WriteLine();
                Console.WriteLine("Press any key to continue..");
                Console.ReadKey();
                Console.Clear();
            }
        }
        public static void PrintStaffInDepartment()
        {
            // Lägg till lön
            using (var context = new SchoolContext())
            {

                for (int i = 1; i <= context.StaffTitles.Count(); i++)
                {
                    string staffDepartmentSalarySql = "SELECT AVG(Salary) FROM Staff WHERE TitleId = @titleId";
                    decimal averageSalary = 0;
                    decimal totalDepartmentCost = 0;

                    using (SqlConnection connection = new SqlConnection(_connectionString))
                    {
                        SqlCommand command = new SqlCommand(staffDepartmentSalarySql, connection);
                        command.Parameters.AddWithValue("@titleId", i);
                        connection.Open();
                        var result = command.ExecuteScalar();
                        averageSalary = result != DBNull.Value ? Convert.ToDecimal(result) : 0; //Om den inte är null, konvertera den till en decimal, annars gör den till 0
                    }
                        var staffData = context.Staff.
                        Where(s => s.TitleId == i).
                        OrderBy(s => s.Name).
                        Select(s => new
                        {
                            FirstName = s.Name,
                            LastName = s.LastName,
                            Title = s.Title.Title,
                            Salary = s.Salary
                        })
                            .ToList();

                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine($"STAFF: {staffData.Count}   |   AVARAGE SALARY: {averageSalary:C2}" );
                    Console.WriteLine(new string('-', 60));
                    Console.WriteLine($"{"NAME",-15} {"LAST NAME",-15} {"SALARY",-10} {"DEPARTMENT",-10}");
                    Console.WriteLine(new string('-', 60));

                    foreach (var staff in staffData)
                    {
                        Console.WriteLine($"{staff.FirstName,-15} {staff.LastName,-15} {staff.Salary,-10} - {staff.Title,-10}");
                        totalDepartmentCost += staff.Salary;
                    }
                    Console.WriteLine(new string('-', 60));
                    Console.WriteLine($"TOTAL DEPARTMENT SPENDINGS: {totalDepartmentCost:C2}");
                    Console.WriteLine(new string('-', 60));
                    totalDepartmentCost = 0;
                }

                Console.WriteLine();
                Console.WriteLine("Press any key to continue..");
                Console.ReadKey();
                Console.Clear();
            }
        }


        public static void PrintStudentsInClasses()
        {
            using (var context = new SchoolContext())
            {
                Console.Clear();
                Console.WriteLine(new string('-', 50));
                Console.WriteLine($"{" ", -15} {"CLASSES AND STUDENTS"}");
                Console.WriteLine(new string('-', 50));


                for (int i = 1; i <= context.Classes.Count(); i++)
                {
                    var studentData = context.Students.
                        Where(s => s.ClassId == i).
                        OrderBy(s => s.Name).
                        Select(s => new
                        {
                            FirstName = s.Name,
                            LastName = s.LastName,
                            ClassName = s.Class.Name,
                            Age = DateTime.Now.Year - s.BirthDate.Year
                        })
                         .ToList();

                    Console.WriteLine();
                    Console.WriteLine(new string('-', 50));
                    Console.WriteLine($"{" ",-15}STUDENTS IN CLASS {studentData.FirstOrDefault().ClassName}: " + studentData.Count);
                    Console.WriteLine(new string('-', 50));
                    Console.WriteLine($"{"NAME",-15} {"LAST NAME",-15} {"AGE",-10}");
                    Console.WriteLine(new string('-', 50));

                    foreach (var Student in studentData)
                    {
                        Console.WriteLine($"{Student.FirstName,-15} {Student.LastName,-15} - {Student.Age,-10}");
                    }
                }

                Console.WriteLine();
                Console.WriteLine("Press any key to continue..");
                Console.ReadKey();
                Console.Clear();
            }
        }

        public static void PrintStudents(bool desc, bool name)
        {
            using (var context = new SchoolContext())
            {
                var query = context.Students.Include(student => student.Class).AsQueryable();

                if (name)
                {
                    query = desc ? query.OrderByDescending(sort => sort.Name)
                                 : query.OrderBy(sort => sort.Name);
                }
                else
                {
                    query = desc ? query.OrderByDescending(sort => sort.LastName)
                                 : query.OrderBy(sort => sort.LastName);
                }             

                Console.WriteLine($"{"NAME",-15} {"LAST NAME",-15} {"CLASS", -2}");
                Console.WriteLine(new string('-', 35));

                foreach ( var student in query)
                {
                    Console.WriteLine($"{student.Name, - 15} {student.LastName, - 15} - {student.Class.Name, -2}");
                }

                Console.WriteLine();
                Console.WriteLine("Press any key to continue..");
                Console.ReadKey();
            }
        }
        public static void PrintClass(int classId)
        {
            using (var context = new SchoolContext())
            {
                //Här hämtar vi klassen OCH dens studenter. Vi filterar ut alla studenter som går i classId med "WHERE" (c => c.Id == classId) 
                //var classWithStudents = context.Classes.Include(c => c.Students).FirstOrDefault(c => c.Id == classId);

                //Här hämtar vi BARA precis vad vi behöver och ignorerar tex Id och ClassId i Students Tabellen
                var studentData = context.Students.
                    Where(s => s.ClassId == classId).
                    Select(s => new
                    {
                        // Här väljer vi exakt vilka kolumner vi vill ha från SQL
                        FirstName = s.Name,
                        LastName = s.LastName,
                        ClassName = s.Class.Name // EF gör en JOIN automatiskt här!
                    })
                    .ToList();

                if (studentData.Any()) //Vi använder Any i stället för =! null efter som att en lista inte är null. 
                {
                    Console.WriteLine($"{"NAME",-15} {"LAST NAME",-15} {"CLASS",-2}");
                    Console.WriteLine(new string('-', 35));

                    foreach (var student in studentData)
                    {
                        //Vi behöver inte flitrera dem efter klass här längre efter som att vi redan gjort det när vi skapade variabeln.
                        Console.WriteLine($"{student.FirstName,-15} {student.LastName,-15} - {student.ClassName,-2}");
                    }
                }
                else { Console.WriteLine("This class does not exsist"); }
            }
            Console.WriteLine();
            Console.WriteLine("Press any key to continue..");
            Console.ReadKey();
        }
    }
}
