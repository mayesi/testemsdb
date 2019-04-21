//// FILE 			: BillingSummary.cs
//// PROJECT          : INFO2180 EMS Solution
//// PROGRAMMER 		: Brendan Brading, Object Orienteers
//// FIRST VERSION 	: December 7th 2018
//// DESCRIPTION 	    : Contains the billing logic for the ems solution

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace testemsdb
//{
//    /// <summary>
//    /// This class produces billing summaries. It uses the Ontario Ministry of Health
//    /// standards for generating billing summaries. It generates these using encounter
//    /// information stored in a database in the EMS. Exceptions are thrown for database,
//    /// file, and parameter errors.
//    /// </summary>
//    public class BillingSummary
//    {
//        //These are the values we are required to keep track of. 
//        private int EncountersBilled { get; set; }          /// sum of encounters that month
//        private double TotalBilledProcedures { get; set; }   /// total billed in dollars
//        private double ReceivedTotal { get; set; }           /// total received in dollars
//        private double ReceivedPercentage { get; set; }      /// (received total)/(total billed)*100
//        private double AverageBilling { get; set; }          /// (received total)/(total encounters billed) in dollars
//        private int FollowUpEncounters { get; set; }        /// number of encounters to follow-up, sum of 'flag encounters for review' and 'contact ministry of health'


//        /// <summary>
//        /// Generates the billing summary file
//        /// </summary>
//        /// <param name="month">the month to summarize</param>
//        private void GenerateBillingSummary(string month)
//        {
//            //generate pretty report


//            string[] Summary = {"\n*************************************************",
//                "Encounters Billed        : " + EncountersBilled,
//                "Total Billed Procedures  : $" + TotalBilledProcedures,
//                "Total Received           : $" + ReceivedTotal,
//                "Received Percentage      : " + ReceivedPercentage + "%",
//                "Average Billed Procedure : $" + AverageBilling,
//                "Follow Up Encounters     : " + FollowUpEncounters,
//                "*************************************************" };

//            //write the report to a summary file so it can be easilly grabbed in future

//            foreach (string element in Summary)
//            {
//                FileSupport.WriteLine(@"c:/ooems/BillingFiles/MonthlySummaries/" + month, element);
//            }
//            //display it
//            DisplayBillingSummary(Summary);
//        }


//        /// <summary>
//        /// Displays the monthly billing summary
//        /// </summary>
//        /// <param name="month">the month to display</param>
//        public static bool DisplayBillingSummary(string[] summary)
//        {
//            // walk through element of strings and display it all
//            foreach (string element in summary)
//            {
//                Console.WriteLine(element);
//            }
//            return true;
//        }


//        /// <summary>
//        /// Default constructor
//        /// </summary>
//        BillingSummary()
//        {
//            EncountersBilled = 0;
//            TotalBilledProcedures = 0.00;
//            ReceivedTotal = 0.00;
//            ReceivedPercentage = 0.00;
//            AverageBilling = 0.00;
//            FollowUpEncounters = 0;
//        }


//        /// <summary>
//        /// Calculates entire response file and returns the results in a string
//        /// </summary>
//        /// <param name="All"> An array of response files</param>
//        /// <returns> results of the calculations</returns>
//        public static bool CalculateResponse(string[] All, string textFile)
//        {
//            BillingSummary thismonths = new BillingSummary();
//            string response = "";
//            string amount = "";
//            string received = "";

//            foreach (string element in All)
//            {
//                //just in cast the file contains a new line with nothing on it
//                if (element == "")
//                {
//                    break;
//                }

//                // keep track of these two, received changed depending
//                response = element.Substring(36);
//                amount = element.Substring(25, 11);

//                if (response == "PAID")
//                {
//                    received = amount;
//                }
//                else
//                {
//                    received = "00000000000";
//                    // The response wasnt declined, but wasnt paid, take note of all relevant information
//                    if (response != "DECL")
//                    {
//                        string date = element.Substring(0, 8);
//                        string HCN = element.Substring(8, 12);
//                        string gender = element.Substring(20, 1);
//                        string code = element.Substring(21, 4);
//                        string fee = element.Substring(25, 11);
//                        fee.Insert(7, ".");
//                        double dFee = double.Parse(fee);
//                        string[] flag = {"\n*************************************************",
//                                        "Date               : "+date,
//                                        "Health Card Number : "+HCN,
//                                        "Gender             : " + gender,
//                                        "Billing Code       : " + code,
//                                        "Fee                : $" + dFee.ToString(),
//                                        "Response Code      : " + response,
//                                        "*************************************************" };
//                        foreach (string flaggedElement in flag)
//                        {
//                            FileSupport.WriteLine(@"c:/ooems/BillingFiles/FlaggedEncounters/" + textFile, flaggedElement);
//                        }
//                    }
//                }

//                // perform calculations now
//                thismonths.ParseCalculatedResponse(amount, received, response);
//            }
//            //generate summary for future use
//            thismonths.GenerateBillingSummary(textFile);

//            return true;
//        }


//        /// <summary>
//        /// Parses out the item from the string to store.
//        /// </summary>
//        /// <param name="allItems">string containing all items</param>
//        /// <param name="field">where to parse the value for a field</param>
//        /// <returns>value of that parsed calculation</returns>
//        public void ParseCalculatedResponse(string money, string received, string response)
//        {
//            // insert the dot at correct location and parse it out as a double
//            double tBilled = double.Parse(money.Insert(7, "."));
//            double tReceived = double.Parse(received.Insert(7, "."));

//            // Increases with each read.
//            this.EncountersBilled++;
//            this.TotalBilledProcedures += tBilled;
//            this.ReceivedTotal += tReceived;

//            // Mathematical calculations on each read
//            this.ReceivedPercentage = (this.ReceivedTotal / this.TotalBilledProcedures) * 100;
//            this.AverageBilling = (this.ReceivedTotal / this.EncountersBilled);

//            if (response != "PAID" && response != "DECL")
//            {
//                this.FollowUpEncounters++;
//            }
//        }
//    }
//}
