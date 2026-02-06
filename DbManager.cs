using Labb_3__Anropa_databasen.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Labb_3__Anropa_databasen
{
    internal class DbManager
    {
        public static void AddStaff(string name, int titleId)
        {
            using (var context = new SchoolContext())
            {
                var newStaff = new Staff { Name = name, TitleId = titleId };
                context.Staff.Add(newStaff);
                context.SaveChanges();
                Console.WriteLine($"{newStaff.Name} has been added to the Staff Table");
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to continue..");
            Console.ReadKey();
        }

        public static void PrintCourses()
        {
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
            }
        }
        public static void PrintStaffInDepartment()
        {
            using (var context = new SchoolContext())
            {

                for (int i = 1; i <= context.StaffTitles.Count(); i++)
                {
                    var staffData = context.Staff.
                        Where(s => s.TitleId == i).
                        OrderBy(s => s.Name).
                        Select(s => new
                        {
                            FirstName = s.Name,
                            LastName = s.LastName,
                            Title = s.Title.Title
                        })
                         .ToList();

                    Console.WriteLine();
                    Console.WriteLine("STAFF: " + staffData.Count);
                    Console.WriteLine($"{"NAME",-15} {"LAST NAME",-15} {"DEPARTMENT",-10}");
                    Console.WriteLine(new string('-', 50));

                    foreach (var staff in staffData)
                    {
                        Console.WriteLine($"{staff.FirstName,-15} {staff.LastName,-15} - {staff.Title,-10}");
                    }
                }

                Console.WriteLine();
                Console.WriteLine("Press any key to continue..");
                Console.ReadKey();
            }
        }


        public static void PrintStudentsInClasses()
        {
            using (var context = new SchoolContext())
            {
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
