using SQLScaffolding.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace SQLScaffolding
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
        public static void PrintClass(int classId)
        {
            using (var context = new SchoolContext())
            {
                var classToPrint = context.Classes.FirstOrDefault(className  => className.Id == classId);
                var studentsInClass = context.Students.ToList();
                var studentClass = context.Students.ToList();

                if (classToPrint != null)
                {
                    Console.WriteLine($"{"NAME",-15} {"LAST NAME",-15} {"CLASS",-2}");
                    Console.WriteLine(new string('-', 35));

                    foreach (var student in studentsInClass)
                    {
                        if (student.ClassId == classId)
                        {
                            Console.WriteLine($"{student.Name, -15} {student.LastName, -15} - {student.Class.Name, -2}");
                        }
                    }
                }
                else { Console.WriteLine("This class does not exsist"); }              
            }
            Console.WriteLine();
            Console.WriteLine("Press any key to continue..");
            Console.ReadKey();
        }
        public static void PrintStudents(bool desc, bool name)
        {
            using (var context = new SchoolContext())
            {
                var query = context.Students.Include(student => student.Class).AsQueryable();
                var students = query.ToList();
                var studentClass = context.Classes.ToList();

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

                foreach ( var student in students)
                {
                    Console.WriteLine($"{student.Name, - 15} {student.LastName, - 15} - {student.Class.Name, -2}");
                }

                Console.WriteLine();
                Console.WriteLine("Press any key to continue..");
                Console.ReadKey();
            }
        }
    }
}
