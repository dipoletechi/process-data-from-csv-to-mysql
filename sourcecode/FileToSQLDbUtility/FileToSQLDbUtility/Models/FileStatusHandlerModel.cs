
using FileToSQLDbUtility.Enum;
using System.IO;

namespace FileToSQLDbUtility.Models
{
    public class FileStatusHandlerModel
    {       
        public string FileName
        {
            get;set;
        }
        public FileProcessStatus Status { get; set; }
        public string FilePath { get; set; }
    }
}
