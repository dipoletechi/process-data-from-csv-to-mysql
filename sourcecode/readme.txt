IDE used= Visual Studio 2017
Programming language: C#
Database connectivity method: ADO.Net
Operating Sytem: Windows 10

1) For My sql Db, first of all run database scripts. All script having prefix MYSQL are the tables for mysql
   For MSsql Db, first of all run database scripts.  All script having prefix MSSQL are the tables for MSSQL


2) In app.config you can change folder path from where data need to be read by setting value of Key "FolderPath"

3) Setup your datase base server credentials
   ServerName= Name of server
   Database Name= Name of database
   USerName= Username for accessing database
   Password= Password for accessing database
   
   Generally for my sql
   Username= root
   Password= root or empty
 
   For MSSQL
	 If   Username & password is empty system will use Integrated security true



3) How to change database type?
   In app.config just change DbType flag

    <!--Mysql-1
    MS SQL-2
    Elastic-3-->


4) Errorfolder: all the files which are not processed will be move to error folder.
                If error folder value in app config is empty, system will create own error folder in it is parent directory
 
   

In Release/Debug mode configuration file app.config name will be "FileToSQLDbUtility.exe"
You can find release or debug folder in bin


5) You can set tables name in app-table.config file

detailstablename- this contains the detail table name where data will be store from files after reading
filereadstatustablename- This contains the file read status table name so that this should not be read again

6)You can enable delete file after processing the file by setting the flag  IsDeleteFileAfterProcessed value to 1 in app.config file

7) you can add fields for checking againest the resctritive keywords by doing comma seperate in app-table.config file in "fieldtobevalidate"
e.g.
You want to validate field 2,5,7
than it should be set like that
<add key="fieldtobevalidate" value="2,5,7"/>

Note: column couting is starting from 1

8) You can add restricted words in app-table.config file by comma seperate in "restrictedwords"

e.g.
  <add key="restrictedwords" value="john,paul,telecom"/>



9) In app-table.config file you can add you columns name e.g.

<add key="columnsname" value="Field1,Field2,Field3,Field4,Field5,Field6,Field7,Field8,Field9,Field10,Field12,Field13"/>