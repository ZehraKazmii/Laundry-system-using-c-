using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_Laundry_Management_System.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerWhatsapp { get; set; }
        public double CustomerDues { get; set; }
        public  string CustomerAddress { get; set; }
        public int CustomerStatus { get; set; }

    }
}