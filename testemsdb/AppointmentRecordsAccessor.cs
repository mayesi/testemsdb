using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testemsdb
{
    class AppointmentRecordsAccessor : DatabaseAccessor
    {
        // the status values from the database
        const string AVAILABLE = "AVA";
        const string BOOKED = "BOK";
        const string CANCELLED = "CAN";

        //
        //
        public List<AppointmentRecord> GetRecords(string healthcard)
        {
            if (healthcard.Length == 0)
            {
                // throw exception about length being zero
            }
            if (healthcard.Contains(';'))
            {
                // throw exception about invalid characters
            }

            SqlCommand command = new SqlCommand("GetAppointmentFromHealthCard", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@healthCard", healthcard));

            return GetAppointmentInfo(command);

        }

        public List<AppointmentRecord> GetRecords(DateTime date, TimeSpan time)
        {
            SqlCommand command = new SqlCommand("GetAppointmentFromDateTime", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@apptDate", date));
            command.Parameters.Add(new SqlParameter("@apptTime", time));

            return GetAppointmentInfo(command);
        }

        private List<AppointmentRecord> GetAppointmentInfo(SqlCommand command)
        {
            List<AppointmentRecord> records = new List<AppointmentRecord>();

            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    // Get ordinals for the columns required to fill the record
                    int p1Pos = reader.GetOrdinal("Patient_1");
                    int datePos = reader.GetOrdinal("Date");
                    int timePos = reader.GetOrdinal("Time");
                    int stPos = reader.GetOrdinal("Status");

                    if (reader.HasRows)
                    {
                        // Read each row, filling in the patient info   
                        while (reader.Read())
                        {
                            AppointmentRecord rec = new AppointmentRecord();
                            rec.PatientHCN = GetSafeString(reader, p1Pos);
                            rec.AppointmentDate = GetSafeDateTime(reader, datePos);
                            rec.AppointmentTime = GetSafeTimeSpan(reader, timePos);
                            rec.Status = GetSafeStatus(reader, stPos);

                            // Add to list of records
                            records.Add(rec);
                        }
                    }
                } // end using
                connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                // this allows the exception to percolate...
                connection.Close();
            }
            return records;
        }

        public bool InsertNewRecord(AppointmentRecord record)
        {
            SqlCommand command = new SqlCommand("AddAppointment", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@apptDate", record.AppointmentDate));
            command.Parameters.Add(new SqlParameter("@apptTime", record.AppointmentTime));
            command.Parameters.Add(new SqlParameter("@patient", record.PatientHCN));

            return ExecuteNonQueryProcedure(command);
        }

        public bool CancelAppointment(DateTime date, TimeSpan time)
        {
            SqlCommand command = new SqlCommand("CancelAppointmentFromDateTime", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@apptDate", date));
            command.Parameters.Add(new SqlParameter("@apptTime", time));

            return ExecuteNonQueryProcedure(command);
        }

        public bool CancelAppointment(string healthcard)
        {
            SqlCommand command = new SqlCommand("CancelAppointmentFromHealthCard", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@healthCard", healthcard));

            return ExecuteNonQueryProcedure(command);
        }

        public AppointmentRecord.StatusValues GetSafeStatus(SqlDataReader reader, int col)
        {
            string status = GetSafeString(reader, col);
            AppointmentRecord.StatusValues retVal = AppointmentRecord.StatusValues.ERROR;
            if (!string.IsNullOrEmpty(status))
            {
                if (status.Equals(AVAILABLE))
                {
                    retVal = AppointmentRecord.StatusValues.AVAILABLE;
                }
                else if (status.Equals(BOOKED))
                {
                    retVal = AppointmentRecord.StatusValues.BOOKED;
                }
                else if (status.Equals(CANCELLED))
                {
                    retVal = AppointmentRecord.StatusValues.CANCELLED;
                }
            }
            return retVal;
        }
    }
}
