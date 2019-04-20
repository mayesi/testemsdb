using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testemsdb
{
    public class AddressRecord
    {
        public int HouseNumber { get; set; }
        public string StreetName { get; set; }
        public string StreetSuffix { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string Province { get; set; }

        // default constructor
        public AddressRecord()
        {
            HouseNumber = 0;
            StreetName = "";
            StreetSuffix = "";
            AddressLine2 = "";
            City = "";
            Province = "";
        }

        // alternate constructor
        public AddressRecord(int houseNum, string street, string suffix, string addrLine2, string city, string province)
        {
            HouseNumber = houseNum;
            StreetName = street;
            StreetSuffix = suffix;
            AddressLine2 = addrLine2;
            City = city;
            Province = province;
        }
    }
}
