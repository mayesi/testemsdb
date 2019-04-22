using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testemsdb
{
    public class DatabaseAccessor
    {
        protected SqlConnection connection = null;

        public bool Connected { get; set; } = false;

        public DatabaseAccessor()
        {
            ////string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["testConn2"].ConnectionString;
            //string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["testConn"].ConnectionString;
            //connection = new SqlConnection(connStr);
        }

        public bool Connect(string username, string password)
        {
            bool validLogin = false;

            string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["emsConnection"].ConnectionString;

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connStr);


            builder.UserID = username;
            builder.Password = password;

            using (var tempConnection = new SqlConnection(builder.ConnectionString))
            {
                try
                {
                    tempConnection.Open();
                    validLogin = true;
                }
                catch (SqlException)
                {
                    validLogin = false;
                }
            }
            
            if (validLogin)
            {
                connection = new SqlConnection(builder.ConnectionString);
            }

            Connected = validLogin;

            return validLogin;
        }


        protected char GetSafeChar(SqlDataReader reader, int col)
        {
            char[] buffer = new char[1];
            if (!reader.IsDBNull(col))
            {
                reader.GetChars(col, 0, buffer, 0, 1);
            }
            else
            {
                buffer[0] = '0';
            }
            return buffer[0];
        }

        protected DateTime GetSafeDateTime(SqlDataReader reader, int col)
        {
            if (!reader.IsDBNull(col))
            {
                return reader.GetDateTime(col);
            }
            return DateTime.MinValue;
        }

        protected TimeSpan GetSafeTimeSpan(SqlDataReader reader, int col)
        {
            if (!reader.IsDBNull(col))
            {
                return reader.GetTimeSpan(col);
            }
            return TimeSpan.MinValue;
        }

        protected int GetSafeInt(SqlDataReader reader, int col)
        {
            if (!reader.IsDBNull(col))
            {
                return reader.GetInt32(col);
            }
            return 0;
        }

        protected string GetSafeString(SqlDataReader reader, int col)
        {
            if (!reader.IsDBNull(col))
            {
                return reader.GetString(col);
            }
            return string.Empty;
        }

        protected bool ExecuteNonQueryProcedure(SqlCommand command)
        {
            bool ret = false;
            try
            {
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
                ret = true;
            }
            catch (Exception e)
            {
                connection.Close();
                Console.WriteLine(e.Message);
            }
            return ret;
        }

        protected int ExecuteNonQueryProcedureWithReturn(SqlCommand command)
        {
            int retVal = -1;

            var retParam = command.Parameters.Add("@ReturnVal", SqlDbType.Int);
            retParam.Direction = ParameterDirection.ReturnValue;

            try
            {
                connection.Open();
                command.ExecuteNonQuery();
                retVal = (int)retParam.Value;
                connection.Close();
            }
            catch (Exception e)
            {
                connection.Close();
                Console.WriteLine(e.Message);
            }

            return retVal;
        }



        // problem with this function, it will return a -1 if anything goes wrong, so returns of -1 
        // are always suspect...
        protected int ExecuteScalarFunction(SqlCommand command)
        {
            int result = -1;
        
            try
            {
                connection.Open();
                result = (int)command.ExecuteScalar();
                connection.Close();
            }
            catch (Exception e)
            {
                connection.Close();
                Console.WriteLine(e.Message);
            }

            return result;
        }


        protected DataTable ExecuteQueryProcedure(SqlCommand command)
        {
            DataTable table = new DataTable();
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    table.Load(reader);
                } // end using
                connection.Close();
            }
            catch (Exception e)
            {
                connection.Close();
                Console.WriteLine(e.Message);
            }
            return table;
        }
    }
}
