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
    public class OrganizationRepository : IRepository<Organization>
    {
        string connectionString = ConfigurationManager.ConnectionStrings["AuctionDbConnection"].ConnectionString;
        string organizationsTable = $"[dbo].[Organizations]";
        DataSet auctionDb = new DataSet();


        public void Add(Organization entity)
        {
            auctionDb.Clear();

            using (SqlConnection connection=new SqlConnection(connectionString))
            {
                connection.Open();

                string selectSql = $"select * from {organizationsTable} where [OrganizationName]='{entity.Name}'";
                using (SqlDataAdapter adapter=new SqlDataAdapter(selectSql, connection))
                {
                    adapter.Fill(auctionDb);
                    SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

                    if (auctionDb.Tables[0].Rows.Count!=0)                    
                        throw new Exception($"Already has an organization with name = {entity.Name}");

                    auctionDb.Clear();
                    string insertSql = $"select * from {organizationsTable}";
                    adapter.SelectCommand = new SqlCommand(insertSql, connection);
                    commandBuilder = new SqlCommandBuilder(adapter);

                    adapter.Fill(auctionDb);
                    DataTable table = auctionDb.Tables[0];
                    DataRow newRow = table.NewRow();
                    newRow["OrganizationId"] = entity.Id;
                    newRow["OrganizationName"] = entity.Name;
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

                string selectSqlById = $"select * from {organizationsTable} where [OrganizationId]='{id}'";
                using (SqlDataAdapter adapter = new SqlDataAdapter(selectSqlById, connection))
                {
                    adapter.Fill(auctionDb);
                    SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

                    if (auctionDb.Tables[0].Rows.Count == 0)
                        throw new Exception($"There is no organization with id = {id}");
                    
                    string deleteSql = $"delete from {organizationsTable} where [OrganizationId]='{id}'";
                    SqlCommand command = new SqlCommand(deleteSql, connection);
                    command.ExecuteNonQuery();
                }
            }
        }

        public Organization Read(string id)
        {
            auctionDb.Clear();
            Organization organization = new Organization();

            using (SqlConnection connection=new SqlConnection(connectionString))
            {
                connection.Open();

                string selectSqlById = $"select * from {organizationsTable} where [OrganizationId]='{id}'";
                using (SqlDataAdapter adapter=new SqlDataAdapter(selectSqlById, connection))
                {
                    adapter.Fill(auctionDb);
                    SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

                    if (auctionDb.Tables[0].Rows.Count==0)
                        throw new Exception($"There is no organization with id = {id}");
                    
                    DataTable table = auctionDb.Tables[0];
                    organization.Id = table.Rows[0]["OrganizationId"].ToString();
                    organization.Name = table.Rows[0]["OrganizationName"].ToString();                    
                }                
            }
            return organization;
        }

        public IEnumerable<Organization> ReadAll()
        {
            List<Organization> organizations = new List<Organization>();
            auctionDb.Clear();

            using (SqlConnection connection=new SqlConnection(connectionString))
            {
                connection.Open();

                string selectSqlAll = $"select * from {organizationsTable}";
                using (SqlDataAdapter adapter=new SqlDataAdapter(selectSqlAll, connection))
                {
                    adapter.Fill(auctionDb);
                    SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

                    if (auctionDb.Tables[0].Rows.Count==0)
                        throw new Exception($"There is no organization in database");
                    
                    DataTable table = auctionDb.Tables[0];
                    foreach (DataRow item in table.Rows)
                    {
                        Organization organization = new Organization()
                        {
                            Id = item["OrganizationId"].ToString(),
                            Name = item["OrganizationName"].ToString()
                        };
                        organizations.Add(organization);
                    }
                }
            }

            return organizations;
        }

        public void Update(string id, Organization updated)
        {
            auctionDb.Clear();

            using (SqlConnection connection=new SqlConnection(connectionString))
            {
                connection.Open();

                string selectSqlById = $"select * from {organizationsTable} where [OrganizationId]='{id}'";
                using (SqlDataAdapter adapter=new SqlDataAdapter(selectSqlById, connection))
                {
                    adapter.Fill(auctionDb);
                    SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

                    if (auctionDb.Tables[0].Rows.Count==0)
                        throw new Exception($"There is no organization with id = {id}");

                    DataTable table = auctionDb.Tables[0];
                    table.Rows[0]["OrganizationId"] = updated.Id;
                    table.Rows[0]["OrganizationName"] = updated.Name;
                    adapter.Update(auctionDb);                    
                }
            }
        }
    }
}
