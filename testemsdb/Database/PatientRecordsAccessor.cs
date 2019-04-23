using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace testemsdb
{
    public class PatientRecordsAccessor : DatabaseAccessor
    {
        // this class uses the DatabaseAccessor constructor and protected member variables

        public enum GETREQUEST { HEALTH_CARD_NUMBER, LASTNAME, HOH_REPORT }

        // Get records based on health card number or last name, uses enum
        public List<PatientRecord> GetRecords(GETREQUEST type, string criteria)
        {

            if (criteria.Length == 0)
            {
                // throw exception about length being zero
            }
            if (criteria.Contains(';'))
            {
                // throw exception about invalid characters
            }

            // This class looks at the request type and call the appropriate function based on the request
            // and object type.
            if (type == GETREQUEST.HEALTH_CARD_NUMBER)
            {
                return GetPatientByHCN(criteria);
            }
            else if (type == GETREQUEST.LASTNAME)
            {
                return GetPatientByLastName(criteria);
            }
            else if (type == GETREQUEST.HOH_REPORT)
            {
                return GetHeadOfHouseholdReport(criteria);
            }

            return new List<PatientRecord>();
        }


        // Update patient info, NOT including a change to the health card number, must use the specific method for that
        public bool UpdateRecords(PatientRecord patient)
        {
            bool retVal = false;

            if (string.Equals(patient.HealthCardNumber, patient.HeadOfHousehold))
            {
                // Want to update all fields 
                retVal = InsertOrUpdatePatient(patient, false, true);
            }
            else 
            {
                // Want to update fields except those related to HOH (address and phone number)
                retVal = InsertOrUpdatePatient(patient, true, true);
            }

            return retVal;
        }

        // Insert a new patient into the database, uses the patientrecord object
        public bool InsertNewRecord(PatientRecord patient)
        {
            bool retVal = false;

            // Check if the patient is the hoh
            if (string.Equals(patient.HealthCardNumber, patient.HeadOfHousehold))
            {
                // Want to update all fields 
                retVal = InsertOrUpdatePatient(patient, false, false);
            }
            else
            {
                // Want to insert fields using those related to HOH
                retVal = InsertOrUpdatePatient(patient, true, false);
            }

            return retVal;
        }


        // Update patient using the hoh info or not
        private bool InsertOrUpdatePatient(PatientRecord patient, bool useHOH, bool update)
        {
            SqlCommand command = new SqlCommand();
            if (update)
            {
                if (useHOH)
                {
                    command.CommandText = "UpdateNonHOHPatient";
                }
                else
                {
                    command.CommandText = "UpdateHOHPatient";
                }
            }
            else
            {
                if (useHOH)
                {
                    command.CommandText = "InsertNonHOHPatient";
                }
                else
                {
                    command.CommandText = "InsertHOHPatient";
                }
            }
            
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@hcn", patient.HealthCardNumber));
            command.Parameters.Add(new SqlParameter("@fname", patient.FirstName));
            command.Parameters.Add(new SqlParameter("@lname", patient.LastName));
            command.Parameters.Add(new SqlParameter("@initial", patient.MiddleInitial));
            command.Parameters.Add(new SqlParameter("@dob", patient.DateOfBirth));
            command.Parameters.Add(new SqlParameter("@sex", patient.Sex));
            if (useHOH)
            {
                command.Parameters.Add(new SqlParameter("@hoh", patient.HeadOfHousehold));
            }
            else
            {
                command.Parameters.Add(new SqlParameter("@phonearea", patient.AreaCode));
                command.Parameters.Add(new SqlParameter("@phonenum", patient.PhoneNumber));
                command.Parameters.Add(new SqlParameter("@addr1Street", patient.Address.StreetName));
                command.Parameters.Add(new SqlParameter("@addr1HouseNum", patient.Address.HouseNumber));
                command.Parameters.Add(new SqlParameter("@addr1Suffix", patient.Address.StreetSuffix));
                command.Parameters.Add(new SqlParameter("@addrLine2", patient.Address.AddressLine2));
                command.Parameters.Add(new SqlParameter("@city", patient.Address.City));
                command.Parameters.Add(new SqlParameter("@province", patient.Address.Province));
            }

            int result = ExecuteNonQueryProcedureWithReturn(command);
            bool retVal = false;

            if (result == 0)
            {
                retVal = true;
            }
            return retVal;

        }

        // Update the health card number, need the old number and new number and if the old number is out of date
        // set to false if old number was entered incorrectly
        public bool UpdateHealthCardNumber(string oldNumber, string newNumber, bool outOfDate)
        {
            bool retVal = false;

            SqlCommand command = new SqlCommand("UpdateHealthCard", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@oldNumber", oldNumber));
            command.Parameters.Add(new SqlParameter("@newNumber", newNumber));
            command.Parameters.Add(new SqlParameter("@outofdate", SqlDbType.Bit).Value = outOfDate);

            int result = ExecuteNonQueryProcedureWithReturn(command);
            if (result == 0)
            {
                retVal = true;
            }
            return retVal;
        }


        // Returns a list with a record with the patient info
        private List<PatientRecord> GetPatientByHCN(string hcn)
        {
            // Set up the command
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = "SELECT * FROM PatientByHCN(@hcn);";
            command.Parameters.Add(new SqlParameter("@hcn", hcn));

            return GetPatientInfo(command);
        }

        // NEEDS TO BE UPDATED BASED ON WHAT IS IN THE DATABASE
        private List<PatientRecord> GetPatientByLastName(string lname)
        {
            // Set up the command
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = "SELECT * FROM PatientByLastName(@name);";
            command.Parameters.Add(new SqlParameter("@name", lname));

            return GetPatientInfo(command);           
        }

        private List<PatientRecord> GetHeadOfHouseholdReport(string healthcard)
        {
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = "SELECT * FROM RetrieveHOHReport(@hoh);";
            command.Parameters.Add(new SqlParameter("@hoh", healthcard));

            return GetPatientInfo(command);
        }

        // This takes
        private List<PatientRecord> GetPatientInfo(SqlCommand command)
        {
            List<PatientRecord> records = new List<PatientRecord>();

            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    // Get ordinals for the columns required to fill the record, should i make these all constants
                    int hcnPos = reader.GetOrdinal("Health_Number");
                    int fnPos = reader.GetOrdinal("First_Name");
                    int lnPos = reader.GetOrdinal("Last_Name");
                    int miPos = reader.GetOrdinal("M_Initial");
                    int dobPos = reader.GetOrdinal("Date_Birth");
                    int sxPos = reader.GetOrdinal("Sex_ID");
                    int hohPos = reader.GetOrdinal("Head_Of_Household");
                    int hnumPos = reader.GetOrdinal("House_Number");
                    int strNamePos = reader.GetOrdinal("Street_Name");
                    int suffPos = reader.GetOrdinal("Suffix");
                    int ad2Pos = reader.GetOrdinal("Address_Line_2");
                    int ciPos = reader.GetOrdinal("City_Name");
                    int proPos = reader.GetOrdinal("Province_Name");
                    int acPos = reader.GetOrdinal("Area_Code");
                    int pnPos = reader.GetOrdinal("Number");

                    if (reader.HasRows)
                    {
                        // Read each row, filling in the patient info   
                        while (reader.Read())
                        {
                            PatientRecord rec = new PatientRecord();
                            rec.HealthCardNumber = GetSafeString(reader, hcnPos);
                            rec.FirstName = GetSafeString(reader, fnPos);
                            rec.LastName = GetSafeString(reader, lnPos);
                            rec.MiddleInitial = GetSafeChar(reader, miPos);
                            rec.DateOfBirth = GetSafeDateTime(reader, dobPos);
                            rec.Sex = GetSafeChar(reader, sxPos);
                            rec.HeadOfHousehold = GetSafeString(reader, hohPos);
                            rec.Address.HouseNumber = GetSafeInt(reader, hnumPos);
                            rec.Address.StreetName = GetSafeString(reader, strNamePos);
                            rec.Address.StreetSuffix = GetSafeString(reader, suffPos);
                            rec.Address.City = GetSafeString(reader, ciPos);
                            rec.Address.Province = GetSafeString(reader, proPos);
                            rec.AreaCode = GetSafeString(reader, acPos);
                            rec.PhoneNumber = GetSafeString(reader, pnPos);

                            // Add to list of records
                            records.Add(rec);
                        }
                    }
                } // end using
                connection.Close();
            }
            catch (Exception e)
            {
                connection.Close();
                Console.WriteLine(e.Message);
            }

            return records;
        }


        // Checks if the health card number is registered to a patient in the database. If the health
        // card number is in the database it returns true.
        public bool IsHealthcardRegistered(string healthcard)
        {
            bool retVal = false;
            if (healthcard.Length == 0)
            {
                // throw exception about length being zero
            }
            if (healthcard.Contains(';'))
            {
                // throw exception about invalid characters
            }

            SqlCommand command = new SqlCommand("checkForHealthcard", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@healthCard", healthcard));

            int result = ExecuteScalarFunction(command);

            if (result == 0)
            {
                retVal = true;
            }

            return retVal;
        }


        // This method checks the database for the area code. if the area code is in the database
        // it returns true.
        public bool IsAreaCodeValid(string areaCode)
        {
            bool retVal = false;

            if (areaCode.Length == 0)
            {
                // throw exception about length being zero
            }
            if (areaCode.Contains(';'))
            {
                // throw exception about invalid characters
            }

            SqlCommand command = new SqlCommand("areaCodeLookup", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@areaCode", areaCode));

            int result = ExecuteNonQueryProcedureWithReturn(command);

            if (result > 0)
            {
                retVal = true;
            }

            return retVal;
        }

        // checks if the province is valid, also returns true for the full province name
        public bool IsProvinceCodeValid(string province)
        {
            bool retVal = false;

            SqlCommand command = new SqlCommand("provinceLookup", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@searchProv", province));

            int result = ExecuteNonQueryProcedureWithReturn(command);

            if (result == 0)
            {
                retVal = true;
            }

            return retVal;
        }

    } // end PatientRecordsAccessor class



}