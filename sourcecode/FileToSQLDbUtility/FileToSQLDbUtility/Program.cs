using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using FileToSQLDbUtility.Controllers;
using System.Threading;

namespace FileToSQLDbUtility
{
    public class Program
    {
        public static string CreateErrorFolder()
        {
            //create error folder for storing files which is not read successfully
            string path = Environment.CurrentDirectory + "/Error";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }


        static void Main(string[] args)
        {
            try
            {
                //Check if error folder exists               
                Console.WriteLine("Initilizing Utility please wait....");                
                var errorFolderPath = System.Configuration.ConfigurationManager.AppSettings["ErrorFolder"];
                if (string.IsNullOrEmpty(errorFolderPath))
                {
                    errorFolderPath = CreateErrorFolder();
                    Console.WriteLine("Error folder created on \"{0}\"", errorFolderPath);
                }
                else
                {
                    Console.WriteLine("Error folder path is \"{0}\"", errorFolderPath);
                }
                
                Console.WriteLine("Utility Initialization completed");

                var folderPath = System.Configuration.ConfigurationManager.AppSettings["FolderPath"];
                var isDeleteFileFlag=System.Configuration.ConfigurationManager.AppSettings["IsDeleteFileAfterProcessed"];

                var isDeleteFile=string.IsNullOrEmpty(isDeleteFileFlag)?false: isDeleteFileFlag=="1"?true:false;

                if (isDeleteFile)
                {
                    Console.WriteLine("*******Warning: File delete command is enabled.*******");
                    Thread.Sleep(3000);
                }
                
                var fileHandler = new FileHandler(folderPath,errorFolderPath, isDeleteFile);
                fileHandler.Process();

                Console.WriteLine("\nProcess has been completed press any key for exit....");
                Console.ReadKey();
            }
            catch(Exception ex)
            {
                Console.WriteLine("\nSomething went wrong press any key for exit and restart utility....");
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
        }
    }
}
