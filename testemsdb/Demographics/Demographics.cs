/*
File: Demographics.cs
Project: EMS-II
Programmer: Caleb Bolsonello
First Version : 2019-04-05
Description : This file holds the PatientInfo Class which holds all of the information on the patient and is used to transfer the patient's
              information into the database.
*/

using System;
using System.Collections.Generic;
using testemsdb;

/// \namespace Demographics
/// 
/// The Demographics namespace holds the PatientInfo class which has all validation and information on a patient required for the EMS program.
namespace Demographics
{
    /// \class PatientInfo
    /// 
    /// The PatientInfo class hold all of the required data about a patient. It also provides all of the required validation for each of the datamembers.
    public class PatientInfo
    {

        #region DataMembers

        HealthCard hcn = null;
        string lastName = "";
        string firstName = "";
        char mInitial = '\0';
        String dateBirth;
        char sex = '\0';
        HealthCard headOfHouse = null;
        EMSAddress patientAdress;
        string numPhone = "";

        PatientRecordsAccessor DAL = new PatientRecordsAccessor();

        public HealthCard HCN ///Holds the health card number
		{
            get
            {
                return hcn;
            }
            set
            {
                if (value.validateNumber(value.ToString()))
                {
                    //if (validateHOHPresent()) { } /////////////////////////////////////////////////////// Call Stored Procedure. Interface with Maye's code
                    hcn = value;
                }
            }
        }

        public String LastName ///Holds the last name
		{
            get
            {
                return lastName;
            }
            set
            {
                if (ValidatePatient.validateLastName(value))
                {
                    lastName = value;
                }
            }
        }

        public String FirstName ///Holds the first name
		{
            get
            {
                return firstName;
            }
            set
            {
                if (ValidatePatient.validateFirstName(value))
                {
                    firstName = value;
                }
            }
        }
        public Char MInitial ///Holds the middle initial [NOT MANDATORY]
		{
            get
            {
                return mInitial;
            }
            set
            {
                if (ValidatePatient.validateMInitial(value))
                {
                    mInitial = value;
                }
            }
        }
        public String DateBirth ///Holds the date of birth
		{
            get
            {
                return dateBirth;
            }
            set
            {
                if (ValidatePatient.validateDateBirth(value))
                {
                    dateBirth = value;
                }
            }
        }

        public char Sex ///Holds the sex
		{
            get
            {
                return sex;
            }
            set
            {
                if (ValidatePatient.validateSex(value))
                {
                    sex = value;
                }
            }
        }

        public HealthCard HeadOfHouse ///Holds the head of house's Health Card Number
		{
            get
            {
                return headOfHouse;
            }
            set
            {
                if (ValidatePatient.validateHeadOfHouse(value))
                {
                    headOfHouse = value;
                }
            }
        }

        public EMSAddress PatientAdress ///Holds the address information
		{
            get
            {
                return patientAdress;
            }
            set
            {
                if (ValidatePatient.validateAddressLine1(value))
                {
                    patientAdress = value;
                }
            }
        }

        public String NumPhone ///Holds the phone number. Default value is ON
		{
            get
            {
                return numPhone;
            }
            set
            {
                if (ValidatePatient.validateNumPhone(value))
                {
                    numPhone = value;
                }
            }
        }
        #endregion

        #region Ctors
        /// \breif
        /// This Ctor handles the non headOfHouse field option
        ///
        public PatientInfo(HealthCard newHCN, String newLastName, String newFirstName, String newDateBirth, char newSex, String newAddressLine1, String newAddressLine2, String newCity, String newProvince, String newNumPhone, char newMInitial)
        {
            hcn = newHCN;
            lastName = newLastName;
            firstName = newFirstName;
            dateBirth = newDateBirth;
            sex = newSex;
            patientAdress.AddressLine1 = newAddressLine1;
            patientAdress.AddressLine2 = newAddressLine2;
            patientAdress.City = newCity;

            if (newProvince.Length <= Globals.empty)
            {
                patientAdress.StateProvince = "ON";
            }
            else
            {
                patientAdress.StateProvince = newProvince;
            }

            numPhone = newNumPhone;
            mInitial = newMInitial;
        }

        /// \breif
        /// This Ctor handles the headOfHouse field option
        ///
        public PatientInfo(HealthCard newHCN, String newLastName, String newFirstName, String newDateBirth, char newSex, HealthCard newHeadOfHouse, char newMInitial)
        {
            hcn = newHCN;
            lastName = newLastName;
            firstName = newFirstName;
            dateBirth = newDateBirth;
            sex = newSex;
            newHeadOfHouse = headOfHouse;
            mInitial = newMInitial;
        }
        #endregion

        #region Data Access

        /// \method insert
        /// 
        /// \param PatientInfo patient
        /// 
        /// \return inserted - True if inserted false otherwise
        /// 
        ///This method takes a object of PatientInfo and inserts it into the patient database
        public bool insert(PatientInfo patient)
        {
            bool inserted = false;

            //Create PatientRecord
            PatientRecord insertPatient = new PatientRecord(patient);

            if (DAL.InsertNewRecord(insertPatient))
            {
                inserted = true;
            }

            return inserted;
        }

        /// \method update
        /// 
        /// \param PatientInfo insertPatient
        /// 
        /// \return updated - True if updated false otherwise
        /// 
        ///This method takes a object of PatientInfo and updates it in the patient database
        public bool update(PatientInfo patient)
        {
            bool updated = false;

            PatientRecord updatePatient = new PatientRecord(patient);

            if (DAL.UpdateRecords(updatePatient))
            {
                updated = true;
            }

            return updated;
        }

        /// \method search
        /// 
        /// \param HealthCard searchPatient
        /// 
        /// \return foundPatient - All patients information
        /// 
        ///This method takes a object of PatientInfo and searches for it in the patient database
        public List<PatientInfo> search(HealthCard searchPatient)
        {
            //bool found = false;

            List<PatientRecord> foundPatients = DAL.GetRecords(PatientRecordsAccessor.GETREQUEST.HEALTH_CARD_NUMBER, searchPatient.ToString());

            List<PatientInfo> patientInfoList = new List<PatientInfo>();
            
            foreach (PatientRecord record in foundPatients)
            {
                patientInfoList.Add(record.GetPatientInfo());
            }

            return patientInfoList;
        }

        /// \method search
        /// 
        /// \param string patientLastName
        /// 
        /// \return foundPatient - All patients information
        /// 
        ///This method takes a object of PatientInfo and searches for it in the patient database
        public List<PatientInfo> search(string patientLastName)
        {
            PatientRecord searchPatient = new PatientRecord();
            
            List<PatientRecord> foundPatients = DAL.GetRecords(PatientRecordsAccessor.GETREQUEST.LASTNAME, patientLastName);

            List<PatientInfo> patientInfoList = new List<PatientInfo>();

            foreach (PatientRecord record in foundPatients)
            {
                patientInfoList.Add(record.GetPatientInfo());
            }

            return patientInfoList;
        }

        /// \method retrieveHOHReport
        /// 
        /// \param string HOHNum
        /// 
        /// \return foundHOHPatient - All patient information releaded to the Head of Household
        /// 
        ///This method Retrieves info on all HOH Relationships so it can be displayed to the user
        public List<PatientInfo> retrieveHOHReport(string HOHNum)
        {
            List<PatientRecord> foundHOHPatient = DAL.GetRecords(PatientRecordsAccessor.GETREQUEST.HOH_REPORT, HOHNum);

            List<PatientInfo> patientInfoList = new List<PatientInfo>();

            foreach (PatientRecord record in foundHOHPatient)
            {
                patientInfoList.Add(record.GetPatientInfo());
            }

            return patientInfoList;
        }
        #endregion

        #region Misc

        /// \method ToString
        /// 
        /// \param void
        /// 
        /// \return void
        /// 
        ///This method Converts the object to a formatted string for the database to read
        public override String ToString()
        {
            string hohPlaceholderValue = "";
            string hohPlaceholderKey = "";

            if (headOfHouse != null && !headOfHouse.ToString().Equals(""))
            {
                hohPlaceholderValue = headOfHouse.ToString() + " ";
                hohPlaceholderKey = "headOfHouse ";
            }

            String PersonInfoString = "`" + hcn.ToString() + "` `" + lastName + "` `" + firstName + "` `" + mInitial +
                                    "` `" + dateBirth + "` `" + sex + "` `" + hohPlaceholderValue + "` `" + patientAdress.AddressLine1 +
                                    "` `" + patientAdress.AddressLine2 + "` `" + patientAdress.City + "` `" + patientAdress.StateProvince + "` `" + numPhone +
                                    "` | `HCN` `lastName` `firstName` `mInitial` `dateBirth` `sex` `headOfHouse" +
                                    "` `addressLine1` `addressLine2` `city` `province` `numPhone`";

            return PersonInfoString;
        }

        /// \method splitPhoneNum
        /// 
        /// \param void
        /// 
        /// \return void
        /// 
        ///This method splits and returns the patient's phone number
        public string[] splitPhoneNum()
        {
            string[] phoneInfo = new string[2];

            string phoneNumber = numPhone;

            if (phoneNumber.Contains("-"))
            {
                phoneNumber = phoneNumber.Replace("-", string.Empty);
            }
            else if (phoneNumber.Contains(" "))
            {
                phoneNumber = phoneNumber.Replace(" ", string.Empty);
            }

            phoneInfo[0] = phoneNumber.Substring(0, Globals.phoneTriplet);
            phoneInfo[1] = phoneNumber.Substring(Globals.phoneTriplet, (phoneNumber.Length - Globals.phoneTriplet));

            return phoneInfo;
        }

        #endregion


    }
}