using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Support
{
    /// <summary>
    /// This class is used for logging events in the EMS. It can be used by any module
    /// or class in the EMS.
    /// </summary>
    /// <remarks>
    /// Exceptions can be thrown for basic File IO errors.
    /// </remarks>
    public class Logging
    {
        /// <summary>
        /// Write a message to a log file.
        /// </summary>
        /// <remarks>
        /// This method takes the originating class and method names and event details
        /// and writes that information with a date and time stamp into a log file for  
        /// the date the log event was created.
        /// </remarks>
        /// <param name="className">String: The originating class name</param>
        /// <param name="methodName">String: The originating method name</param>
        /// <param name="eventDetails">String: The event details</param>
        public static void LogMsg(string className, string methodName, string eventDetails)
        {
            // get time, compose full message, write log to file
            DateTime date = DateTime.Now;
            string dateStr = date.ToString("yyyy'-'MM'-'dd HH':'mm':'ss");
            string fullMsg = dateStr + " [" + className + "." + methodName + "] " + eventDetails;
            using (StreamWriter sw = new StreamWriter(SupportConstants.LOG_FILE_PATH, true))
            {
                sw.WriteLine(fullMsg);
                sw.Close();
            }
        }
    }
}
