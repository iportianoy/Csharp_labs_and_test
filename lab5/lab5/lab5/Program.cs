//Особисто запропонований варіант
//Кафедра забезпечує викладання певних дисциплін.
//Для кожної дисципліни передбачені певні години 
//педагогічного навантаження по видах: лекції, практичні 
//заняття, лабораторні роботи, практика(це також вид дисципліни), 
//дипломне проектування, екзамен. Має значення також посада викладача: 
//певні види навантаження(як-от практичні та лабораторні заняття) може 
//виконувати асистент, інші – лише старший викладач, доцент або професор.
//На кафедрі існують викладачі, як штатні, так і сумісники.
//Кожний викладач викладає кілька дисциплін(в середньому біля чотирьох). 

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab5
{
    class Program
    {
       
        static DB db = new DB();
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
                Console.WriteLine($"{iter + 2}. Exit");

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
                else if (operation < 1 || operation > db.Tables.Length + 1)
                {
                    flag = true;
                    Console.WriteLine();
                    Console.WriteLine($"Number of operation must be between 1 and {db.Tables.Length + 1}");
                    Console.WriteLine();
                }
                else if (operation == iter + 2)
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
