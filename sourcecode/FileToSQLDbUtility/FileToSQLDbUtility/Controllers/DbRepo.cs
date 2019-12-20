using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileToSQLDbUtility.Controllers
{
    public static class DbRepo
    {
        public static IDbHandler GetDb()
        {
            var dbType = System.Configuration.ConfigurationManager.AppSettings["DbType"];            
            switch (dbType)
            {
                case "1":
                    return new MySqlDbHandler();                    
                case "2":
                    return new MSSqlDbHandler();                    
                case "3":
                    break;

            }
            return null;
        }
    }
}
