using System;
using System.Collections.Generic;
using System.Linq;

namespace FileToSQLDbUtility.Controllers
{
    public class FieldValidator
    {
        string[] _RestrictedWords;
        string[] _FieldsToBeValidate;

        public FieldValidator()
        {
            var restrictedwords =Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["restrictedwords"]);
            if (!string.IsNullOrEmpty(restrictedwords))
            {
                _RestrictedWords = restrictedwords.Split(',');
            }

            var fieldtobevalidate = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["fieldtobevalidate"]);
            if (!string.IsNullOrEmpty(fieldtobevalidate))
            {
                _FieldsToBeValidate = fieldtobevalidate.Split(',');
            }

        }

        public string[] GetValidLines(List<string> lines)
        {
            foreach (var line in lines)
            {
                if (!IsRowValid(line.Split(',')))
                {
                    lines.Remove(line);
                }
            }
            return lines.ToArray();
        }

        private bool IsRowValid(string[] cols)
        {
            var isValid = true;
            var columnsToBeValidate =new List<string>();
            if (_FieldsToBeValidate!=null && _FieldsToBeValidate.Count() > 0)
            {
                foreach (var fieldToBeValidate in _FieldsToBeValidate)
                {                                        
                    columnsToBeValidate.Add(cols[Convert.ToInt32(fieldToBeValidate)-1]);
                }

                foreach (var columnToBeValidate in columnsToBeValidate)
                {
                    if (_RestrictedWords.Any(r=>r.ToLower()==columnToBeValidate.ToLower()))
                    {
                        isValid = false;
                        break;
                    }
                }

            }
            return isValid;
        }
    }
}
