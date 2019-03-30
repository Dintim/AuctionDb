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
    public class LotItemRepository : IRepository<LotItem>
    {
        string connectionString = ConfigurationManager.ConnectionStrings["AuctionDbConnection"].ConnectionString;
        string lotItemTable = $"[dbo].[LotItems]";
        DataSet auctionDb = new DataSet();


        public void Add(LotItem entity)
        {
            auctionDb.Clear();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string selectSql = $"select * from {lotItemTable} where [LotId]='{entity.Id}'";
                using (SqlDataAdapter adapter = new SqlDataAdapter(selectSql, connection))
                {
                    adapter.Fill(auctionDb);
                    SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

                    if (auctionDb.Tables[0].Rows.Count != 0)
                        throw new Exception($"Already has an lot with id = {entity.Id}");

                    auctionDb.Clear();
                    string insertSql = $"select * from {lotItemTable}";
                    adapter.SelectCommand = new SqlCommand(insertSql, connection);
                    commandBuilder = new SqlCommandBuilder(adapter);

                    adapter.Fill(auctionDb);
                    DataTable table = auctionDb.Tables[0];
                    DataRow newRow = table.NewRow();
                    newRow["LotId"] = entity.Id;
                    newRow["LotName"] = entity.Name;
                    newRow["LotDescription"] = entity.Description;
                    newRow["PublishedDate"] = entity.PublishedDate.ToString("yyyy-MM-dd");
                    newRow["InitialCost"] = entity.InitialCost;                    
                    newRow["CreatedByEmployeeId"] = entity.CreatedByEmployeeId;
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

                string selectSqlById = $"select * from {lotItemTable} where [LotId]='{id}'";
                using (SqlDataAdapter adapter = new SqlDataAdapter(selectSqlById, connection))
                {
                    adapter.Fill(auctionDb);
                    SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

                    if (auctionDb.Tables[0].Rows.Count == 0)
                        throw new Exception($"There is no lot with id = {id}");

                    string deleteSql = $"delete from {lotItemTable} where [LotId]='{id}'";
                    SqlCommand command = new SqlCommand(deleteSql, connection);
                    command.ExecuteNonQuery();
                }
            }
        }

        public LotItem Read(string id)
        {
            auctionDb.Clear();
            LotItem lotItem = new LotItem();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string selectSqlById = $"select * from {lotItemTable} where [LotId]='{id}'";
                using (SqlDataAdapter adapter = new SqlDataAdapter(selectSqlById, connection))
                {
                    adapter.Fill(auctionDb);
                    SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

                    if (auctionDb.Tables[0].Rows.Count == 0)
                        throw new Exception($"There is no lot with id = {id}");

                    DataTable table = auctionDb.Tables[0];
                    lotItem.Id = table.Rows[0]["LotId"].ToString();
                    lotItem.Name = table.Rows[0]["LotName"].ToString();
                    lotItem.Description = table.Rows[0]["LotDescription"].ToString();
                    lotItem.PublishedDate = Convert.ToDateTime(table.Rows[0]["PublishedDate"].ToString());
                    lotItem.InitialCost = Convert.ToDecimal(table.Rows[0]["InitialCost"].ToString());
                    lotItem.CreatedByEmployeeId = table.Rows[0]["CreatedByEmployeeId"].ToString();
                }
            }
            return lotItem;
        }

        public IEnumerable<LotItem> ReadAll()
        {
            List<LotItem> lotItems = new List<LotItem>();
            auctionDb.Clear();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string selectSqlAll = $"select * from {lotItemTable}";
                using (SqlDataAdapter adapter = new SqlDataAdapter(selectSqlAll, connection))
                {
                    adapter.Fill(auctionDb);
                    SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

                    if (auctionDb.Tables[0].Rows.Count == 0)
                        throw new Exception($"There are no lots in database");

                    DataTable table = auctionDb.Tables[0];
                    foreach (DataRow item in table.Rows)
                    {
                        LotItem lot = new LotItem()
                        {
                            Id = item["LotId"].ToString(),
                            Name = item["LotName"].ToString(),
                            Description = item["LotDescription"].ToString(),
                            PublishedDate = Convert.ToDateTime(item["PublishedDate"].ToString()),
                            InitialCost = Convert.ToDecimal(item["InitialCost"].ToString()),
                            CreatedByEmployeeId = item["CreatedByEmployeeId"].ToString()
                        };
                        lotItems.Add(lot);
                    }
                }
            }

            return lotItems;
        }

        public void Update(string id, LotItem updated)
        {
            auctionDb.Clear();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string selectSqlById = $"select * from {lotItemTable} where [LotId]='{id}'";
                using (SqlDataAdapter adapter = new SqlDataAdapter(selectSqlById, connection))
                {
                    adapter.Fill(auctionDb);
                    SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

                    if (auctionDb.Tables[0].Rows.Count == 0)
                        throw new Exception($"There is no lot with id = {id}");

                    DataTable table = auctionDb.Tables[0];
                    table.Rows[0]["LotId"] = updated.Id;
                    table.Rows[0]["LotName"] = updated.Name;
                    table.Rows[0]["LotDescription"] = updated.Description;
                    table.Rows[0]["PublishedDate"] = updated.PublishedDate.ToString("yyyy-MM-dd");
                    table.Rows[0]["InitialCost"] = updated.InitialCost;                    
                    table.Rows[0]["CreatedByEmployeeId"] = updated.CreatedByEmployeeId;
                    adapter.Update(auctionDb);
                }
            }
        }
    }
}
