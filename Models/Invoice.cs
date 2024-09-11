using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_Laundry_Management_System.Models
{
    public class Invoice
    {
        public int InvoiceId { get; set; }
        public string Customer {  get; set; }
        public string Type { get; set; }
        public DateTime Created { get; set; }
        public string Code { get; set; }
        public decimal Amount { get; set; }
    }
}