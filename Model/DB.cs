using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class DB
    {
        private string dbName;

        public string DbName
        {
            get { return dbName; }
            set { dbName = value; }
        }

        public DB(string dbName)
        {
            this.dbName = dbName;
            CheckDB();
        }

        public void CheckDB()
        {
            if (!File.Exists(dbName))
                SQLiteConnection.CreateFile(dbName);
            CreateTables();
        }

        public void CreateTables()
        {
            using (SQLiteConnection con = new SQLiteConnection("Data Source=" + dbName))
            {
                con.Open();
                using (SQLiteCommand command = con.CreateCommand())
                {

                    command.CommandText = "SELECT name FROM sqlite_master WHERE name='AdapterConfiguration'";
                    var name = command.ExecuteScalar();

                    // check account table exist or not 
                    // if exist do nothing 
                    if (name != null && name.ToString() == "AdapterConfiguration")
                        return;
                    // acount table not exist, create table and insert 
                    command.CommandText = "CREATE TABLE `AdapterConfiguration` ( `Id` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,`GroupName` TEXT,`Name` TEXT NOT NULL, `Description` TEXT, `IpAddress` TEXT NOT NULL, `SubnetMask` TEXT NOT NULL, `Gateway` TEXT, `DHCPServer` TEXT, `DNSServer1` TEXT, `DNSServer2` TEXT)";
                    command.ExecuteNonQuery();
                }
                con.Close();
            }
        }

        public bool IsThereRecord(string Name)
        {
            using (SQLiteConnection con = new SQLiteConnection("Data Source=" + dbName))
            {
                con.Open();
                using (SQLiteCommand command = con.CreateCommand())
                {
                    command.CommandText = "SELECT Name FROM AdapterConfiguration WHERE Name='" + Name + "'";
                    var name = command.ExecuteScalar();
                    if (name != null && name.ToString() == Name)
                    {
                        return true;
                    }
                }
                con.Close();
            }
            return false;
        }

        public bool IsThereRecord(string Name, long Id)
        {
            using (SQLiteConnection con = new SQLiteConnection("Data Source=" + dbName))
            {
                con.Open();
                using (SQLiteCommand command = con.CreateCommand())
                {
                    command.CommandText = "SELECT Name FROM AdapterConfiguration WHERE Name='" + Name + "' AND Id!=" + Id.ToString() + "";
                    var name = command.ExecuteScalar();
                    if (name != null && name.ToString() == Name)
                    {
                        return true;
                    }
                }
                con.Close();
            }
            return false;
        }

        public List<AdapterConfiguration> ListRecords()
        {
            List<AdapterConfiguration> dbAdapterList = new List<AdapterConfiguration>();
            using (SQLiteConnection con = new SQLiteConnection("Data Source=" + dbName))
            {
                con.Open();
                using (SQLiteCommand command = new SQLiteCommand(con))
                {
                    command.CommandText = "select * from AdapterConfiguration order by Id desc";
                    SQLiteDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        dbAdapterList.Add(new AdapterConfiguration
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            GroupName = (string)reader["GroupName"],
                            Name = (string)reader["Name"],
                            Description = (string)reader["Description"],
                            IpAddress = (string)reader["IpAddress"],
                            SubnetMask = (string)reader["SubnetMask"],
                            Gateway = (string)reader["Gateway"],
                            DHCPServer = (string)reader["DHCPServer"],
                            DNSServer1 = (string)reader["DNSServer1"],
                            DNSServer2 = (string)reader["DNSServer2"],
                        });
                    }
                }
                con.Close();
            }
            return dbAdapterList;
        }

        public void AddRecord(ref AdapterConfiguration data)
        {

            using (SQLiteConnection con = new SQLiteConnection("Data Source=" + dbName))
            {
                con.Open();
                using (SQLiteCommand command = new SQLiteCommand(con))
                {
                    command.CommandText = "insert into AdapterConfiguration (GroupName,Name,Description,IpAddress,SubnetMask,Gateway,DHCPServer,DNSServer1,DNSServer2) " +
                                          "values(@GroupName,@Name,@Description,@IpAddress,@SubnetMask,@Gateway,@DHCPServer,@DNSServer1,@DNSServer2); " +
                        "select last_insert_rowid();";
                    command.CommandType = System.Data.CommandType.Text;
                    command.Parameters.Add(new SQLiteParameter("@GroupName", data.GroupName != null ? data.GroupName : ""));
                    command.Parameters.Add(new SQLiteParameter("@Name", data.Name != null ? data.Name : ""));
                    command.Parameters.Add(new SQLiteParameter("@Description", data.Description != null ? data.Description : ""));
                    command.Parameters.Add(new SQLiteParameter("@IpAddress", data.IpAddress != null ? data.IpAddress : ""));
                    command.Parameters.Add(new SQLiteParameter("@SubnetMask", data.SubnetMask != null ? data.SubnetMask : ""));
                    command.Parameters.Add(new SQLiteParameter("@Gateway", data.Gateway != null ? data.Gateway : ""));
                    command.Parameters.Add(new SQLiteParameter("@DHCPServer", data.DHCPServer != null ? data.DHCPServer : ""));
                    command.Parameters.Add(new SQLiteParameter("@DNSServer1", data.DNSServer1 != null ? data.DNSServer1 : ""));
                    command.Parameters.Add(new SQLiteParameter("@DNSServer2", data.DNSServer2 != null ? data.DNSServer2 : ""));
                    object obj = command.ExecuteScalar();
                    data.Id = (long)obj;

                }
                con.Close();
            }

        }

        public int DeleteRecord(long id)
        {
            int status;
            using (SQLiteConnection con = new SQLiteConnection("Data Source=" + dbName))
            {
                con.Open();
                using (SQLiteCommand command = new SQLiteCommand(con))
                {
                    command.CommandText = "delete from AdapterConfiguration where Id=" + id.ToString() + "";
                    status = command.ExecuteNonQuery();
                }
                con.Close();
            }
            return status;
        }

        public int UpdateRecord(AdapterConfiguration data)
        {
            int status;
            using (SQLiteConnection con = new SQLiteConnection("Data Source=" + dbName))
            {

                con.Open();
                using (SQLiteCommand command = new SQLiteCommand(con))
                {
                    command.CommandText = "update AdapterConfiguration set GroupName=@GroupName,Name=@Name, " +
                        "Description=@Description, IpAddress=@IpAddress, SubnetMask=@SubnetMask, Gateway=@Gateway, " +
                        "DHCPServer=@DHCPServer, DNSServer1=@DNSServer1, DNSServer2=@DNSServer2 where Id=@Id; " + "";

                    command.CommandType = System.Data.CommandType.Text;
                    command.Parameters.Add(new SQLiteParameter("@Id", data.Id));
                    command.Parameters.AddWithValue("@GroupName", data.GroupName);
                    command.Parameters.AddWithValue("@Name", data.Name);
                    command.Parameters.AddWithValue("@Description", data.Description);
                    command.Parameters.AddWithValue("@IpAddress", data.IpAddress);
                    command.Parameters.AddWithValue("@SubnetMask", data.SubnetMask);
                    command.Parameters.AddWithValue("@Gateway", data.Gateway);
                    command.Parameters.AddWithValue("@DHCPServer", data.DHCPServer);
                    command.Parameters.AddWithValue("@DNSServer1", data.DNSServer1);
                    command.Parameters.AddWithValue("@DNSServer2", data.DNSServer2);
                    status = command.ExecuteNonQuery();
                }
                con.Close();
            }
            return status;
        }
    }

    public static class DBStatic
    {
        public static DB db = new DB("AdapterDB");

    }
}