using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testemsdb
{
    public class BillingRecordsAccessor : DatabaseAccessor
    {
        // Use these to specify which search 
        public enum UPDATE_OPTION { APPOINTMENT_ID, ORDER_ID, LINE_ID }


        // Checks the database to see if the passed in value is in there
        public bool CheckBillingCode(string code)
        {
            SqlCommand command = new SqlCommand("CheckBillingCode", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@code", code));

            int result = ExecuteNonQueryProcedureWithReturn(command);

            bool ret = false;
            if (result > 0)
            {
                ret = true;
            }
            return ret;
        }

        // Returns an empty string if the fee code is not valid
        public string SearchBillingCode(string code)
        {
            SqlCommand command = new SqlCommand("GetServiceFee", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@code", code));

            DataTable table = ExecuteQueryProcedure(command);

            string fee = "";
            if (table.Rows.Count > 0)
            {
                try
                {
                    string temp = Convert.ToString(table.Rows[0][0]);
                    fee = temp;
                }
                catch (Exception ex)
                {
                    // write an error
                }
            }
            return fee;
        }

        // Gets records for a specific month
        public List<BillingRecord> GetRecords(int month, int year)
        {
            SqlCommand command = new SqlCommand("GetMonthlyBilling", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@year", year));
            command.Parameters.Add(new SqlParameter("@month", month));

            return GetBillingInfo(command);
        }

        // Gets records for a specific day
        public List<BillingRecord> GetRecords(int day, int month, int year)
        {
            SqlCommand command = new SqlCommand("GetBillingByDay", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@year", year));
            command.Parameters.Add(new SqlParameter("@month", month));
            command.Parameters.Add(new SqlParameter("@day", day));

            return GetBillingInfo(command);
        }

        // Gets records for a specific order number
        public List<BillingRecord> GetRecordsByOrder(int orderId)
        {
            SqlCommand command = new SqlCommand("GetBillingByOrderNumber", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@orderNum", orderId));

            return GetBillingInfo(command);
        }

        // Gets records for a specific appointment number
        public List<BillingRecord> GetRecordsByAppointment(int appointmentId)
        {
            SqlCommand command = new SqlCommand("GetBillingByAppointment", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@apptid", appointmentId));

            return GetBillingInfo(command);
        }


        // Use this to insert a single new billing order 
        public bool InsertNewRecord(BillingRecord record)
        {
            List<BillingRecord> temp = new List<BillingRecord>();
            temp.Add(record);

            bool ret = InsertNewRecord(temp);

            return ret;
        }


        // Use this to insert entirely new multiple billing records that belong to the same person and appointment id
        public bool InsertNewRecord(List<BillingRecord> recordList)
        {
            bool ret = false;
            bool ok = true;

            // Check that the appointment and healthcard number are the same for all records
            if (recordList.Count > 0)   // Multiple records
            {
                string hcn = recordList[0].HealthCardNumber;
                int appt = recordList[0].Appointment;

                if (hcn.Length > 0 && appt > 0)
                {
                    // Check each record in the list
                    for (int i = 1; i < recordList.Count; i++)
                    {
                        if (recordList[i].HealthCardNumber != hcn || recordList[i].Appointment != appt)
                        {
                            ok = false;
                        }
                    }
                }
                else
                {
                    ok = false; // Can also throw a custom exception
                }

                // check if ok, if so, continue
                if (ok)
                {
                    // create the table as a parameter for the stored procedure
                    DataTable myTable = CreateTable();

                    foreach(BillingRecord rec in recordList)
                    {
                        myTable.Rows.Add(rec.ServiceCode, rec.Status);
                    }

                    SqlCommand command = new SqlCommand("InsertBillingOrder", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    SqlParameter tbl = command.Parameters.AddWithValue("@billinglines", myTable);
                    tbl.SqlDbType = SqlDbType.Structured;

                    command.Parameters.Add(new SqlParameter("apptid", appt));

                    int result = ExecuteNonQueryProcedureWithReturn(command);
                    if (result == 0)    // other return values give information but i'm not dealing with that right now
                    {
                        ret = true;
                    }
                }
            }
            
            return ret;
        }

        // Creates a DataTable that matches the parameter required for the insert stored procedure
        private DataTable CreateTable()
        {
            // https://www.c-sharpcorner.com/UploadFile/ff2f08/table-value-parameter-use-with-C-Sharp/
            DataTable dt = new DataTable();
            dt.Columns.Add("Billing_Code", typeof(char[]));
            dt.Columns.Add("Status_ID", typeof(char[]));

            return dt;
        }

        

        // Add more services to an appointment or update an existing record
        public bool UpdateRecord(UPDATE_OPTION option, BillingRecord record)
        {
            bool ret = false;
            if (option == UPDATE_OPTION.APPOINTMENT_ID)
            {
                ret = UpdateByAppointmentId(record);
            }
            else if (option == UPDATE_OPTION.ORDER_ID)
            {
                ret = UpdateByOrderId(record);
            }
            else if (option == UPDATE_OPTION.LINE_ID)
            {
                ret = UpdateByLineId(record);
            }
            return ret;
        }

        private bool UpdateByAppointmentId(BillingRecord record)
        {
            SqlCommand command = new SqlCommand("UpdateBillingOrderByAppointmentId", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@appt", record.Appointment));
            command.Parameters.Add(new SqlParameter("@codeid", record.ServiceCode));
            command.Parameters.Add(new SqlParameter("@statusid", record.Status));

            int result = ExecuteNonQueryProcedureWithReturn(command);

            bool ret = false;
            if (result == 0)
            {
                ret = true;
            }
            return ret;
        }

        private bool UpdateByOrderId(BillingRecord record)
        {
            SqlCommand command = new SqlCommand("UpdateBillingOrderByOrderId", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@orderid", record.OrderId));
            command.Parameters.Add(new SqlParameter("@codeid", record.ServiceCode));
            command.Parameters.Add(new SqlParameter("@statusid", record.Status));

            int result = ExecuteNonQueryProcedureWithReturn(command);

            bool ret = false;
            if (result == 0)
            {
                ret = true;
            }
            return ret;
        }


        private bool UpdateByLineId(BillingRecord record)
        {
            SqlCommand command = new SqlCommand("UpdateBillingLine", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@lineid", record.ServiceId));
            command.Parameters.Add(new SqlParameter("@codeid", record.ServiceCode));
            command.Parameters.Add(new SqlParameter("@statusid", record.Status));

            int result = ExecuteNonQueryProcedureWithReturn(command);

            bool ret = false;
            if (result == 0)
            {
                ret = true;
            }
            return ret;
        }


        private List<BillingRecord> GetBillingInfo(SqlCommand command)
        {
            List<BillingRecord> records = new List<BillingRecord>();

            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    // Get ordinals for the columns required to fill the record
                    int ordid = reader.GetOrdinal("Order_ID");
                    int apptid = reader.GetOrdinal("Appointment_ID");
                    int apptdate = reader.GetOrdinal("ApptDate");
                    int hcn = reader.GetOrdinal("Patient_1");
                    int sex = reader.GetOrdinal("Sex_ID");
                    int lineid = reader.GetOrdinal("Line_ID");
                    int code = reader.GetOrdinal("Billing_Code_ID");
                    int fee = reader.GetOrdinal("Price");
                    int status = reader.GetOrdinal("Status_ID");

                    if (reader.HasRows)
                    {
                        // Read each row filling in the billing information
                        while(reader.Read())
                        {
                            BillingRecord rec = new BillingRecord();
                            rec.OrderId = GetSafeInt(reader, ordid);
                            rec.Appointment = GetSafeInt(reader, apptid);
                            rec.AppointmentDate = GetSafeDateTime(reader, apptdate);
                            rec.HealthCardNumber = GetSafeString(reader, hcn);
                            rec.Gender = GetSafeChar(reader, sex);
                            rec.ServiceId = GetSafeInt(reader, lineid);
                            rec.ServiceCode = GetSafeString(reader, code);
                            rec.Fee = GetSafeString(reader, fee);
                            rec.Status = GetSafeString(reader, status);

                            records.Add(rec);   // Add to the list
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



        // Method to insert all billing codes given a file
        public static void InsertBillingCodes(string file)
        {
            string[] codes = System.IO.File.ReadAllLines(file);
            List<Tuple<string, int>> ts = new List<Tuple<string, int>>();
            int retVal = 0; // Keeps track of how many are successfully inserted into the database
            
            // Insert the values into the list
            foreach (string code in codes)
            {
                string bc = code.Substring(0, 3);
                int money = int.Parse(code.Substring(12));
                ts.Add(new Tuple<string, int>(bc, money));
                retVal++;   // Add up all the pairs to insert
            }

            // Get the connection
            string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["testConn"].ConnectionString;
            SqlConnection connection = new SqlConnection(connStr);
            
            // Items should be inserted into the list now
            foreach (Tuple<string, int> tuple in ts)
            {
                using (var cmd = new SqlCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@code", tuple.Item1);
                    cmd.Parameters.AddWithValue("@money", tuple.Item2);

                    var storedProcResult = cmd.Parameters.AddWithValue("@Ret", SqlDbType.Int);
                    storedProcResult.Direction = ParameterDirection.ReturnValue;

                    cmd.CommandText = "InsertBillingCode";

                    try
                    {
                        // Open the connection and insert
                        cmd.Connection.Open();
                        cmd.ExecuteNonQuery();
                        cmd.Connection.Close();

                        int retcode = 0;                 
                        retcode = Convert.ToInt32(storedProcResult.Value);

                        if (retcode == -1)
                        {
                            Console.WriteLine("Could not insert billing code.");
                        }
                        else if (retcode == 0)
                        {
                            retVal--;   // Decrement because we successfully added an entry
                        }
                    }
                    catch (Exception e)
                    {
                        cmd.Connection.Close();
                        Console.WriteLine(e.Message);
                    }
                } // end using cmd
            }

        }
    }


}
