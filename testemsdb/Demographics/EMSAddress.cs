/*
File: EMSAddress.cs
Project: EMS-II
Programmer: Caleb Bolsonello
First Version : 2019-04-05
Description : This file holds the EMSAddress class which is a child of the CivicAddress class and holds information on addresses.
*/

using System;
using System.Device.Location;

namespace Demographics
{
    public class EMSAddress : CivicAddress
    {
        //Public Members from CivicAddress
        /*
        public string AddressLine1;
        public string AddressLine2;
        public string City;
        public string Province;
        */

        // Private Members
        private int houseNumber;
        private string streetName;
        private string addressSuffix;

        public int HouseNumber
		{
            get
            {
                return houseNumber;
            }
            set
            {
                houseNumber = value;
            }
        }

        public string StreetName
		{
            get
            {
                return streetName;
            }
            set
            {
                streetName = value;
            }
        }

        public string AddressSuffix
		{
            get
            {
                return addressSuffix;
            }
            set
            {
                addressSuffix = value;
            }
        }

        // Default constructor
        public EMSAddress()
        {
            AddressLine1 = null;
            AddressLine2 = null;
            City = null;
            StateProvince = null;
            HouseNumber = 0;
            StreetName = null;
            AddressSuffix = null;
        }

        // Copy Constructor to be used with getting the information for the head of house.
        public EMSAddress(EMSAddress address)
        {
            AddressLine1 = address.AddressLine1;
            AddressLine2 = address.AddressLine2;
            City = address.City;
            StateProvince = address.StateProvince;
            HouseNumber = address.HouseNumber;
            StreetName = address.StreetName;
            AddressSuffix = address.AddressSuffix;
        }

        // default constructor
        public EMSAddress(string addr1, string addr2, string city, string province)
        {
            AddressLine1 = addr1;
            AddressLine2 = addr2;
            City = city;
            StateProvince = province;
            HouseNumber = 0;
            StreetName = null;
            AddressSuffix = null;
        }


        // Overloaded to string, do with this as you want to.
        public override string ToString()
        {
            return "AddressLine1:{" + AddressLine1 + "} \nAddressLine2:{" + AddressLine2 + "} \nCity:{" + City + "{\n" +
                "Province:{" + StateProvince + "} \nHouseNumber:{" + HouseNumber.ToString() + "} \nStreetName:{" + StreetName + "} " +
                "\nAddressSuffix:" + AddressSuffix + "}";
        }


        // if the addresses equal its good
        public override bool Equals(object obj)
        {
            EMSAddress address = (EMSAddress)obj;
            if (AddressLine1 == address.AddressLine1 && AddressLine2 == address.AddressLine2 && City == address.City && 
                StateProvince == address.StateProvince && HouseNumber == address.HouseNumber &&
                StreetName == address.StreetName && AddressSuffix == address.AddressSuffix)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        // This splits the addressline 1 into a housenum, street name and suffix
        public void Split()
        {
            string[] address = { null };
            address = AddressLine1.Split(' ');
            try
            {
                int.TryParse(address[0], out houseNumber);
            }
            catch (Exception e)
            {
                // Log here
            }
            streetName = address[1];
            addressSuffix = address[2];
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

}
