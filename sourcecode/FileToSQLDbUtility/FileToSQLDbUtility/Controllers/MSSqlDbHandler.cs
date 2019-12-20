using System;
using System.Collections.Generic;
using FileToSQLDbUtility.Enum;
using FileToSQLDbUtility.Models;
using System.Data.SqlClient;

namespace FileToSQLDbUtility.Controllers
{
    public class MSSqlDbHandler : DbInitializer, IDbHandler
    {
        private SqlConnection _connection;

        public MSSqlDbHandler()
        {
            Init();
            ConnectionString = "Data Source=" + Server + ";Initial Catalog=" + Database + ";";
        
            if(string.IsNullOrEmpty(UID) || string.IsNullOrEmpty(Password))
            {
                ConnectionString += "Integrated Security=true;";
            }
            else
            {
                ConnectionString += "User Id=" + UID + ";" + "Password=" + Password + ";";
            }
            
            _connection = new SqlConnection(ConnectionString);
            _connection.Open();
        }

        public string InsertQueryBuilder(string[] cols)
        {
            var columns = "(";
            var values = "VALUES(";
            var colPosition = 0;
            foreach (var col in columnnames)
            {
                var coldata = "";
                if (colPosition < cols.Length)
                {
                    coldata = cols[colPosition];
                    coldata = coldata.Replace("'", "''");
                }
                
                columns += "[" + col+"]";
                values += "'" + coldata + "'";
                if (colPosition < columnnames.Length-1)
                {
                    columns += ",";
                    values += ",";
                }
                colPosition++;
            }
            columns += ")";
            values += ")";

            string query = "INSERT INTO "+detailsTableName+" " + columns + values + ";";
            return query;
        }

        public void AddDetails(string[] lines)
        {
            var inserQuery = "";
            foreach (var line in lines)
            {
                inserQuery += InsertQueryBuilder(line.Split(','));
            }

            //create command and assign the query and connection from the constructor
            SqlCommand cmd = new SqlCommand(inserQuery, _connection);

            //Execute command
            cmd.ExecuteNonQuery();

        }

        public void AddFileStatus(FileStatusHandlerModel fileStatusHandlerModel)
        {
            string query = "INSERT INTO " +fileStatusTableName+" (FileName,FileStatus) VALUES('" + fileStatusHandlerModel.FileName + "', '" + (int)fileStatusHandlerModel.Status + "')";
            SqlCommand cmd = new SqlCommand(query, _connection);
            cmd.ExecuteNonQuery();
        }

        public List<FileStatusHandlerModel> GetAllFileStatus(List<FileStatusHandlerModel> _fileRepo, string filePath)
        {
            var conditions = "(";
            var counter = 1;
            foreach (var file in _fileRepo)
            {

                conditions += "'" + file.FileName + "'";
                if (counter < _fileRepo.Count)
                {
                    conditions += ",";
                }
                counter++;
            }

            conditions += ")";

            string query = "SELECT * FROM "+fileStatusTableName+" where filename in " + conditions + "";

            //Create a list to store the result
            var fileStatusRepo = new List<FileStatusHandlerModel>();

            SqlCommand cmd = new SqlCommand(query, _connection);
            //Create a data reader and Execute the command
            SqlDataReader dataReader = cmd.ExecuteReader();

            //Read the data and store them in the list
            while (dataReader.Read())
            {
                var fileStatusHandlerModel = new FileStatusHandlerModel();
                fileStatusHandlerModel.FileName = Convert.ToString(dataReader["filename"]);
                fileStatusHandlerModel.Status = (FileProcessStatus)(int)dataReader["filestatus"];
                fileStatusHandlerModel.FilePath = filePath + "/" + Convert.ToString(dataReader["filename"]);
                fileStatusRepo.Add(fileStatusHandlerModel);
            }

            //close Data Reader
            dataReader.Close();
            return fileStatusRepo;
        }

        public void Dispose()
        {
            _connection.Close();
            _connection = null;
        }
    }
}
