using System;
using System.Collections.Generic;
using System.IO;
using FileToSQLDbUtility.Enum;
using FileToSQLDbUtility.Models;
using MySql.Data.MySqlClient;

namespace FileToSQLDbUtility.Controllers
{
    public class MySqlDbHandler: DbInitializer,IDbHandler
    {
        private MySqlConnection _connection;

        public MySqlDbHandler()
        {
            Init();
            _connection = new MySqlConnection(ConnectionString);
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

                columns += "" +col;
                values +="'"+ coldata+"'";
                if (colPosition < columnnames.Length-1)
                {
                    columns += ",";
                    values += ",";
                }
                    colPosition++;
            }
            columns += ")";
            values += ")";
           
            string query = "INSERT INTO "+detailsTableName+" "+columns+values+";";
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
            MySqlCommand cmd = new MySqlCommand(inserQuery, _connection);

            //Execute command
            cmd.ExecuteNonQuery();

        }

        public void AddFileStatus(FileStatusHandlerModel fileStatusHandlerModel)
        {
           
            string query = "INSERT INTO filereadstatus (FileName,FileStatus) VALUES('"+ fileStatusHandlerModel.FileName + "', '"+(int)fileStatusHandlerModel.Status+"')";

            MySqlCommand cmd = new MySqlCommand(query, _connection);

            cmd.ExecuteNonQuery();
        }

        public List<FileStatusHandlerModel> GetAllFileStatus(List<FileStatusHandlerModel> _fileRepo,string filePath)
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

            MySqlCommand cmd = new MySqlCommand(query, _connection);
            //Create a data reader and Execute the command
            MySqlDataReader dataReader = cmd.ExecuteReader();

            //Read the data and store them in the list
            while (dataReader.Read())
            {
                var fileStatusHandlerModel = new FileStatusHandlerModel();
                fileStatusHandlerModel.FileName = Convert.ToString(dataReader["filename"]);
                fileStatusHandlerModel.Status = (FileProcessStatus)(int)dataReader["filestatus"];
                fileStatusHandlerModel.FilePath = filePath+"/"+ Convert.ToString(dataReader["filename"]);
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
