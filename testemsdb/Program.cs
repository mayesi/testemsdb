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
            string testThis = "patient";
            bool result;

            if (testThis == "patient")
            {
                PatientRecordsAccessor pr = new PatientRecordsAccessor();
                PatientRecord rec1 = new PatientRecord();
                rec1.HealthCardNumber = "6408383104";
                rec1.HeadOfHousehold = rec1.HealthCardNumber;
                
                PatientRecord rec2 = new PatientRecord();
                rec2.LastName = "Hernandez";

                List<PatientRecord> list = pr.GetRecords(PatientRecordsAccessor.GETREQUEST.HEALTH_CARD_NUMBER, rec1.HealthCardNumber);

                if (list.Count > 0)
                {
                    Console.WriteLine(list[0].FirstName);
                }
                else
                {
                    Console.WriteLine("Failed.");
                }

                List<PatientRecord> list2 = pr.GetRecords(PatientRecordsAccessor.GETREQUEST.LASTNAME, rec2.LastName);
                if (list2 != null && list2.Count > 0)
                {
                    Console.WriteLine(list2[0].FirstName);
                }
                else
                {
                    Console.WriteLine("Failed.");
                }

                rec1.LastName = "NewLastName";
                result = pr.UpdateRecords(rec1);

                if (result)
                {
                    Console.WriteLine("Update hoh new last name ok.");
                }
                else
                {
                    Console.WriteLine("Update hoh new last name FAILED.");
                }

                rec2.FirstName = "Mary";
                rec2.HealthCardNumber = "1234567890";
                rec2.HeadOfHousehold = "1868176460";
                //result = pr.InsertNewRecord(rec2);

                //if (result)
                //{
                //    Console.WriteLine("Insert non-hoh info ok.");
                //}
                //else
                //{
                //    Console.WriteLine("Insert non-hoh info FAILED.");
                //}

                list = pr.GetRecords(PatientRecordsAccessor.GETREQUEST.HOH_REPORT, "1868176460");
                for(int i = 0; i < list.Count; i++)
                {
                    Console.WriteLine(list[i].LastName);
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
            else if (testThis == "billing")
            {

            }

            
            

            Console.ReadKey();
        }
    }
}
