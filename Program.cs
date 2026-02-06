using Microsoft.Data.SqlClient;
using Labb_3__Anropa_databasen;
using Labb_3__Anropa_databasen.Models;

namespace Labb_3___Anropa_databasen
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Menu();
        }

        static void Menu()
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

                    case 2:
                        Console.Clear();
                        Console.WriteLine("1: Class 1A");
                        Console.WriteLine("2: Class 2B");
                        Console.WriteLine("3: Class 3C");
                        Console.WriteLine("4: Class 4D");
                        Console.WriteLine("5: Class 5A");
                        Console.WriteLine();

                        DbManager.PrintClass(UserNumberChoise(1, 5));
                        break;

                    case 3:
                        string name;

                        Console.Clear();
                        Console.WriteLine("STAFF NAME");
                        name = Console.ReadLine();
                        Console.Clear();

                        Console.WriteLine("STAFF TITLE");
                        Console.WriteLine("1: Cleaner");
                        Console.WriteLine("2: Teacher");
                        Console.WriteLine("3: Admin");
                        Console.WriteLine("4: IT");
                        Console.WriteLine("5: Lecturer");
                        Console.WriteLine();

                        DbManager.AddStaff(name, UserNumberChoise(1, 5));
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
}

