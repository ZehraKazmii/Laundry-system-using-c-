using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_Laundry_Management_System.Models
{
    public class Payments
    {
        public int Id { get; set; }
        public string Customer {  get; set; }
        public string Invoice { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; }
        public DateTime PaidAt { get; set; }    
    }
}