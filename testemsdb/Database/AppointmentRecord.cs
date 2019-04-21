using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testemsdb
{
    public class AppointmentRecord
    {
        public enum StatusValues { AVAILABLE = 0, BOOKED, CANCELLED, ERROR };

        public string PatientHCN { get; set; }          // The first patient booked
        public bool Caregiver { get; set; }             // Indicates whether the patient is the caregiver
        public DateTime AppointmentDate { get; set; }   // The appointment date
        public TimeSpan AppointmentTime { get; set; }   // The time of day for the appointment
        public StatusValues Status { get; set; }        // the appointment status (see enum)
    
        // default constructor
        public AppointmentRecord()
        {
            PatientHCN = "";
            Caregiver = false;
            AppointmentDate = DateTime.MinValue;
            AppointmentTime = TimeSpan.MinValue;
            Status = StatusValues.AVAILABLE;
        }

        // alternate constructor
        public AppointmentRecord(string patient, bool caregiver, DateTime date, TimeSpan time, StatusValues stat)
        {
            PatientHCN = patient;
            Caregiver = caregiver;
            AppointmentDate = date;
            AppointmentTime = time;
            Status = stat;
        }
    }
}
