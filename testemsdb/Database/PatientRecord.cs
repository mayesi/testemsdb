using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
