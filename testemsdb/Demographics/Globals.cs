using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demographics
{
    public class Globals
    {
        //Name
        public const int maxNameLen = 35;
        
        //Phone
        public const int phoneTriplet = 3;
        public const int phoneQuad = 4;
        public const int phoneLength = 10;

        //HCN
        public const int HCNLength = 12;

        //Address
        public const int maxAddressLen = 125;
        public const int maxCityLen = 40;

        public const int maxHouseNum = 999999999;
        public const int minHouseNum = 0;

        //Date
        public const int dateLength = 8;
        public const int maxDay = 31;
        public const int maxMonth = 12;
        public const int minYear = 1900;
        public const int dayFormat = 2;
        public const int monthFormat = 2;

        //Misc
        public const int empty = 0;
    }
}
