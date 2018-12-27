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
    class DB
    {
        DataSet ds = new DataSet();
        SqlDataAdapter adapter = new SqlDataAdapter();

        string connectionString = ConfigurationManager.ConnectionStrings["Lab5Connection"].ConnectionString;


        public string[] Tables { get; private set; } = new string[] { "Burden", "Burden_type", "Employment_type", "Subject", "Teacher", "Teacher_position" };


        public void ShowTable(int numberOfTable)
        {
            const int sizeTitle = 25;
            const int sizeContent = 28;

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
                            Console.Write($"{reader.GetName(i), 20}");
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
                                Console.Write($"{reader.GetValue(i), 21}");
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


        private string GetSQLCode(int numberOfTable)
        {
            if (numberOfTable < 0 || numberOfTable >= Tables.Length)
            {
                throw new Exception("Incorrect number of table!");
            }
            return "Select * From " + Tables[numberOfTable];
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
                            newRow = EditBurden(row, ds.Tables[0].NewRow());
                            if (newRow == null)
                            {
                                connection.Close();
                                return false;
                            }
                            break;
                        }
                    case 1:
                        {
                            newRow = EditBurdenType(row, ds.Tables[0].NewRow());
                            if (newRow == null)
                            {
                                connection.Close();
                                return false;
                            }

                            break;
                        }
                    case 2:
                        {
                            newRow = EditEmploymentType(row, ds.Tables[0].NewRow());
                            if (newRow == null)
                            {
                                connection.Close();
                                return false;
                            }
                            break;
                        }
                    case 3:
                        {
                            newRow = EditSubject(row, ds.Tables[0].NewRow());
                            if (newRow == null)
                            {
                                connection.Close();
                                return false;
                            }
                            break;
                        }
                    case 4:
                        {
                            newRow = EditTeacher(row, ds.Tables[0].NewRow());
                            if (newRow == null)
                            {
                                connection.Close();
                                return false;
                            }
                            break;
                        }
                    case 5:
                        {
                            newRow = EditTeacherPosition(row, ds.Tables[0].NewRow());
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

                            if (EditBurden(row, editRow) == null)
                            {
                                connection.Close();
                                return false;
                            }
                            break;
                        }
                    case 1:
                        {
                            if (EditBurdenType(row, editRow) == null)
                            {
                                connection.Close();
                                return false;
                            }

                            break;
                        }
                    case 2:
                        {

                            if (EditEmploymentType(row, editRow) == null)
                            {
                                connection.Close();
                                return false;
                            }
                            break;
                        }
                    case 3:
                        {
                            if (EditSubject(row, editRow) == null)
                            {
                                connection.Close();
                                return false;
                            }
                            break;
                        }
                    case 4:
                        {
                            if (EditTeacher(row, editRow) == null)
                            {
                                connection.Close();
                                return false;
                            }
                            break;
                        }
                    case 5:
                        {
                            if (EditTeacherPosition(row, editRow) == null)
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


        private DataRow EditBurden(string row, DataRow newRow)
        {
            string[] rowData = row.Split(',');

            int idBurden;
            int numberOfHours;
            int fkIDTeacher;
            int fkIDSubject;
            int fkIDBurdenType;

            if (rowData.Length < 5)
            {
                return null;
            }

            if (!int.TryParse(rowData[0].Replace(" ", string.Empty), out idBurden))
            {
                return null;
            }
            if (!int.TryParse(rowData[1].Replace(" ", string.Empty), out numberOfHours))
            {
                return null;
            }
            if (!int.TryParse(rowData[2].Replace(" ", string.Empty), out fkIDTeacher))
            {
                return null;
            }
            if (!int.TryParse(rowData[3].Replace(" ", string.Empty), out fkIDSubject))
            {
                return null;
            }
            if (!int.TryParse(rowData[4].Replace(" ", string.Empty), out fkIDBurdenType))
            {
                return null;
            }

            newRow["id_burden"] = idBurden;
            newRow["number_of_hours"] = numberOfHours;
            newRow["fk_id_teacher"] = fkIDTeacher;
            newRow["fk_id_subject"] = fkIDSubject;
            newRow["fk_id_burden_type"] = fkIDBurdenType;

            return newRow;

        }


        private DataRow EditBurdenType(string row, DataRow newRow)
        {
            string[] rowData = row.Split(',');

            int idBurdenType;
            string burdenTypeName;

            if (rowData.Length < 2)
            {
                return null;
            }

            if (!int.TryParse(rowData[0].Replace(" ", string.Empty), out idBurdenType))
            {
                return null;
            }

            burdenTypeName = rowData[1];
            
            newRow["id_burden_type"] = idBurdenType;
            newRow["burden_type"] = burdenTypeName;

            return newRow;
        }


        private DataRow EditEmploymentType(string row, DataRow newRow)
        {
            string[] rowData = row.Split(',');

            int idEmploymentType;
            string employmentTypeName;

            if (rowData.Length < 2)
            {
                return null;
            }

            if (!int.TryParse(rowData[0].Replace(" ", string.Empty), out idEmploymentType))
            {
                return null;
            }

            employmentTypeName = rowData[1];

            newRow["id_employment_type"] = idEmploymentType;
            newRow["employment_type_name"] = employmentTypeName;

            return newRow;
        }


        private DataRow EditSubject(string row, DataRow newRow)
        {
            string[] rowData = row.Split(',');

            int idSubject;
            string subjectName;
            int numberOfHours;

            if (rowData.Length < 3)
            {
                return null;
            }
            
            if (!int.TryParse(rowData[0].Replace(" ", string.Empty), out idSubject))
            {
                return null;
            }

            subjectName = rowData[1];

            if (!int.TryParse(rowData[2].Replace(" ", string.Empty), out numberOfHours))
            {
                return null;
            }

            newRow["id_subject"] = idSubject;
            newRow["subject_name"] = subjectName;
            newRow["number_of_hours"] = numberOfHours;

            return newRow;
        }


        private DataRow EditTeacher(string row, DataRow newRow)
        {
            string[] rowData = row.Split(',');

            int idTeacher;
            string firstName;
            string lastName;
            string adress;
            string phoneNumber;
            int numberOfHours;
            int fkIDPosition;
            int fkIDEmploymentType;

            if (rowData.Length < 8)
            {
                return null;
            }

            if (!int.TryParse(rowData[0].Replace(" ", string.Empty), out idTeacher))
            {
                return null;
            }

            firstName = rowData[1];
            lastName = rowData[2];
            adress = rowData[3];
            phoneNumber = rowData[4];


            if (!int.TryParse(rowData[5].Replace(" ", string.Empty), out numberOfHours))
            {
                return null;
            }
            if (!int.TryParse(rowData[6].Replace(" ", string.Empty), out fkIDPosition))
            {
                return null;
            }
            if (!int.TryParse(rowData[7].Replace(" ", string.Empty), out fkIDEmploymentType))
            {
                return null;
            }

            newRow["id_teacher"] = idTeacher;
            newRow["firstname"] = firstName;
            newRow["lastname"] = lastName;
            newRow["adress"] = adress;
            newRow["phone_number"] = phoneNumber;
            newRow["number_of_hours"] = numberOfHours;
            newRow["fk_id_position"] = fkIDPosition;
            newRow["fk_id_employment_type"] = fkIDEmploymentType;

            return newRow;
        }


        private DataRow EditTeacherPosition(string row, DataRow newRow)
        {
            string[] rowData = row.Split(',');

            int idPosition;
            string positionName;

            if (rowData.Length < 2)
            {
                return null;
            }

            if (!int.TryParse(rowData[0].Replace(" ", string.Empty), out idPosition))
            {
                return null;
            }

            positionName = rowData[1];

            newRow["id_position"] = idPosition;
            newRow["position_name"] = positionName;

            return newRow;
        }

    }
}
