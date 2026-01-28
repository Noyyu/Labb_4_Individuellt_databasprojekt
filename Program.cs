using Microsoft.Data.SqlClient;
using SQLScaffolding;

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
                Console.WriteLine("0: Exit");

                switch (UserNumberChoise(0, 3))
                {
                    case 1:
                        Console.Clear();
                        Console.WriteLine("1: Sort by last name");
                        Console.WriteLine("2: Sort by first name");
                        bool userBoolOne = UserNumberChoise(1, 2) == 2;
                        Console.Clear();

                        Console.WriteLine("1: Sort by decending order");
                        Console.WriteLine("2: Sort by acanding order");
                        bool userBoolTwo = UserNumberChoise(1, 2) == 1;
                        Console.Clear();

                        DbManager.PrintStudents(userBoolTwo, userBoolOne);
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
                        Console.WriteLine("Staff name?");
                        name = Console.ReadLine();
                        Console.Clear();

                        Console.WriteLine("Staff title?");
                        Console.WriteLine("1: Admin");
                        Console.WriteLine("2: Rektor");
                        Console.WriteLine("3: Lärare");
                        Console.WriteLine("4: IT");
                        Console.WriteLine("5: Vaktmästare");
                        Console.WriteLine();

                        DbManager.AddStaff(name, UserNumberChoise(1, 5));
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

