// FILE 			: Billing.cs
// PROJECT          : INFO2180 EMS Solution
// PROGRAMMER 		: Brendan Brading, Object Orienteers
// FIRST VERSION 	: December 7th 2018
// DESCRIPTION 	    : Contains the billing logic for the ems solution
using System;
using System.IO;
using Support;
using SchedulingLib;
using testemsdb;

namespace billing
{
    /// <summary>
    /// This class handles billing related functions in the EMS, such as producing
    /// database records and the UI. In particular, it interacts with the Menu and 
    /// Database classes. Exceptions are thrown for file IO, database, and parameter
    /// errors.
    /// </summary>
    public class Billing
    {
        // Private Data Members

        private string Month { get; set; } /// The month for the record to be kept in 
        // For Billing File
        private string DateOfAppointment { get; set; } /// The date of the appointment
        private string HealthCardNumber { get; set; } /// the HCN of the patient
        private char Gender { get; set; } /// this contains the persons gender
        private string BillingCode { get; set; } /// This contains the billing code for the visit
        private string Fee { get; set; } /// This will contain the fee for the visit

        //For Reconcile File
        private string Response { get; set; } /// This will contain the response for the response code for this billing entry


        /// <summary>
        /// This method adds a billing code to a record and stores that record in
        /// the Billing database.
        /// </summary>
        /// <remarks>
        /// This method gets the billing information from the patient database needed,
        /// and the fee from the master file associated with that code. It then adds
        /// that record into a billing records database.
        /// </remarks>
        /// <param name="Date"> visit date </param>
        /// <param name="Code"> billing code </param>
        /// <param name="HCN"> health card number </param>
        public static bool AddBillingCode(Appointment apt, string[] codes1, string[] codes2)
        {


            BillingRecordsAccessor dal = new BillingRecordsAccessor();
            dal.Connect("ems_general", "qwerty");
            string money = "";

            try
            {
                //This is to be comented out if the date arrives in the correct format, otherwise use it
                string day = apt.DateTime.Day.ToString();
                string month = apt.DateTime.Month.ToString();
                string year = apt.DateTime.Year.ToString();
                if (apt.DateTime.Day.ToString().Length < 2)
                {
                    day = "0" + day;
                }
                if (apt.DateTime.Month.ToString().Length < 2)
                {
                    month = "0" + month;
                }
                if (apt.DateTime.Year.ToString().Length < 2)
                {
                    year = "0" + year;
                }

                string Date = year + month + day;

                //Now get the info from the appointment object,
                string Gender = apt.PatientHCN.Sex.ToString();

                foreach (string code in codes1)
                {
                    // here we search the database for the code
                    if (dal.CheckBillingCode(code))
                    {
                        money = dal.SearchBillingCode(code);
                    }
                    else
                    {
                        throw new ArgumentException(string.Format("Invalid Billing Code: {0}", code));
                    }

                    Billing BillingEntry = new Billing(month, Date, apt.PatientHCN.HCN.ToString(), char.Parse(Gender), code, money);
                    BillingEntry.GenerateMonthlyBillRecord();
                    
                }
                foreach (string code in codes2)
                {
                    // here we search the database for the code
                    if (dal.CheckBillingCode(code))
                    {
                        money = dal.SearchBillingCode(code);
                    }
                    else
                    {
                        throw new ArgumentException(string.Format("Invalid Billing Code: {0}", code));
                    }

                    Billing BillingEntry = new Billing(month, Date, apt.PatientHCN.HCN.ToString(), char.Parse(Gender), code, money);
                    BillingEntry.GenerateMonthlyBillRecord();
                }
            }
            catch (ArgumentException e)
            {
                throw e;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            throw new BillingRecallException("Flag For Recall?");
        }

        public static bool AddBillingCode(Appointment apt, string[] codes1)
        {

            BillingRecordsAccessor dal = new BillingRecordsAccessor();

            string money = "";

            try
            {
                //This is to be comented out if the date arrives in the correct format, otherwise use it
                string day = apt.DateTime.Day.ToString();
                string month = apt.DateTime.Month.ToString();
                string year = apt.DateTime.Year.ToString();
                if (apt.DateTime.Day.ToString().Length < 2)
                {
                    day = "0" + day;
                }
                if (apt.DateTime.Month.ToString().Length < 2)
                {
                    month = "0" + month;
                }
                if (apt.DateTime.Year.ToString().Length < 2)
                {
                    year = "0" + year;
                }

                string Date = year + month + day;

                //Now get the info from the appointment object,
                string Gender = apt.PatientHCN.Sex.ToString();

                foreach (string code in codes1)
                {
                    // here we search the database for the code
                    if (dal.CheckBillingCode(code))
                    {
                        money = dal.SearchBillingCode(code);
                    }
                    else
                    {
                        throw new ArgumentException(string.Format("Invalid Billing Code: {0}", code));
                    }
                     
                    Billing BillingEntry = new Billing(month, Date, apt.PatientHCN.HCN.ToString(), char.Parse(Gender), code, money);
                    BillingEntry.GenerateMonthlyBillRecord();
                }
            }
            catch (ArgumentException e)
            {
                throw e;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            throw new BillingRecallException("Flag For Recall?");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        public static string getMonthName(string month)
        {
            int mm = int.Parse(month);
            string monthName = "";
            switch (mm)
            {
                case 1:
                    {
                        monthName = "January";
                        break;
                    }
                case 2:
                    {
                        monthName = "Febuary";
                        break;
                    }
                case 3:
                    {
                        monthName = "March";
                        break;
                    }
                case 4:
                    {
                        monthName = "April";
                        break;
                    }
                case 5:
                    {
                        monthName = "May";
                        break;
                    }
                case 6:
                    {
                        monthName = "June";
                        break;
                    }
                case 7:
                    {
                        monthName = "July";
                        break;
                    }
                case 8:
                    {
                        monthName = "August";
                        break;
                    }
                case 9:
                    {
                        monthName = "September";
                        break;
                    }
                case 10:
                    {
                        monthName = "October";
                        break;
                    }
                case 11:
                    {
                        monthName = "November";
                        break;
                    }
                case 12:
                    {
                        monthName = "December";
                        break;
                    }
                default:
                    throw new ArgumentException(string.Format("Invalid month"));
            }
            return monthName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        override public string ToString()
        {
            string thisstring = "";

            thisstring = DateOfAppointment + HealthCardNumber + Gender.ToString() + BillingCode + Fee;

            return thisstring;
        }

        /// <summary>
        /// Take the reconcile file 
        /// </summary>
        public static void LoadReport(string path)
        {
            string[] filecontents;
            filecontents = File.ReadAllLines(path);
            // here we call the update by multiple

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
     //   public static bool ViewReport(int month, int year)
       // {
            //first check to see if there is a summary file 
        //    string[] myreport = { "" };
        //    string thismonth = getMonthName(month.ToString());
        //    string filename = year.ToString() + "_" + thismonth + ".txt";

        //    // first try to open the summary file
        //    try
        //    {
        //        myreport = FileSupport.ReadAllLines(@"c:/ooems/BillingFiles/MonthlySummaries/" + filename);

        //        return BillingSummary.DisplayBillingSummary(myreport);
        //    }
        //    // First catch a directory not found exception this means a directory does not exist for that location
        //    catch (DirectoryNotFoundException)
        //    {
        //        // Create directory.
        //        DirectoryInfo di = Directory.CreateDirectory(@"c:/ooems/BillingFiles/MonthlySummaries/");

        //        try
        //        {
        //            //If by some miracle that creating the directory and this call the file was placed there. 
        //            myreport = FileSupport.ReadAllLines(@"c:/ooems/BillingFiles/MonthlySummaries/" + filename);
        //            return BillingSummary.DisplayBillingSummary(myreport);
        //        }
        //        // created the directory however the file does not exist, then we must read the response file, parse it, save the summary then
        //        // display the summary
        //        catch (FileNotFoundException)
        //        {
        //            try
        //            {
        //                myreport = FileSupport.ReadAllLines(@"c:/ooems/BillingFiles/MonthlyResponse/" + filename);

        //                BillingSummary.CalculateResponse(myreport, filename);
        //            }
        //            catch (FileNotFoundException)
        //            {
        //                Console.WriteLine("No response file found.\n");
        //                return false;
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine(ex.Message);
        //                return false;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex.Message);
        //            return false;
        //        }
        //    }
        //    // Basically does the same thing again as above
        //    catch (FileNotFoundException)
        //    {
        //        try
        //        {
        //            myreport = FileSupport.ReadAllLines(@"c:/ooems/BillingFiles/MonthlyResponse/" + filename);

        //            BillingSummary.CalculateResponse(myreport, filename);
        //        }
        //        catch
        //        {
        //            Console.WriteLine("No response file found.\n");
        //            return false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        return false;
        //    }
        //    //this function always returns true.
        //    return true;
        //}

        /// <summary>
        /// Parses the billing record database, and pulls all of the records into a single monthly bill.
        /// </summary>
        /// <param name="month"> month for the bill to be made</param>
        private void GenerateMonthlyBillRecord()
        {
            // Billing accessor to add line to the 
            BillingRecordsAccessor dal = new BillingRecordsAccessor();
            BillingRecord record = new BillingRecord(); 
            dal.InsertNewRecord(record);
        }

        ///<summary>
        /// Default constructor for the billing class
        ///</summary>
        public Billing()
        {
            Month = "";
            DateOfAppointment = "";
            HealthCardNumber = "";
            Gender = 'U';
            BillingCode = "";
            Fee = "00000000000";
            Response = "";
        }

        /// <summary>
        /// Constructor for generating a billing Object without response code.
        /// </summary>
        /// <param name="BMonth">Billed Month</param>
        /// <param name="DoA">Date of Appointment</param>
        /// <param name="HCN">Health Card Number</param>
        /// <param name="BGender">Gender of the patient</param>
        /// <param name="BC">Billing Code</param>
        /// <param name="BFee">Billed Fee</param>
        public Billing(string BMonth, string DoA, string HCN, char BGender, string BC, string BFee)
        {
            Month = BMonth;
            DateOfAppointment = DoA;
            HealthCardNumber = HCN;
            Gender = BGender;
            BillingCode = BC;
            Fee = BFee;
            Response = "";
        }

        /// <summary>
        /// 
        /// </summary>
        public class BillingRecallException : Exception
        {
            public BillingRecallException(string message)
               : base(message)
            {
            }
        }
    }
}