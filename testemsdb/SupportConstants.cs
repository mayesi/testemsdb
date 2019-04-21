using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Support
{
    /// <summary>
    /// This class contains constants for use in the Database class.
    /// </summary>
    public class SupportConstants
    {
        public const String DATABASE_FOLDER = @"C:/ooems/Databases";      /// The folder holding the databases
        public const String DATABASE_LIST = @"C:/ooems/DatabaseList.txt"; /// The list of databases
        public const char FIELD_DELIM = '|';                    /// The field delimiter for records
        public const String LOG_FILE_PATH = @"C:/ooems/ems_log.txt";      /// The default file for logs
      
    }
}
