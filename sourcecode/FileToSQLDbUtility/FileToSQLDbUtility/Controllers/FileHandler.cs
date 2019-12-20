using FileToSQLDbUtility.Enum;
using FileToSQLDbUtility.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileToSQLDbUtility.Controllers
{
    public class FileHandler
    {
        /// <summary>
        /// this is the path for folder from where we need to read all files
        /// this comes from app settings and can also be update by user from command prompt
        /// </summary>
        private string _folderPath="";
        private string _errorFolderPath;
        private List<FileStatusHandlerModel> _fileRepo;
        private FieldValidator _fieldValidator;
        private bool _isDeleteFile;
        public FileHandler(string folderPath,string errorFolderPath,bool isDeleteFile)
        {
            _isDeleteFile = isDeleteFile;
            _folderPath = folderPath;
            _errorFolderPath = errorFolderPath;
            _fieldValidator = new FieldValidator();
            _fileRepo = new List<FileStatusHandlerModel>();           
        }

        public void Process()
        {
            FileRepoGenerator();
        }


        /// <summary>
        /// This method reads all files path from the given directory and
        /// generates a file repo as list
        /// </summary>
        private void FileRepoGenerator()
        {
            Console.WriteLine("Searching files from the directory {0}",_folderPath);

            var searchedDirectory = new System.IO.DirectoryInfo(_folderPath);
            var filesFoundInDirectory =
                from file in searchedDirectory.GetFiles() select file.FullName;

            Console.WriteLine("Total files found:"+ filesFoundInDirectory.Count());
            Console.WriteLine("Generating file repo");

            foreach (var filePath in filesFoundInDirectory)
            {
                //Only those files should be included which does not have entry in database
                _fileRepo.Add(new FileStatusHandlerModel { FilePath= filePath, Status=FileProcessStatus.Idle,FileName=Path.GetFileName(filePath)});                               
            }
            Console.WriteLine("File repository generation completed");

            Console.WriteLine("Verifying file does not processed already");
            if (filesFoundInDirectory.Count() > 0)
            {
                using (var db = DbRepo.GetDb())
                {
                    var fileStatusRepo = db.GetAllFileStatus(_fileRepo, _folderPath);                 
                    _fileRepo = _fileRepo.Where(f => !fileStatusRepo.Any(t => t.FileName == f.FileName)).ToList();
                }
                Console.WriteLine("File verification completed");

                if (_fileRepo.Count() <= 0)
                {
                    Console.WriteLine("!!!!All files are already had been processed!!!!");
                }
                else
                {
                    Console.WriteLine("File processing started!");
                    ProcessFile();
                }

            }
            else
            {
                Console.WriteLine("No file available to process in given directory");
            }

            //If reading file falied add into error folder

        }
        
        /// <summary>
        /// This method will process all files
        /// Reads single line from the given file
        /// and inserts into details table
        /// also updates the status for fileread in filereadstatus table
        /// </summary>
        private void ProcessFile()
        {
            var counter = 1;
            foreach (var file in _fileRepo)
            {
                Console.WriteLine("\n");
                Console.WriteLine("{0}. Processing file {1}",counter,file.FileName);
                file.Status = FileProcessStatus.Progress;
                var lines = File.ReadAllLines(file.FilePath, System.Text.Encoding.Default);
                
                if (lines.Length <= 0)
                {
                    file.Status = FileProcessStatus.Error;
                    throw new Exception("No data found in file "+file);
                }

                var detailModel = new DetailModel();              
                
                using (IDbHandler db=DbRepo.GetDb())
                {
                    try
                    {
                       // lines=_fieldValidator.GetValidLines(lines.ToList());
                        db.AddDetails(lines);
                        file.Status = FileProcessStatus.Success;
                    }
                    catch(Exception ex)
                    {
                        try
                        {
                            // To copy a file to another location and 
                            // overwrite the destination file if it already exists.
                            System.IO.File.Copy(file.FilePath, _errorFolderPath+"\\"+file.FileName, true);
                            Console.WriteLine("!!!!!!{0}. Error in processing file {1}, File has been moved to error folder", counter, file.FileName);
                            Console.WriteLine("Error is : {0}",ex.Message);
                        }
                        catch
                        {
                            Console.WriteLine("!!!!!!{0}. Error in processing file {1}, we tried to copy file into error folder but something got wrong and we are not able to copy {2} into error folder, please manually check this file.", counter, file.FileName,file.FileName);
                        }
                        file.Status = FileProcessStatus.Error;
                    }
                   
                    db.AddFileStatus(file);
                    if (_isDeleteFile)
                    {                       
                        File.Delete(file.FilePath);
                        Console.WriteLine("File delete command is enabled, file {0} deleted successfully", file.FileName);
                    }
                }

                Console.WriteLine("{0}. File processing completed for file {1}", counter, file.FileName);
                counter++;
            }
        }
    }
}
