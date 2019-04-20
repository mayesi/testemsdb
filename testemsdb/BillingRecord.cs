using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testemsdb
{
    public class BillingRecord
    {
        public int OrderId { get; set; }                // the order id from the database
        public string HealthCardNumber { get; set; }    // the health card number
        public char Gender { get; set; }                // char gender f or m
        public int Appointment { get; set; }            // Appointment number from the database
        public DateTime AppointmentDate { get; set; }   // The date of the appointment (date only no time)

        // Service information
        public int ServiceId { get; set; }         // the id number from the database
        public string ServiceCode { get; set; }    // the fee code
        public string Status { get; set; }  // the status of the order
        public string Fee { get; set; }     // the fee

        public BillingRecord()
        {
            OrderId = 0;
            HealthCardNumber = "";
            Gender = '\0';
            Appointment = 0;
            AppointmentDate = DateTime.MaxValue;
            ServiceId = 0;
            ServiceCode = "";
            Status = "";
            Fee = "";
        }
    }
}
