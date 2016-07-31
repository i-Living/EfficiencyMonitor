using SQLite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace EfficiencyMonitor.DB
{
    class DbConnector
    {
        private string dbPath;

        public DbConnector(string dbPath)
        {
            this.dbPath = dbPath;
        }
        private bool CheckRecord(string tableName, string columnName, string record)
        {
            using (System.Data.SQLite.SQLiteConnection connection = new System.Data.SQLite.SQLiteConnection(dbPath))
            {
                connection.ConnectionString = "Data Source = " + dbPath;
                connection.Open();
                using (System.Data.SQLite.SQLiteCommand command = new System.Data.SQLite.SQLiteCommand(connection))
                {
                    command.CommandText = @"SELECT COUNT(*) FROM " + tableName + " WHERE " + columnName +" = '" + record +"';";
                    //command.CommandText = @"SELECT COUNT(*) FROM " + tableName + " WHERE @columnName = @record;";
                    //command.Parameters.Add("@tableName", DbType.String).Value = tableName;
                    //command.Parameters.Add("@columnName", DbType.StringFixedLength).Value = columnName;
                    //command.Parameters.Add("@record", DbType.String).Value = record;
                    int count = Convert.ToInt16(command.ExecuteScalar());
                    if (count > 0)
                        return true;
                }
            }
            return false;
        }
        public void AddProcess(ProcessData Process)
        {
            using (System.Data.SQLite.SQLiteConnection connection = new System.Data.SQLite.SQLiteConnection(dbPath))
            {
                connection.ConnectionString = "Data Source = " + dbPath;
                connection.Open();
                using (System.Data.SQLite.SQLiteCommand command = new System.Data.SQLite.SQLiteCommand(connection))
                {
                    command.CommandText = "INSERT INTO KnownApplications (Name, Time) VALUES (@name, @time);";
                    command.Parameters.Add("@name", DbType.String).Value = Process.Name;
                    command.Parameters.Add("@time", DbType.UInt64).Value = Process.Time;
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
        public ProcessCollection GetProcessCollection(string TableName)
        {
            ProcessCollection pc = new ProcessCollection();

            return pc;
        }
        public void UpdateProcess(ProcessData Process)
        {
            if(CheckRecord("KnownApplications", "Name", Process.Name))
            {
                using (System.Data.SQLite.SQLiteConnection connection = new System.Data.SQLite.SQLiteConnection(dbPath))
                {
                    connection.ConnectionString = "Data Source = " + dbPath;
                    connection.Open();
                    using (System.Data.SQLite.SQLiteCommand command = new System.Data.SQLite.SQLiteCommand(connection))
                    {
                        command.CommandText = "Update KnownApplications SET Time = " + Process.Time + " WHERE Name = '" + Process.Name + "';";
                        //command.Parameters.Add("@name", DbType.String).Value = Process.Name;
                        //command.Parameters.Add("@time", DbType.UInt64).Value = Process.Time;
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }
            else
            {
                AddProcess(Process);
            }
            
        }
        public void UpdateAllProcess(ProcessCollection ProcessC)
        {
            foreach (var Process in ProcessC)
            {
                UpdateProcess(Process);
            }
        }
        public DataTable GetTable(string tableName, string columnName)
        {
            DataTable dt = new DataTable();
            using (System.Data.SQLite.SQLiteConnection connection = new System.Data.SQLite.SQLiteConnection(dbPath))
            {
                connection.ConnectionString = "Data Source = " + dbPath;
                connection.Open();
                using (System.Data.SQLite.SQLiteCommand command = new System.Data.SQLite.SQLiteCommand(connection))
                {
                    command.CommandText = "SELECT * FROM " + tableName + " WHERE Name like " + columnName + " ORDER BY Name;";
                    //command.Parameters.Add("@name", DbType.String).Value = '%' + name + '%';
                    SQLiteDataReader reader = command.ExecuteReader();
                    dt.Load(reader);
                    reader.Close();
                }
                connection.Close();
            }
            return dt;
        }
        public DataSet GetDataSet(string tableName, string columnName)
        {
            DataSet ds = new DataSet();
            using (System.Data.SQLite.SQLiteConnection connection = new System.Data.SQLite.SQLiteConnection(dbPath))
            {
                connection.ConnectionString = "Data Source = " + dbPath;
                connection.Open();
                using (System.Data.SQLite.SQLiteCommand command = new System.Data.SQLite.SQLiteCommand(connection))
                {
                    command.CommandText = "SELECT * FROM " + tableName + " WHERE Name like " + columnName + " ORDER BY Name;";
                    //command.Parameters.Add("@name", DbType.String).Value = '%' + name + '%';
                    SQLiteDataAdapter da = new SQLiteDataAdapter(command.CommandText, connection);
                    da.Fill(ds);
                }
                connection.Close();
            }
            return ds;
        }
        public List<ProcessData> ConvertToItemList(DataRowCollection rows)
        {
            List<ProcessData> itemList = new List<ProcessData>();

            for (int i = 0; i < rows.Count; i++)
            {
                itemList.Add(new ProcessData(
                    Convert.ToInt32(rows[i]["Id"]),
                    rows[i]["Name"].ToString(),
                     Convert.ToUInt64(rows[i]["Time"])));
            }
            return itemList;
        }
        public List<ProcessData> GetAllProcess(string name)
        {
            return ConvertToItemList(GetTable(name,"Name").Rows);
        }
        public void DeleteProcess(ProcessData Process)
        {
            using (System.Data.SQLite.SQLiteConnection connection = new System.Data.SQLite.SQLiteConnection(dbPath))
            {
                connection.ConnectionString = "Data Source = " + dbPath;
                connection.Open();
                using (System.Data.SQLite.SQLiteCommand command = new System.Data.SQLite.SQLiteCommand(connection))
                {
                    command.CommandText = "DELETE FROM KnownApplications WHERE id = @id;";
                    command.Parameters.Add("@id", DbType.Int32).Value = Process.ProcessId;
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
    }
}
