using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testemsdb
{

    
    // This class is to be used for billing one person for one appointment
    class BillingRecordOld
    {
        public List<Service> Services;
        public string HealthCardNumber { get; set; }    // the health card number
        public char Gender { get; set; }        // char gender f or m
        public int Appointment { get; set; }    // Appointment number from the database

        // Default constructor
        public BillingRecordOld()
        {
            HealthCardNumber = "";
            Services = new List<Service>();
            Gender = 'n';
        }

        // Other constructor
        public BillingRecordOld(string hcn, string code, string fee, char gender, string status)
        {
            HealthCardNumber = hcn;
            Gender = gender;
            Service s = new Service(code, fee, status);
            Services.Add(s);
        }

        // Can use this as an alternate way to add a service to the Services list
        public void AddService(string code, string status)
        {
            Services.Add(new Service(code, status));
        }

        // Can use this as an alternate way to add a service to the Services list
        public void AddService(string code, string fee, string status)
        {
            Services.Add(new Service(code, fee, status));
        }

        // Can use this as an alternate way to add a service to the Services list
        public void AddService(int id, string code, string fee, string status)
        {
            Services.Add(new Service(id, code, fee, status));
        }

        // Ca
        public List<Service> GetAllServices()
        {
            return Services;
        }
    }


}
