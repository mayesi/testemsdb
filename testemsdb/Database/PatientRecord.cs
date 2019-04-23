using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demographics;

namespace testemsdb
{
    public class PatientRecord
    {
        public string HealthCardNumber { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public char MiddleInitial { get; set; }
        public DateTime DateOfBirth { get; set; }
        public char Sex { get; set; }
        public string HeadOfHousehold { get; set; }
        public string AreaCode { get; set; }
        public string PhoneNumber { get; set; }
        public AddressRecord Address { get; set; }

        public PatientRecord()
        {
            HealthCardNumber = "";
            LastName = "";
            FirstName = "";
            MiddleInitial = '\0';
            DateOfBirth = DateTime.MaxValue;
            Sex = '\0';
            HeadOfHousehold = "";
            AreaCode = "";
            PhoneNumber = "";
            Address = new AddressRecord();
        }

        public PatientRecord(PatientInfo patient)
        {
            HealthCardNumber = patient.HCN.ToString();
            LastName = patient.LastName;
            FirstName = patient.FirstName;
            MiddleInitial = patient.MInitial;

            try
            {
                DateTime dob = DateTime.Parse(patient.DateBirth);
                DateOfBirth = dob;
            }
            catch (Exception ex)
            {
                DateOfBirth = DateTime.MaxValue;
            }

            Sex = patient.Sex;

            if (HeadOfHousehold != null)
            {
                HeadOfHousehold = patient.HeadOfHouse.ToString();
            }
            else
            {
                HeadOfHousehold = HealthCardNumber;
            }

            // Get the phone number
            string[] split = patient.splitPhoneNum();
            AreaCode = split[0];
            PhoneNumber = split[1];

            // Get the address info
            Address = new AddressRecord();
            Address.HouseNumber = patient.PatientAdress.HouseNumber;
            Address.StreetName = patient.PatientAdress.StreetName;
            Address.StreetSuffix = patient.PatientAdress.AddressSuffix;
            Address.AddressLine2 = patient.PatientAdress.AddressLine2;
            Address.City = patient.PatientAdress.City;
            Address.Province = patient.PatientAdress.StateProvince;
            
        }


        public PatientInfo GetPatientInfo()
        {
            string addressLine1 = Address.HouseNumber + " " + Address.StreetName + " " + Address.StreetSuffix;
            HealthCard hc = new HealthCard(HealthCardNumber);
            HealthCard hoh = new HealthCard(HeadOfHousehold);
            PatientInfo patient;
            if (HeadOfHousehold != HealthCardNumber)
            {
                patient = new PatientInfo(
                    hc,
                    LastName,
                    FirstName,
                    DateOfBirth.ToShortDateString(),
                    Sex,
                    hoh,
                    MiddleInitial);
            }
            else
            {
                string phonenum = AreaCode + PhoneNumber;
                patient = new PatientInfo(
                    hc,
                    LastName,
                    FirstName,
                    DateOfBirth.ToShortDateString(),
                    Sex,
                    addressLine1,
                    Address.AddressLine2,
                    Address.City,
                    Address.Province,
                    phonenum,
                    MiddleInitial);
            }

            return patient;
        }
    }
}
