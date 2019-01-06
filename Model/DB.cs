﻿using System;
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
        }

        /// <summary>
        /// if not exist db file => create db file
        /// </summary>
        /// <param name="dbName"></param>
        public void CheckDB()
        {
            if (!File.Exists(dbName))
                SQLiteConnection.CreateFile(dbName);
            CreateTables();
        }

        /// <summary>
        /// SQLite create tables if no exist tables 
        /// </summary>
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
                    command.CommandText = "CREATE TABLE `AdapterConfiguration` ( `Id` INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,`Group` TEXT,`Name` TEXT NOT NULL, `Description` TEXT, `IpAddress` TEXT NOT NULL, `SubnetMask` TEXT NOT NULL, `Gateway` TEXT)";
                    command.ExecuteNonQuery();
                }
                con.Close();
            }
        }

        /// <summary>
        /// SQLite Check Exist Records in VpnName Column by Same Name
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
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

        /// <summary>
        /// SQLite Check Exist Records in VpnName Column by Same Name Expect Id
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
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

        /// <summary>
        /// SQLite Get All Records
        /// </summary>
        /// <returns></returns>
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
                            Group = (string)reader["Group"],
                            Name = (string)reader["Name"],
                            Description = (string)reader["Description"],
                            IpAddress = (string)reader["IpAddress"],
                            SubnetMask = (string)reader["SubnetMask"],
                            Gateway = (string)reader["Gateway"],
                        });
                    }
                }
                con.Close();
            }
            return dbAdapterList;
        }

        /// <summary>
        /// SQLite Insert Record
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public void AddRecord(ref AdapterConfiguration data)
        {

            using (SQLiteConnection con = new SQLiteConnection("Data Source=" + dbName))
            {
                con.Open();
                using (SQLiteCommand command = new SQLiteCommand(con))
                {
                    command.CommandText = "insert into AdapterConfiguration (Group,Name,Description,IpAddress,SubnetMask,Gateway) values(@Group,@Name,@Description,@IpAddress,@SubnetMask,@Gateway); " +
                        "select last_insert_rowid();";
                    command.CommandType = System.Data.CommandType.Text;
                    command.Parameters.Add(new SQLiteParameter("@Group", data.Name));
                    command.Parameters.Add(new SQLiteParameter("@Name", data.Name));
                    command.Parameters.Add(new SQLiteParameter("@Description", data.Description));
                    command.Parameters.Add(new SQLiteParameter("@IpAddress", data.IpAddress));
                    command.Parameters.Add(new SQLiteParameter("@SubnetMask", data.SubnetMask));
                    command.Parameters.Add(new SQLiteParameter("@Gateway", data.Gateway));
                    
                    object obj = command.ExecuteScalar();
                    data.Id = (long)obj;

                }
                con.Close();
            }

        }

        /// <summary>
        /// SQLite Delete Record
        /// </summary>
        /// <param name="id"></param>
        /// <returns>-1 an Error</returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns>-1 an Error</returns>
        public int UpdateRecord(AdapterConfiguration data)
        {
            int status;
            using (SQLiteConnection con = new SQLiteConnection("Data Source=" + dbName))
            {

                con.Open();
                using (SQLiteCommand command = new SQLiteCommand(con))
                {
                    command.CommandText = "update AdapterConfiguration set Group=@Group,Name=@Name, Description=@Description, IpAddress=@IpAddress, SubnetMask=@SubnetMask, Gateway=@Gateway where Id=@Id; " + "";
                    command.CommandType = System.Data.CommandType.Text;
                    command.Parameters.Add(new SQLiteParameter("@Id", data.Id));
                    command.Parameters.AddWithValue("@Group", data.Group);
                    command.Parameters.AddWithValue("@Name", data.Name);
                    command.Parameters.AddWithValue("@Description", data.Description);
                    command.Parameters.AddWithValue("@IpAddress", data.IpAddress);
                    command.Parameters.AddWithValue("@SubnetMask", data.SubnetMask);
                    command.Parameters.AddWithValue("@Gateway", data.Gateway);
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