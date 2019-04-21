/*
File: BuildPatient.cs
Project: EMS-II
Programmer: Caleb Bolsonello
First Version : 2019-04-05
Description : This file holds the Builder class for the PatientInfo class which builds a PatientInfo object if the patient has valid information
*/

using System;

namespace Demographics
{
    #region Builder

    public static class BuildPatient
    {
        //Build Patient
        public static bool buildPatientInfo(String HCNNumber, String newLastName, String newFirstName, String newDateBirth, char newSex, String newAddressLine1, String newAddressLine2, String newCity, String newProvince, String newNumPhone, char newMInitial)
        {
            bool successfulBuild = false;

            //Call all of the methods
            try
            {
                HealthCard HCN = instantiateHCN(HCNNumber);

                PatientInfo patient = spawnPatient(HCN, newLastName, newFirstName, newDateBirth, newSex, newAddressLine1, newAddressLine2, newCity, newProvince, newNumPhone, newMInitial);
            }
            catch (Exception e)
            {
                throw new Exception("There was an issue instantiating PersonInfo. Error: {0}", e);
            }

            return successfulBuild;
        }

        public static bool buildPatientInfo(String HCNNumber, String newLastName, String newFirstName, String newDateBirth, char newSex, HealthCard newHeadOfHouse, char newMInitial)
        {
            bool successfulBuild = false;

            //Call all of the methods
            try
            {
                HealthCard HCN = instantiateHCN(HCNNumber);

                PatientInfo patient = spawnPatient(HCN, newLastName, newFirstName, newDateBirth, newSex, newHeadOfHouse, newMInitial);
            }
            catch (Exception e)
            {
                throw new Exception("There was an issue instantiating PersonInfo. Error: {0}", e);
            }

            return successfulBuild;
        }

        //instantiate HCN
        public static HealthCard instantiateHCN(String HCNNumber)
        {
            try
            {
                HealthCard HCN = new HealthCard(HCNNumber);
                return HCN;
            }
            catch (Exception e)
            {
                throw new Exception("Health card number format is wrong. Error: {0}", e);
            }
        }

        //Validate Address????


        //Instantiate person info
        public static PatientInfo spawnPatient(HealthCard newHCN, String newLastName, String newFirstName, String newDateBirth, char newSex, String newAddressLine1, String newAddressLine2, String newCity, String newProvince, String newNumPhone, char newMInitial)
        {
            PatientInfo patient;

            if (noHOHCheck(newHCN, newLastName, newFirstName, newDateBirth, newSex, newAddressLine1, newAddressLine2, newCity, newProvince, newNumPhone, newMInitial))
            {
                return patient = new PatientInfo(newHCN, newLastName, newFirstName, newDateBirth, newSex, newAddressLine1, newAddressLine2, newCity, newProvince, newNumPhone, newMInitial);
            }

            return null;
        }

        public static PatientInfo spawnPatient(HealthCard newHCN, String newLastName, String newFirstName, String newDateBirth, char newSex, HealthCard newHeadOfHouse, char newMInitial)
        {
            PatientInfo patient;

            if (HOHCheck(newHCN, newLastName, newFirstName, newDateBirth, newSex, newHeadOfHouse, newMInitial))
            {
                return patient = new PatientInfo(newHCN, newLastName, newFirstName, newDateBirth, newSex, newHeadOfHouse, newMInitial);
            }

            return null;
        }

        public static bool noHOHCheck(HealthCard newHCN, String newLastName, String newFirstName, String newDateBirth, char newSex, String newAddressLine1, String newAddressLine2, String newCity, String newProvince, String newNumPhone, char newMInitial)
        {
            bool allValid = false;

            if (newHCN.validateNumber(newHCN.ToString()))
            {
                if (ValidatePatient.validateLastName(newLastName))
                {
                    if (ValidatePatient.validateFirstName(newFirstName))
                    {
                        if (ValidatePatient.validateDateBirth(newDateBirth))
                        {
                            if (ValidatePatient.validateSex(newSex))
                            {
                                if (ValidatePatient.validateAddressLine1(newAddressLine1))
                                {
                                    if ((newAddressLine2 == "") || (ValidatePatient.validateAddressLine2(newAddressLine2)))
                                    {
                                        if (ValidatePatient.validateCity(newCity))
                                        {
                                            if ((newProvince == null) || (newProvince == ""))
                                            {
                                                newProvince = "ON";
                                            }

                                            if (ValidatePatient.validateProvince(newProvince))
                                            {
                                                if (ValidatePatient.validateNumPhone(newNumPhone))
                                                {
                                                    if ((newMInitial == '\0') || (ValidatePatient.validateMInitial(newMInitial)))
                                                    {
                                                        allValid = true;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return allValid;
        }

        public static bool HOHCheck(HealthCard newHCN, String newLastName, String newFirstName, String newDateBirth, char newSex, HealthCard newHeadOfHouse, char newMInitial)
        {
            bool allValid = false;

            if (newHCN.validateNumber(newHCN.ToString()))
            {
                if (ValidatePatient.validateLastName(newLastName))
                {
                    if (ValidatePatient.validateFirstName(newFirstName))
                    {
                        if (ValidatePatient.validateDateBirth(newDateBirth))
                        {
                            if (ValidatePatient.validateSex(newSex))
                            {
                                if (ValidatePatient.validateHeadOfHouse(newHeadOfHouse))
                                {
                                    if ((newMInitial == '\0') || (ValidatePatient.validateMInitial(newMInitial)))
                                    {
                                        allValid = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return allValid;
        }

        #endregion
    }
}