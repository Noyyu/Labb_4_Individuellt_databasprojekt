using Labb_3__Anropa_databasen.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb_3__Anropa_databasen
{
    internal class App
    {
        public static void Menu()
        {
            int exit = -1;
            while (exit != 0)
            {
                Console.Clear();
                Console.WriteLine("1: See students");
                Console.WriteLine("2: See classes");
                Console.WriteLine("3: Add staff");
                Console.WriteLine("4: See Staff");
                Console.WriteLine("5: See Courses");
                Console.WriteLine("0: Exit");

                switch (UserNumberChoise(0, 5))
                {
                    case 1:
                        Console.Clear();
                        DbManager.PrintStudentsInClasses();
                        break;

                    case 2: // Dynamiiiiisk
                        Console.Clear();
                        DbManager.PrintClass(ChooseClass());
                        break;

                    case 3:
                        string name = string.Empty;
                        string lastName = string.Empty;
                        float salary = 0;
                        int department = 0;

                        Console.Clear();
                        Console.WriteLine("STAFF FIRST NAME");
                        name = Console.ReadLine();

                        Console.Clear();
                        Console.WriteLine("STAFF LAST NAME");
                        lastName = Console.ReadLine();

                        Console.Clear();
                        Console.WriteLine("DEPARTMENT");
                        department = ChooseDepartment();

                        Console.Clear();
                        Console.WriteLine("SALARY");
                        while (! float.TryParse(Console.ReadLine(), out salary) || salary < 0)
                        {
                            Console.WriteLine("Please enter a valid salary");
                        }

                        Console.Clear();
                        DbManager.AddStaff(name, lastName, department, salary);
                        break;

                    case 4:
                        Console.Clear();
                        DbManager.PrintStaffInDepartment();
                        break;

                    case 5:
                        Console.Clear();
                        DbManager.PrintCourses();
                        break;

                    case 0:
                        exit = 0;
                        break;
                }
            }
        }

        public static int ChooseClass()
        {
            using (var context = new SchoolContext())
            {
                int classId = 0;
                var classData = context.Classes.Select(c => new 
                {
                    Class = c.Name
                }).ToList();

                for (int i = 1; i <= classData.Count; i++)
                {
                    Console.WriteLine($"{i, -2} :  {classData.ElementAt(i - 1).Class}");
                }
                while (!int.TryParse(Console.ReadLine(), out classId) || classId < 1 || classId > classData.Count)
                {
                    Console.WriteLine("Please enter a class Id");
                }
                Console.Clear();
                return (classId);
            }
        }
        public static int ChooseDepartment()
        {
            using (var context = new SchoolContext())
            {
                int departmentId = 0;
                var departmentData = context.StaffTitles.Select(d => new
                {
                    Title = d.Title
                }).ToList();

                for (int i = 1; i <= departmentData.Count; i++)
                {
                    Console.WriteLine($"{i} {departmentData.ElementAt(i - 1).Title}");
                }
                while (!int.TryParse(Console.ReadLine(), out departmentId) || departmentId < 1 || departmentId > departmentData.Count)
                {
                    Console.WriteLine("Please enter a department Id");
                }
                return departmentId;
            }
        }

        static int UserNumberChoise(int min, int max)
        {
            int userChoise = -1;
            while (!int.TryParse(Console.ReadLine(), out userChoise) || userChoise > max || userChoise < min)
            {
                Console.WriteLine("Invalid choice");
            }
            return userChoise;
        } 
    }
}
