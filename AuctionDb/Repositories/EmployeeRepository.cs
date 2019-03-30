using AuctionDb.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionDb.Repositories
{
    public class EmployeeRepository:IRepository<Employee>
    {
        string connectionString = ConfigurationManager.ConnectionStrings["AuctionDbConnection"].ConnectionString;
        string employeesTable = $"[dbo].[Employees]";
        DataSet auctionDb = new DataSet();


        public void Add(Employee entity)
        {
            auctionDb.Clear();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string selectSql = $"select * from {employeesTable} where [EmployeeId]='{entity.Id}'";
                using (SqlDataAdapter adapter = new SqlDataAdapter(selectSql, connection))
                {
                    adapter.Fill(auctionDb);
                    SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

                    if (auctionDb.Tables[0].Rows.Count != 0)
                        throw new Exception($"Already has an employee with id = {entity.Id}");

                    auctionDb.Clear();
                    string insertSql = $"select * from {employeesTable}";
                    adapter.SelectCommand = new SqlCommand(insertSql, connection);
                    commandBuilder = new SqlCommandBuilder(adapter);

                    adapter.Fill(auctionDb);
                    DataTable table = auctionDb.Tables[0];
                    DataRow newRow = table.NewRow();
                    newRow["EmployeeId"] = entity.Id;
                    newRow["FirstName"] = entity.FirstName;
                    newRow["LastName"] = entity.LastName;
                    newRow["Email"] = entity.Email;
                    newRow["PasswordHash"] = entity.Password;
                    newRow["DoB"] = entity.DoB.ToString("yyyy-MM-dd");
                    newRow["OrganizationId"] = entity.OrganizationId;
                    auctionDb.Tables[0].Rows.Add(newRow);
                    adapter.Update(auctionDb);
                }
            }
        }

        public void Delete(string id)
        {
            auctionDb.Clear();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string selectSqlById = $"select * from {employeesTable} where [EmployeeId]='{id}'";
                using (SqlDataAdapter adapter = new SqlDataAdapter(selectSqlById, connection))
                {
                    adapter.Fill(auctionDb);
                    SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

                    if (auctionDb.Tables[0].Rows.Count == 0)
                        throw new Exception($"There is no employee with id = {id}");

                    string deleteSql = $"delete from {employeesTable} where [EmployeeId]='{id}'";
                    SqlCommand command = new SqlCommand(deleteSql, connection);
                    command.ExecuteNonQuery();
                }
            }
        }

        public Employee Read(string id)
        {
            auctionDb.Clear();
            Employee employee = new Employee();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string selectSqlById = $"select * from {employeesTable} where [EmployeeId]='{id}'";
                using (SqlDataAdapter adapter = new SqlDataAdapter(selectSqlById, connection))
                {
                    adapter.Fill(auctionDb);
                    SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

                    if (auctionDb.Tables[0].Rows.Count == 0)
                        throw new Exception($"There is no employee with id = {id}");

                    DataTable table = auctionDb.Tables[0];
                    employee.Id = table.Rows[0]["EmployeeId"].ToString();
                    employee.FirstName= table.Rows[0]["FirstName"].ToString();
                    employee.LastName = table.Rows[0]["LastName"].ToString();
                    employee.Email = table.Rows[0]["Email"].ToString();
                    employee.Password = table.Rows[0]["PasswordHash"].ToString();
                    employee.DoB = Convert.ToDateTime(table.Rows[0]["DoB"].ToString());
                    employee.OrganizationId = table.Rows[0]["OrganizationId"].ToString();
                }
            }
            return employee;
        }

        public IEnumerable<Employee> ReadAll()
        {
            List<Employee> employees = new List<Employee>();
            auctionDb.Clear();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string selectSqlAll = $"select * from {employeesTable}";
                using (SqlDataAdapter adapter = new SqlDataAdapter(selectSqlAll, connection))
                {
                    adapter.Fill(auctionDb);
                    SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

                    if (auctionDb.Tables[0].Rows.Count == 0)
                        throw new Exception($"There are no employees in database");

                    DataTable table = auctionDb.Tables[0];
                    foreach (DataRow item in table.Rows)
                    {
                        Employee employee = new Employee()
                        {
                            Id = item["EmployeeId"].ToString(),
                            FirstName=item["FirstName"].ToString(),
                            LastName = item["LastName"].ToString(),
                            Email = item["Email"].ToString(),
                            Password = item["PasswordHash"].ToString(),
                            DoB = Convert.ToDateTime(item["DoB"].ToString()),
                            OrganizationId = item["OrganizationId"].ToString()
                        };
                        employees.Add(employee);
                    }
                }
            }

            return employees;
        }

        public void Update(string id, Employee updated)
        {
            auctionDb.Clear();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string selectSqlById = $"select * from {employeesTable} where [EmployeeId]='{id}'";
                using (SqlDataAdapter adapter = new SqlDataAdapter(selectSqlById, connection))
                {
                    adapter.Fill(auctionDb);
                    SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

                    if (auctionDb.Tables[0].Rows.Count == 0)
                        throw new Exception($"There is no employee with id = {id}");

                    DataTable table = auctionDb.Tables[0];
                    table.Rows[0]["EmployeeId"] = updated.Id;
                    table.Rows[0]["FirstName"] = updated.FirstName;
                    table.Rows[0]["LastName"] = updated.LastName;
                    table.Rows[0]["Email"] = updated.Email;
                    table.Rows[0]["PasswordHash"] = updated.Password;
                    table.Rows[0]["DoB"] = updated.DoB.ToString("yyyy-MM-dd");
                    table.Rows[0]["OrganizationId"] = updated.OrganizationId;

                    adapter.Update(auctionDb);
                }
            }
        }
    }
}
