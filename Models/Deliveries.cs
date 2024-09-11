using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_Laundry_Management_System.Models
{
    public class Deliveries
    {
        public int Id { get; set; }
        public string Customer { get; set; }
        public string Invoice { get; set; }
        public string Status { get; set; }
    }
}