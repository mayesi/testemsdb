using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Support
{
    /// <summary>
    /// This class provides basic methods to read a line from a text file and write a line to
    /// a text file. 
    /// </summary>
    /// <remarks>
    /// Exceptions are thrown for IO exceptions. For example, if the specified file 
    /// or directory do not exist.
    /// </remarks>
    public class FileSupport
    {
        /// <summary>
        /// Writes a line to a file.
        /// </summary>
        /// <remarks>
        /// This method writes a line to a text file. It can be used by other modules
        /// to write a line of text with the end marked by newline characters to a text file.
        /// If the file has content, it will append the line to the end of the file.
        /// </remarks>
        /// <param name="filepath">the file to write to</param>
        /// <param name="content">the line to write</param>
        /// <returns>true - successfully wrote the line, false - unsuccessful</returns>
        public static bool WriteLine(String filepath, String content)
        {
            using (StreamWriter sw = new StreamWriter(filepath, true))
            {
                sw.WriteLine(content);
                sw.Close();
            }
            return true;
        }


        /// <summary>
        /// Reads all lines from a file into a string array.
        /// </summary>
        /// <remarks>
        /// This method reads all line from a text file into a string[]. It can be used by 
        /// other modules to read lines of text with the end marked by newline characters 
        /// from a text file. It is a wrapper for File.ReadAllLines() (System.IO).
        /// </remarks>
        /// <param name="filename"> the filename to read from</param>
        /// <returns>String - the line of text as a string</returns>
        public static String[] ReadAllLines(String filepath)
        {
            string[] array = File.ReadAllLines(filepath);
            return array;
        }

        /// <summary>
        /// Searches for a line that matches the search term.
        /// </summary>
        /// <remarks>
        /// This method will search through a given file line by line. It will compare the first
        /// characters of that file (number of characters specified by user) to the search term.
        /// It will return the full line of the first match it finds.
        /// </remarks>
        /// <param name="filepath">file path to file to search</param>
        /// <param name="searchTerm">the term to search for</param>
        /// <param name="numBytes">the number of bytes to compare (not including null terminator)</param>
        /// <returns>String - the line or an empty string if the line was not found</returns>
        public static String FindLineByBytes(string filepath, string searchTerm, int numBytes)
        {
            string[] array = File.ReadAllLines(filepath);
            string retStr = "";

            foreach (string line in array)
            {
                if (searchTerm == line.Substring(0, numBytes))
                {
                    retStr = line;
                    break;
                }
            }
            return retStr;
        }
    }
}
