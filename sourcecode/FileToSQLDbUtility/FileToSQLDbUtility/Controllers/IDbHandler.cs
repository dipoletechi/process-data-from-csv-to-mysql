using FileToSQLDbUtility.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileToSQLDbUtility.Controllers
{
    public interface IDbHandler: IDisposable
    {       
        /// <summary>
        ///Creates insert statements
        /// </summary>
        /// <returns></returns>
        string InsertQueryBuilder(string[] cols);

        /// <summary>
        /// Adds details into database
        /// </summary>
        void AddDetails(string[] lines);

        /// <summary>
        /// All files after processing completion inserts into database
        /// </summary>
        void AddFileStatus(FileStatusHandlerModel fileStatusHandlerModel);

        /// <summary>
        /// Returns all files status from database
        /// </summary>
        List<FileStatusHandlerModel> GetAllFileStatus(List<FileStatusHandlerModel> _fileRepo, string filePath);

    }
}
