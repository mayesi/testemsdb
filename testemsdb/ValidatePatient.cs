/*
File: ValidatePatient.cs
Project: EMS-II
Programmer: Caleb Bolsonello
First Version : 2019-04-05
Description : This file holds the ValidatePatient class for the PatientInfo class which validates if the patient has valid information
*/

using System;
using System.Collections.Generic;
using SupportLib;
using System.Text.RegularExpressions;

namespace Demographics
{
    #region ValidatePatient
    public static class ValidatePatient
    {
        //Call all validations
        #region Validation

        /// \method validateLastName
        /// 
        ///Confirms that lastName is a valid value to be entered into the Demographics database 
        public static bool validateLastName(String newLName)
        {
            bool retCode = false;

            if (newLName != "")
            {
                if (newLName.Length <= Globals.maxNameLen)
                {
                    if (Regex.IsMatch(newLName, @"^[a-zA-Z '-]+$"))
                    {
                        retCode = true;
                    }
                    else
                    {
                        retCode = false;
                        //Enter a name containing only alpha characters with no digits or special characters
                        throw new Exception("Last name can only contain alpha characters");
                    }
                }
                else
                {
                    throw new Exception("Last name that is too long");
                }
            }
            else
            {
                throw new Exception("Last name field is empty");
            }

            return retCode;
        }

        /// \method validateFirstName
        /// 
        ///Confirms that firstName is a valid value to be entered into the Demographics database 
        public static bool validateFirstName(String newFName)
        {
            bool retCode = false;

            Regex lastNameCheck = new Regex(@"^[a-zA-Z '-]+$");
            Match lastNameMatch = lastNameCheck.Match(newFName);

            if (newFName != "")
            {
                if (newFName.Length < Globals.maxNameLen)
                {
                    if (lastNameMatch.Success)
                    {
                        retCode = true;
                    }
                    else
                    {
                        retCode = false;
                        //Enter a name containing only alpha characters with no digits or special characters
                        throw new Exception("First name can only contain alpha characters");
                    }
                }
                else
                {
                    throw new Exception("First name that is too short");
                }
            }
            else
            {
                throw new Exception("First name field is empty");
            }

            return retCode;
        }

        /// \method validateMInitial
        /// 
        ///Confirms that mInitial is a valid value to be entered into the Demographics database 
        public static bool validateMInitial(char newMInitial)
        {
            bool retCode = false;

            if (!Utilities.IsNumeric(newMInitial))
            {
                retCode = true;
            }
            else
            {
                throw new Exception("Middle initial can only contain an alpha character");
            }

            return retCode;
        }

        /// \method validateDateBirth
        /// 
        ///Confirms that the Date of Birth is a valid value to be entered into the Demographics database 
        public static bool validateDateBirth(String birthDate)
        {
            bool retCode = false;

            //Make sure the string is not empty
            if (birthDate != "")
            {
                //Remove any / from the string
                if (birthDate.Contains("/"))
                {
                    string[] birthParts = birthDate.Split('/');

                    birthDate = "";
                    foreach (string parts in birthParts)
                    {
                        birthDate += parts;
                    }
                }

                //Remove any - from the string
                if (birthDate.Contains("-"))
                {
                    string[] birthParts = birthDate.Split('-');

                    birthDate = "";
                    foreach (string parts in birthParts)
                    {
                        birthDate += parts;
                    }
                }

                //Make sure the length of the string is correct
                if (birthDate.Length == Globals.dateLength)
                {
                    //Get only the Day from the string
                    string day = birthDate.Substring(0, 2);
                    if (day.Length < Globals.dayFormat)
                    {
                        day = day.Insert(0, "0");
                    }

                    //Get only the Month from the string
                    string month = birthDate.Substring(2, 2);
                    if (month.Length < Globals.monthFormat)
                    {
                        month = month.Insert(0, "0");
                    }

                    //Get only the Year from the string
                    string year = birthDate.Substring(4, 4);

                    //Check if the string is only numeric
                    if (Utilities.IsNumeric(birthDate))
                    {
                        //Check if the day is 
                        int dayVal = int.Parse(day);
                        if ((dayVal <= Globals.maxDay) && (dayVal > Globals.empty))
                        {
                            int monthVal = int.Parse(month);
                            if ((monthVal <= Globals.maxMonth) && (monthVal > Globals.empty))
                            {
                                int yearVal = int.Parse(year);
                                int currentYear = DateTime.Now.Year;
                                if ((yearVal <= currentYear) && (yearVal > Globals.minYear))
                                {
                                    try
                                    {
                                        DateTime DOB = new DateTime(yearVal, monthVal, dayVal);
                                        retCode = true;
                                    }
                                    catch (Exception invalidBirthday)
                                    {
                                        throw new Exception(invalidBirthday.ToString());
                                    }
                                }
                                else
                                {
                                    //Incorrect Year
                                    throw new Exception("Year format is incorrect");
                                }
                            }
                            else
                            {
                                //Incorrect Month
                                throw new Exception("Month format is incorrect");
                            }
                        }
                        else
                        {
                            //Incorrect Day
                            throw new Exception("Day format is incorrect");
                        }
                    }
                    else
                    {
                        //Only digits allowed
                        throw new Exception("Please enter only numeric digits");
                    }
                }
                else
                {
                    //Wrong Format
                    throw new Exception("Birthday is in the incorrect format");
                }
            }
            else
            {
                //Birthdate empty
                throw new Exception("Birthdat field is empty");
            }

            return retCode;
        }

        /// \method validateSex
        /// 
        ///Confirms that sex is a valid value to be entered into the Demographics database 
        public static bool validateSex(char newSex)
        {
            bool retCode = false;

            newSex = char.ToUpper(newSex);
            if ((newSex == 'M') || (newSex == 'F') || (newSex == 'H') || (newSex == 'I'))
            {
                retCode = true;
            }
            else
            {
                throw new Exception("Sex not valid");
            }

            return retCode;
        }

        /// \method validateHeadOfHouse
        /// 
        ///Confirms that headOfHouse is a valid value to be entered into the Demographics database 
        public static bool validateHeadOfHouse(HealthCard newHOH)//////////////////////////////////////////////////////////////////////////////////
        {
            bool retCode = false;

            string newHOHStr = newHOH.ToString();

            retCode = false;

            return retCode;
        }

        /// \method validateAddressLine1
        /// 
        ///Confirms that addressLine1 is a valid value to be entered into the Demographics database 
        //public static bool validateAddressLine1(String newAddr1)
        //{
        //    bool retCode = false;

        //    string[] splitAddress;
        //    splitAddress = newAddr1.Split(' ');

        //    if (splitAddress.Length >= Globals.addressSplitCount)
        //    {
        //        int homeNum;
        //        if (int.TryParse(splitAddress[0], out homeNum))
        //        {
        //            if (Utilities.IsNumeric(splitAddress[1]))
        //            {
        //                retCode = true;
        //            }
        //        }
        //    }

        //    return retCode;
        //}

        public static bool validateAddressLine1(EMSAddress newAddr1)
        {
            bool retCode = false;

            newAddr1.Split();

            int houseNum = newAddr1.HouseNumber;
            string streetName = newAddr1.StreetName;
            string suffix = newAddr1.AddressSuffix;

            if ((newAddr1.AddressLine1.Length > Globals.empty) && (newAddr1.AddressLine1.Length < Globals.maxAddressLen))
            {
                if ((houseNum > Globals.minHouseNum) && (houseNum < Globals.maxHouseNum))
                {
                    if (streetName.Length > Globals.empty)
                    {
                        Regex streetNameCheck = new Regex(@"[a-zA-Z0-9 '-]+$");
                        Match streetNameMatch = streetNameCheck.Match(streetName);
                        if (streetNameMatch.Success)
                        {
                            retCode = true;
                        }
                    }
                }
            }

            return retCode;
        }

        /// \method validateAddressLine2
        /// 
        ///Confirms that addressLine2 is a valid value to be entered into the Demographics database 
        public static bool validateAddressLine2(EMSAddress newAddr2)
        {
            bool retCode = false;

            if (newAddr2.AddressLine2.Length > Globals.maxAddressLen)
            {
                throw new Exception("Address Line 2 is too long");
            }
            else if (newAddr2.AddressLine2.Length < Globals.empty)
            {
                throw new Exception("Address Line 2 is too short");
            }
            else
            {
                retCode = true;
            }

            return retCode;
        }

        /// \method validateCity
        /// 
        ///Confirms that city is a valid value to be entered into the Demographics database 
        public static bool validateCity(String newCity)
        {
            bool retCode = false;

            Regex cityCheck = new Regex(@"^[a-zA-Z '-]+$");
            Match cityMatch = cityCheck.Match(newCity);

            if (newCity.Length <= Globals.maxCityLen)
            {
                if (cityMatch.Success)
                {
                    retCode = true;
                }
                else
                {
                    retCode = false;
                    //Enter a name containing only alpha characters with no digits or special characters
                    throw new Exception("City name can only contain alpha characters");
                }
            }
            else
            {
                throw new Exception("City name is invalid");
            }

            return retCode;
        }

        /// \method validateProvince
        /// 
        ///Confirms that province is a valid value to be entered into the Demographics database. Supports all 13 
        ///provinces and territories in Canada. If short form is entered, program validates as normal but if full name
        ///is entered, the input is converted to the short form.
        public static bool validateProvince(String newProvince)
        {
            bool retCode = false;

            String[] provinces = File.ReadAllLines(FilePaths.provinceFile);

            //string province = //Call maye's data abstraction for the stored procedure provinceLookup

            List<String> listTest = new List<String>(provinces);

            newProvince = newProvince.ToUpper();
            if (listTest.Contains(newProvince))
            {
                retCode = true;

                if (newProvince == "ONTARIO")
                {
                    newProvince = "ON";
                }
                else if (newProvince == "QUEBEC")
                {
                    newProvince = "QC";
                }
                else if (newProvince == "BRITISH COLUMBIA")
                {
                    newProvince = "BC";
                }
                else if (newProvince == "ALBERTA")
                {
                    newProvince = "AB";
                }
                else if (newProvince == "MANITOBA")
                {
                    newProvince = "MB";
                }
                else if (newProvince == "SASKATCHEWAN")
                {
                    newProvince = "SK";
                }
                else if (newProvince == "NOVA SCOTIA")
                {
                    newProvince = "NS";
                }
                else if (newProvince == "NEW BRUNSWICK")
                {
                    newProvince = "NB";
                }
                else if (newProvince == "NEWFOUNDLAND AND LABRADOR")
                {
                    newProvince = "NL";
                }
                else if (newProvince == "PRINCE EDWARD ISLAND")
                {
                    newProvince = "PE";
                }
                else if (newProvince == "NORTHWEST TERRITORIES")
                {
                    newProvince = "NT";
                }
                else if (newProvince == "NUNAVUT")
                {
                    newProvince = "NU";
                }
                else if (newProvince == "YUKON")
                {
                    newProvince = "YT";
                }
            }
            else
            {
                //This input is not allowed
                throw new Exception("Invalid province");
            }

            return retCode;
        }

        /// \method validateNumPhone
        /// 
        ///Confirms that numPhone is a valid value to be entered into the Demographics database. Supports phone numbers
        ///all together (8149589914), delimited by ' ' (814 958 9914) or delimited by '-' (814-958-9914). Canadian and American
        ///area codes only. Area codes are stored in a file and cross checked with user input.
        public static bool validateNumPhone(String newPhone)
        {
            bool retCode = false;

            String[] areaCodes = File.ReadAllLines(FilePaths.areaCodeFile);
            List<String> listTest = new List<String>(areaCodes);

            string[] reconstructPhone;
            if (newPhone.Contains(" "))
            {
                reconstructPhone = newPhone.Split(' ');

                if (reconstructPhone.Length == Globals.phoneTriplet)
                {
                    newPhone = reconstructPhone[0] + reconstructPhone[1] + reconstructPhone[2];
                }
            }

            string[] numberSplit;
            if (newPhone.Contains("-"))
            {
                numberSplit = newPhone.Split('-');

                if (numberSplit.Length == Globals.phoneTriplet)
                {
                    if (listTest.Contains(numberSplit[0]))
                    {
                        String testLength = numberSplit[1].ToString();
                        if (testLength.Length == Globals.phoneTriplet)
                        {
                            testLength = numberSplit[2].ToString();
                            if (testLength.Length == Globals.phoneQuad)
                            {
                                newPhone = numberSplit[0] + numberSplit[1] + numberSplit[2];
                                retCode = true;
                            }
                        }
                    }
                }
            }
            else
            {
                if (newPhone.Length == Globals.phoneLength)
                {
                    String areaCode = newPhone.Substring(0, 3);
                    if (listTest.Contains(areaCode))
                    {
                        retCode = true;
                    }
                }
                else
                {
                    if (newPhone.Length > Globals.phoneLength)
                    {
                        //Too many numbers
                    }
                    else if (newPhone.Length < Globals.phoneLength)
                    {
                        //Too few numbers
                    }
                }
            }

            if (retCode == false)
            {

                throw new Exception("Incorrect phone format");
            }

            return retCode;
        }
        #endregion
    }
    #endregion
}
