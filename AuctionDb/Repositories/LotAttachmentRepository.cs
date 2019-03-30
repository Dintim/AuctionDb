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
    public class LotAttachmentRepository : IRepository<LotAttachment>
    {
        string connectionString = ConfigurationManager.ConnectionStrings["AuctionDbConnection"].ConnectionString;
        string attachmentTable = $"[dbo].[LotItemAttachments]";
        DataSet auctionDb = new DataSet();


        public void Add(LotAttachment entity)
        {
            auctionDb.Clear();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string selectSql = $"select * from {attachmentTable} where [AttachmentId]='{entity.Id}'";
                using (SqlDataAdapter adapter = new SqlDataAdapter(selectSql, connection))
                {
                    adapter.Fill(auctionDb);
                    //SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

                    if (auctionDb.Tables[0].Rows.Count != 0)
                        throw new Exception($"Already has an lot attachment with id = {entity.Id}");

                    auctionDb.Clear();
                    string selectAllSql = $"select * from {attachmentTable}";
                    adapter.SelectCommand = new SqlCommand(selectAllSql, connection);
                    //commandBuilder = new SqlCommandBuilder(adapter);
                    adapter.Fill(auctionDb);

                    DataTable table = auctionDb.Tables[0];
                    DataRow newRow = table.NewRow();
                    newRow["AttachmentId"] = entity.Id;
                    newRow["AttachmentName"] = entity.Name;
                    newRow["AttachmentExtension"] = entity.Extension;
                    newRow["AttachmentBody"] = entity.Body;
                    newRow["LotItemId"] = entity.LotItemId;
                    table.Rows.Add(newRow);

                    string insertSql = $"INSERT INTO {attachmentTable} ([AttachmentId],[AttachmentName],[AttachmentExtension],[AttachmentBody],[LotItemId])" +
                        $"VALUES (@AttachmentId,@AttachmentName,@AttachmentExtension,@AttachmentBody,@LotItemId)";
                    SqlCommand command = new SqlCommand(insertSql, connection);
                    command.Parameters.Add("@AttachmentId", SqlDbType.UniqueIdentifier, int.MaxValue, "AttachmentId");
                    command.Parameters.Add("@AttachmentName", SqlDbType.NVarChar, int.MaxValue, "AttachmentName");
                    command.Parameters.Add("@AttachmentExtension", SqlDbType.NVarChar, int.MaxValue, "AttachmentExtension");
                    command.Parameters.Add("@AttachmentBody", SqlDbType.VarBinary, int.MaxValue, "AttachmentBody");
                    command.Parameters.Add("@LotItemId", SqlDbType.UniqueIdentifier, int.MaxValue, "LotItemId");
                    adapter.InsertCommand = command;
                    //commandBuilder = new SqlCommandBuilder(adapter);
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

                string selectSqlById = $"select * from {attachmentTable} where [AttachmentId]='{id}'";
                using (SqlDataAdapter adapter = new SqlDataAdapter(selectSqlById, connection))
                {
                    adapter.Fill(auctionDb);
                    SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

                    if (auctionDb.Tables[0].Rows.Count == 0)
                        throw new Exception($"There is no lot attachment with id = {id}");

                    string deleteSql = $"delete from {attachmentTable} where [AttachmentId]='{id}'";
                    SqlCommand command = new SqlCommand(deleteSql, connection);
                    command.ExecuteNonQuery();
                }
            }
        }

        public LotAttachment Read(string id)
        {
            auctionDb.Clear();
            LotAttachment attachment = new LotAttachment();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string selectSqlById = $"select * from {attachmentTable} where [AttachmentId]='{id}'";
                using (SqlDataAdapter adapter = new SqlDataAdapter(selectSqlById, connection))
                {
                    adapter.Fill(auctionDb);
                    SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

                    if (auctionDb.Tables[0].Rows.Count == 0)
                        throw new Exception($"There is no lot attachment with id = {id}");

                    DataTable table = auctionDb.Tables[0];
                    attachment.Id = table.Rows[0]["AttachmentId"].ToString();
                    attachment.Name = table.Rows[0]["AttachmentName"].ToString();
                    attachment.Extension = table.Rows[0]["AttachmentExtension"].ToString();
                    attachment.Body = Encoding.ASCII.GetBytes(table.Rows[0]["AttachmentBody"].ToString());
                    attachment.LotItemId = table.Rows[0]["LotItemId"].ToString();
                }
            }
            return attachment;
        }

        public IEnumerable<LotAttachment> ReadAll()
        {
            List<LotAttachment> attachments = new List<LotAttachment>();
            auctionDb.Clear();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string selectSqlAll = $"select * from {attachmentTable}";
                using (SqlDataAdapter adapter = new SqlDataAdapter(selectSqlAll, connection))
                {
                    adapter.Fill(auctionDb);
                    SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

                    if (auctionDb.Tables[0].Rows.Count == 0)
                        throw new Exception($"There are no lot attachments in database");

                    DataTable table = auctionDb.Tables[0];
                    foreach (DataRow item in table.Rows)
                    {
                        LotAttachment attachment = new LotAttachment()
                        {
                            Id = item["AttachmentId"].ToString(),
                            Name = item["AttachmentName"].ToString(),
                            Extension = item["AttachmentExtension"].ToString(),
                            Body = Encoding.ASCII.GetBytes(item["AttachmentBody"].ToString()),
                            LotItemId = item["AttachmentId"].ToString(),
                        };
                        attachments.Add(attachment);
                    }
                }
            }

            return attachments;
        }

        public void Update(string id, LotAttachment updated)
        {
            auctionDb.Clear();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string selectSqlById = $"select * from {attachmentTable} where [AttachmentId]='{id}'";
                using (SqlDataAdapter adapter = new SqlDataAdapter(selectSqlById, connection))
                {
                    adapter.Fill(auctionDb);
                    SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

                    if (auctionDb.Tables[0].Rows.Count == 0)
                        throw new Exception($"There is no lot attachment with id = {id}");

                    DataTable table = auctionDb.Tables[0];
                    table.Rows[0]["AttachmentId"] = updated.Id;
                    table.Rows[0]["AttachmentName"] = updated.Name;
                    table.Rows[0]["AttachmentExtension"] = updated.Extension;
                    table.Rows[0]["AttachmentBody"] = updated.Body;
                    table.Rows[0]["LotItemId"] = updated.LotItemId;
                    adapter.Update(auctionDb);
                }
            }
        }
    }
}
