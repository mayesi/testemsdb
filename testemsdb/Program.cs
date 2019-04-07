using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testemsdb
{
    class Program
    {
        static void Main(string[] args)
        {
            string testThis = "appointment";
            bool result;

            if (testThis == "patient")
            {
                PatientRecordsAccessor pr = new PatientRecordsAccessor();
                PatientRecord rec1 = new PatientRecord();
                rec1.HealthCardNumber = "6408383104";
                PatientRecord rec2 = new PatientRecord();
                rec2.LastName = "Hernandez";

                List<PatientRecord> list = pr.GetRecords(PatientRecordsAccessor.REQUESTS.HEALTH_CARD_NUMBER, rec1);

                if (list.Count > 0)
                {
                    Console.WriteLine(list[0].FirstName);
                }
                else
                {
                    Console.WriteLine("Failed.");
                }

                List<PatientRecord> list2 = pr.GetRecords(PatientRecordsAccessor.REQUESTS.LASTNAME, rec2);
                if (list2 != null && list2.Count > 0)
                {
                    Console.WriteLine(list2[0].FirstName);
                }
                else
                {
                    Console.WriteLine("Failed.");
                }
            }
            
            if (testThis == "appointment")
            {
                AppointmentRecordsAccessor ara = new AppointmentRecordsAccessor();
                List<AppointmentRecord> lst = new List<AppointmentRecord>();

                // Add a new appointment
                AppointmentRecord app = new AppointmentRecord();
                app.PatientHCN = "8028261884";
                app.AppointmentDate = new DateTime(2019, 4, 5);
                app.AppointmentTime = new TimeSpan(8, 0, 0);

                result = ara.InsertNewRecord(app);

                if (result)
                {
                    Console.WriteLine("Added record.");
                }
                else
                {
                    Console.WriteLine("Failed.");
                }

                // Cancel appointment
                result = ara.CancelAppointment(app.PatientHCN);
                if (result)
                {
                    Console.WriteLine("Cancelled record.");
                }
                else
                {
                    Console.WriteLine("Failed.");
                }

                // Cancel appointment
                result = ara.CancelAppointment(app.AppointmentDate, app.AppointmentTime);
                if (result)
                {
                    Console.WriteLine("Cancelled record.");
                }
                else
                {
                    Console.WriteLine("Failed.");
                }

                lst = ara.GetRecords(app.PatientHCN);
                if (lst.Count > 0)
                {
                    Console.WriteLine(lst[0].PatientHCN);
                    Console.WriteLine(lst[0].Status);
                }
                else
                {
                    Console.WriteLine("Failed.");
                }
            }

            Console.ReadKey();
        }
    }
}
