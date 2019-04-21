using System;

namespace SupportLib
{
    public static class Utilities
    {
        /*			
		* name    :	IsNumeric
		* desc    : This function checks if the string that was passed in is numeric or not
		* params  :	String testString
		* returns : bool isNumeric. This returns true if the string is all numeric and false otherwise.
		*/
        public static bool IsNumeric(String testString)
        {
            bool isNumeric = true;

            foreach (char character in testString)
            {
                if (!char.IsDigit(character))
                {
                    isNumeric = false;
                }
            }

            return isNumeric;
        }

        /*			
		* name    :	IsNumeric
		* desc    : This function is an overloaded version of the above function that checks if a character is numeric or not
		* params  :	String testString
		* returns : bool isNumeric. This returns true if the string is all numeric and false otherwise.
		*/
        public static bool IsNumeric(char testChar)
        {
            bool isNumeric = true;

            if (!char.IsDigit(testChar))
            {
                isNumeric = false;
            }

            return isNumeric;
        }
    }

    //WILL BE REPLACED
    public static class FilePaths
    {
        public const String provinceFile = (@".\DemographicValidation\Provinces.txt");
        public const String areaCodeFile = (@".\DemographicValidation\Areacodes.txt");
    }
}


