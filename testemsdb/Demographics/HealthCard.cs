/*
File: HealthCard.cs
Project: EMS-II
Programmer: Caleb Bolsonello
First Version : 2019-04-05
Description : This file holds the HealthCard class which holds information on a Health Card. This class is used in the PatientInfo class.
*/

using System;
using System.Collections.Generic;
using testemsdb;

namespace Demographics
{
    /// \class HealthCard
    /// 
    /// The HealthCard class holds a healthcard number as well as the validation required for a healthcard number.
    public class HealthCard
    {
        String healthCardNum;

        /// \breif
        /// This Ctor allows a new healthcard number to be instantiated
        ///
        public HealthCard(String newNumber)
        {
            if (validateNumber(newNumber))
            {
                healthCardNum = newNumber;
            }
        }

        /// \method getHealthCardNum
        /// 
        /// \return Returns string formatted of all class info
        ///This method returns the value of number in a formatted string form
        public override string ToString()
        {
            string retString = "";

            if (healthCardNum != null)
            {
                string[] HCNValue = healthCardNum.Split(':');

                retString = HCNValue[0];
            }

            return retString;
        }

        /// \method getHealthCardNum
        /// 
        ///This method returns the value of number so that anything outside the class can access the value of the variable
        private String getHealthCardNum()
        {
            return healthCardNum;
        }

        /// \method setHealthCardNum
        /// \param <b>newNumber</b> - String. Holds the new number that will be set as the number
        /// 
        /// \return Returns true if the number was set false otherwise
        /// 
        ///Sets the value of number to whatever the user enters providing it is a valid value
        private bool setHealthCardNum(String newNumber)
        {
            bool retCode = false;

            if (validateNumber(newNumber))
            {
                healthCardNum = newNumber;
                retCode = true;
            }

            return retCode;
        }

        /// \method validateNumber
        /// 
        /// \param void
        /// 
        /// \return void
        /// 
        ///Confirms that HCN is a valid value to be entered into the Demographics database
        public bool validateNumber(String newHCN)
        {
            bool retCode = false;

            PatientRecordsAccessor DAL = new PatientRecordsAccessor();

            List<PatientRecord> foundPatients = DAL.GetRecords(PatientRecordsAccessor.GETREQUEST.HEALTH_CARD_NUMBER, newHCN);

            if (foundPatients.Count > 0)
            {
                retCode = true;
            }

            return retCode;
        }

    }
}
