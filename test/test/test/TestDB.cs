using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test
{
    class TestDB
    {
        DataSet ds = new DataSet();
        SqlDataAdapter adapter = new SqlDataAdapter();

        string connectionString = ConfigurationManager.ConnectionStrings["TestConnection"].ConnectionString;


        public string[] Tables { get; private set; } = new string[] { "City", "citizen", "citizen_type", "number_of_population"};


        public void ShowTable(int numberOfTable)
        {
            const int sizeTitle = 15;
            const int sizeContent = 18;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(GetSQLCode(numberOfTable), connection);
                SqlDataReader reader = command.ExecuteReader();
                Console.WriteLine($"=============================== {Tables[numberOfTable]} ===============================");
                if (reader.HasRows)
                {
                    Console.Write("Col number\t");
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        if (numberOfTable == 4)
                        {
                            Console.Write($"{reader.GetName(i),20}");
                        }
                        else
                            Console.Write($"{reader.GetName(i),sizeTitle}");
                    }
                    Console.WriteLine();

                    int index = 0;
                    while (reader.Read())
                    {
                        index++;
                        Console.Write(index + "\t");
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            if (numberOfTable == 4)
                            {
                                Console.Write($"{reader.GetValue(i),21}");
                            }
                            else
                                Console.Write($"{reader.GetValue(i),sizeContent}");
                        }
                        Console.WriteLine();
                    }
                }
                else
                {
                    Console.WriteLine("Empty table");
                }
            }
        }

        public void ShowSQLResult(string command_str)
        {
            const int sizeTitle = 15;
            const int sizeContent = 18;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(command_str, connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    int index = 0;
                    while (reader.Read())
                    {
                        index++;
                        Console.Write(index + "\t");
                        for (int i = 0; i < reader.FieldCount; i++)
                        {

                            Console.Write($"{reader.GetValue(i),21}");

                        }
                        Console.WriteLine();
                    }
                }
                else
                {
                    Console.WriteLine("Empty table");
                }
            }
        }

        private string GetSQLCode(int numberOfTable)
        {
            if (numberOfTable < 0 || numberOfTable >= Tables.Length + 2)
            {
                throw new Exception("Incorrect number of table!");
            }
            return "Select * From " + Tables[numberOfTable];
        }

        public void GetSQLAboutCitizenFromCity()
        {
            string command_str = "Select * From citizen Where fk_id_city = 1 and language = 'ukrainian'";
            ShowSQLResult(command_str);

        }

        public void GetSQLAboutCityForCitizenType()
        {
            string command_str = "Select c.name, c.foundation_year, square From City c " +
                "INNER JOIN number_of_population n ON c.id_city = n.fk_id_city " +
                "WHERE n.fk_id_citizen_type = 2";
            ShowSQLResult(command_str);
        }


        public bool Add(int numberOfTable, string row)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                adapter.SelectCommand = new SqlCommand(GetSQLCode(numberOfTable), connection);
                adapter.Fill(ds);

                DataRow newRow;

                switch (numberOfTable)
                {
                    case 0:
                        {
                            newRow = EditCity(row, ds.Tables[0].NewRow());
                            if (newRow == null)
                            {
                                connection.Close();
                                return false;
                            }
                            break;
                        }
                    case 1:
                        {
                            newRow = EditCitizen(row, ds.Tables[0].NewRow());
                            if (newRow == null)
                            {
                                connection.Close();
                                return false;
                            }

                            break;
                        }
                    case 2:
                        {
                            newRow = EditCitizenType(row, ds.Tables[0].NewRow());
                            if (newRow == null)
                            {
                                connection.Close();
                                return false;
                            }
                            break;
                        }
                    case 3:
                        {
                            newRow = EditNumberOfPopulation(row, ds.Tables[0].NewRow());
                            if (newRow == null)
                            {
                                connection.Close();
                                return false;
                            }
                            break;
                        }
                    
                    default:
                        {
                            connection.Close();
                            return false;

                        }
                }


                ds.Tables[0].Rows.Add(newRow);
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);
                adapter.Update(ds);

            }
            return true;
        }


        public bool Delete(int numberOfTable, int rowIndex)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                adapter.SelectCommand = new SqlCommand(GetSQLCode(numberOfTable), connection);
                adapter.Fill(ds);

                DataTable dt = ds.Tables[0];

                if (dt.Rows.Count < rowIndex)
                {
                    connection.Close();

                    if (dt.Rows.Count == 0)
                    {
                        return true;
                    }

                    return false;
                }

                dt.Rows[rowIndex - 1].Delete();

                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);
                adapter.Update(ds);
            }


            return true;
        }


        public bool Edit(int numberOfTable, int rowIndex, string row)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                adapter.SelectCommand = new SqlCommand(GetSQLCode(numberOfTable), connection);
                adapter.Fill(ds);

                if (rowIndex > ds.Tables[0].Rows.Count)
                {
                    connection.Close();

                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        return true;
                    }
                    return false;
                }

                DataRow editRow = ds.Tables[0].Rows[rowIndex - 1];

                switch (numberOfTable)
                {
                    case 0:
                        {

                            if (EditCity(row, editRow) == null)
                            {
                                connection.Close();
                                return false;
                            }
                            break;
                        }
                    case 1:
                        {
                            if (EditCitizen(row, editRow) == null)
                            {
                                connection.Close();
                                return false;
                            }

                            break;
                        }
                    case 2:
                        {

                            if (EditCitizenType(row, editRow) == null)
                            {
                                connection.Close();
                                return false;
                            }
                            break;
                        }
                    case 3:
                        {
                            if (EditNumberOfPopulation(row, editRow) == null)
                            {
                                connection.Close();
                                return false;
                            }
                            break;
                        }
                    
                    default:
                        {
                            connection.Close();
                            return false;

                        }
                }

                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);
                adapter.Update(ds);

            }
            return true;
        }


        private DataRow EditCity(string row, DataRow newRow)
        {
            string[] rowData = row.Split(',');

            int id_city;
            string name;
            string foundation_year;
            float square;

            if (rowData.Length < 4)
            {
                return null;
            }

            if (!int.TryParse(rowData[0].Replace(" ", string.Empty), out id_city))
            {
                return null;
            }
            name = rowData[1];
            foundation_year = rowData[2];
            if (!float.TryParse(rowData[3].Replace(" ", string.Empty), out square))
            {
                return null;
            }
            newRow["id_city"] = id_city;
            newRow["name"] = name;
            newRow["fk_id_city"] = foundation_year;
            newRow["fk_id_citizen_type"] = square;

            return newRow;

        }


        private DataRow EditCitizen(string row, DataRow newRow)
        {
            string[] rowData = row.Split(',');

            int id_citizen;
            string name;
            int fk_id_city;
            int fk_id_citizen_type;
            string language;

            if (rowData.Length < 4)
            {
                return null;
            }

            if (!int.TryParse(rowData[0].Replace(" ", string.Empty), out id_citizen))
            {
                return null;
            }
            name = rowData[1];
            if (!int.TryParse(rowData[2].Replace(" ", string.Empty), out fk_id_city))
            {
                return null;
            }
            if (!int.TryParse(rowData[3].Replace(" ", string.Empty), out fk_id_citizen_type))
            {
                return null;
            }
            language = rowData[4];

            newRow["id_citizen"] = id_citizen;
            newRow["name"] = name;
            newRow["fk_id_city"] = fk_id_city;
            newRow["fk_id_citizen_type"] = fk_id_citizen_type;
            newRow["language"] = language;


            return newRow;
        }


        private DataRow EditCitizenType(string row, DataRow newRow)
        {
            string[] rowData = row.Split(',');

            int id_citizen_type;
            string name;

            if (rowData.Length < 2)
            {
                return null;
            }

            if (!int.TryParse(rowData[0].Replace(" ", string.Empty), out id_citizen_type))
            {
                return null;
            }

            name = rowData[1];

            newRow["id_citizen_type"] = id_citizen_type;
            newRow["name"] = name;

            return newRow;
        }


        private DataRow EditNumberOfPopulation(string row, DataRow newRow)
        {
            string[] rowData = row.Split(',');

            int id;
            int fk_id_city;
            int fk_id_citizen_type;
            int quantity;

            if (rowData.Length < 4)
            {
                return null;
            }

            if (!int.TryParse(rowData[0].Replace(" ", string.Empty), out id))
            {
                return null;
            }

            if (!int.TryParse(rowData[1].Replace(" ", string.Empty), out fk_id_city))
            {
                return null;
            }

            if (!int.TryParse(rowData[2].Replace(" ", string.Empty), out fk_id_citizen_type))
            {
                return null;
            }

            if (!int.TryParse(rowData[3].Replace(" ", string.Empty), out quantity))
            {
                return null;
            }
            

            newRow["id"] = id;
            newRow["fk_id_city"] = fk_id_city;
            newRow["fk_id_citizen_type"] = fk_id_citizen_type;
            newRow["quantity"] = quantity;

            return newRow;
        }


    
    }
}
