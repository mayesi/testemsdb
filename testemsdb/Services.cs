using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testemsdb
{
    class Service
    {
        public int Id { get; set; }         // the id from the database
        public string Code { get; set; }    // the fee code
        public string Status { get; set; }  // the status of the order
        public string Fee { get; set; }     // the fee

        public Service()
        {
            Id = 0;
            Code = "";
            Fee = "";
            Status = "";
        }

        public Service(string code, string status)
        {
            Id = 0;
            Code = code;
            Fee = "";
            Status = status;
        }

        public Service(string code, string fee, string status)
        {
            Id = 0;
            Code = code;
            Fee = fee;
            Status = status;
        }

        public Service(int id, string code, string fee, string status)
        {
            Id = id;
            Code = code;
            Fee = fee;
            Status = status;
        }
    }
}
