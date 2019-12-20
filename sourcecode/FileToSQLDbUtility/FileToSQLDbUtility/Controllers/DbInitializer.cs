using FileToSQLDbUtility.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FileToSQLDbUtility.Controllers
{
    public abstract class DbInitializer
    {

        public string Server;
        public string Database;
        public string UID;
        public string Password;
        public string ConnectionString { get; set; }
        public string detailsTableName { get; set; }
        public string fileStatusTableName { get; set; }
        public string[] columnnames { get; set; }
        public void Init()
        {
            detailsTableName =string.IsNullOrEmpty(ConfigurationManager.AppSettings["detailstablename"])? "details": ConfigurationManager.AppSettings["detailstablename"];
            fileStatusTableName = string.IsNullOrEmpty(ConfigurationManager.AppSettings["filereadstatustablename"]) ? "filereadstatus" : ConfigurationManager.AppSettings["filereadstatustablename"];

            Server = System.Configuration.ConfigurationManager.AppSettings["ServerName"];
            Database = System.Configuration.ConfigurationManager.AppSettings["DbName"];
            UID = System.Configuration.ConfigurationManager.AppSettings["UserName"];
            Password = System.Configuration.ConfigurationManager.AppSettings["Password"];
            ConnectionString = "SERVER=" + Server + ";SslMode=none;" + "DATABASE=" +
            Database + ";" + "UID=" + UID + ";" + "PASSWORD=" + Password + ";";


            var columnstring = string.IsNullOrEmpty(ConfigurationManager.AppSettings["columnsname"]) ? "" : ConfigurationManager.AppSettings["columnsname"];
            if (!string.IsNullOrEmpty(columnstring))
            {
                columnnames =columnstring.Split(',');
            }
            if (columnnames.Length == 0)
            {
                throw new Exception("No column added in config file, please add columns name by comma seperate");
            }

        }

    }
}
