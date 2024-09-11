using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_Laundry_Management_System.Models
{
    public class InvoiceItems
    {
        public int Id { get; set; }
        public int Customer { get; set; }
        public int Invoice { get; set; }
        public int Garment { get; set; }
        public double Amount { get; set; }

    }
}