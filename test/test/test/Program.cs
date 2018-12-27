//Міста.В БД зберігається інформація про міста и їх мешканців.
//Для міст необхідно зберігати:
//-	назву;
//-	рік заснування;
//-	площу;
//-	кількість населення для кожного типу мешканців.
//Для типів мешканців необхідно зберігати:
//-	місто проживання;
//-	назву;
//-	мову спілкування.
//Завдання:
//-	Вивести інформацію про всіх мешканців заданого міста, котрі розмовляють заданою мовою.
//-	Вивести інформацію про всі міста, в яких проживають мешканці заданого типу.
//-	Вивести інформацію про місто з заданою кількістю мешканців та всі типи мешканців, котрі в ньому проживають.
//-	Вивести інформацію про найбільш старий тип мешканців.
// Портяний Іван ІС-63


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test
{
    class Program
    {
        static TestDB db = new TestDB();
        static void Main(string[] args)
        {
            MainMenu();
            Console.WriteLine("Press any key...");
            Console.Read();
        }


        private static void MainMenu()
        {
            Console.Clear();

            bool flag = true;
            while (flag)
            {
                Console.WriteLine("============ Tables ============");

                int iter = 0;

                for (int i = 0; i < db.Tables.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {db.Tables[i]}");
                    iter = i;
                }
                Console.WriteLine($"{iter + 2}. Show info about citizen who live in Kyiv and speak ukrainian");
                Console.WriteLine($"{iter + 3}. Show info about city where live adult people");
                Console.WriteLine($"{iter + 4}. Exit");

                int operation;
                Console.WriteLine("Select operation:");
                string input = Console.ReadLine();

                if (!int.TryParse(input, out operation))
                {
                    flag = true;
                    Console.WriteLine();
                    Console.WriteLine("Incorrect operation!");
                    Console.WriteLine();
                }
                else if (operation < 1 || operation > db.Tables.Length + 3)
                {
                    flag = true;
                    Console.WriteLine();
                    Console.WriteLine($"Number of operation must be between 1 and {db.Tables.Length + 1}");
                    Console.WriteLine();
                }
                else if (operation == iter + 2)
                {
                    Console.WriteLine($"==================== People form city 1 and who speak urainian =======================");

              
                    db.GetSQLAboutCitizenFromCity();
                    Console.WriteLine("Press any key...");
                    Console.Read();
                    Console.Clear();
                }
                else if (operation == iter + 3)
                {
                    Console.WriteLine($"==================== Info about city where live adult people =======================");

                    db.GetSQLAboutCityForCitizenType();
                    Console.WriteLine("Press any key...");
                    Console.Read();
                    Console.Clear();
                }
                else if (operation == iter + 3)
                {
                    System.Environment.Exit(1);
                }
                else
                {
                    TableMenu(operation - 1);
                    flag = true;
                }
            }

        }

        private static void TableMenu(int numberOfTable)
        {
            Console.Clear();
            bool flag = true;
            db.ShowTable(numberOfTable);

            while (flag)
            {
                Console.WriteLine("============ Table Menu ============");
                Console.WriteLine("1. Add new row");
                Console.WriteLine("2. Delete row");
                Console.WriteLine("3. Edit row");
                Console.WriteLine("4. Exit");
                Console.WriteLine("Select operation:");

                int operation;
                string input = Console.ReadLine();

                if (!int.TryParse(input, out operation))
                {
                    flag = true;
                    Console.WriteLine();
                    Console.WriteLine("Incorrect operation!");
                    Console.WriteLine();

                }
                else
                {
                    switch (operation)
                    {
                        case 1:
                            {
                                flag = false;
                                Add(numberOfTable);
                                break;
                            }
                        case 2:
                            {
                                flag = false;
                                Delete(numberOfTable);
                                break;
                            }
                        case 3:
                            {
                                flag = false;
                                Edit(numberOfTable);
                                break;
                            }
                        case 4:
                            {
                                flag = false;
                                MainMenu();
                                break;
                            }
                        default:
                            {
                                flag = true;
                                Console.WriteLine();
                                Console.WriteLine("Number of operation must be between 1 and 4");
                                Console.WriteLine();
                                break;
                            }
                    }
                }

            }

        }

        private static void Add(int numberOfTable)
        {
            bool flag = true;
            while (flag)
            {
                Console.WriteLine("Enter values for each column and separate with ',':");

                string input = Console.ReadLine();

                if (!db.Add(numberOfTable, input))
                {
                    flag = true;
                    Console.WriteLine();
                    Console.WriteLine("Incorrect data!");
                    Console.WriteLine();
                }
                else
                    flag = false;

            }
        }

        private static void Delete(int numberOfTable)
        {
            bool flag = true;
            while (flag)
            {
                Console.WriteLine("Select number of row:");

                int rowNumber;
                string input = Console.ReadLine();

                if (!int.TryParse(input, out rowNumber))
                {
                    flag = true;
                    Console.WriteLine();
                    Console.WriteLine("Incorrect number!");
                    Console.WriteLine();

                }
                else if (!db.Delete(numberOfTable, rowNumber))
                {
                    flag = true;
                    Console.WriteLine();
                    Console.WriteLine("Input number must be between 1 and number of latest row");
                    Console.WriteLine();
                }
                else
                {
                    flag = false;
                }

            }
        }

        private static void Edit(int numberOfTable)
        {
            bool flag = true;
            while (flag)
            {
                Console.WriteLine("Select number of row for editing:");

                int rowNum;
                string input1 = Console.ReadLine();

                Console.WriteLine("Enter new values for each column and separate with ',':");

                string input2 = Console.ReadLine();


                if (!int.TryParse(input1, out rowNum))
                {
                    flag = true;
                    Console.WriteLine();
                    Console.WriteLine("Incorrect number!");
                    Console.WriteLine();

                }
                else if (!db.Edit(numberOfTable, rowNum, input2))
                {
                    flag = true;
                    Console.WriteLine();
                    Console.WriteLine("Incorrect data!");
                    Console.WriteLine();
                }
                else
                {
                    flag = false;
                }

            }
        }
    }
}

