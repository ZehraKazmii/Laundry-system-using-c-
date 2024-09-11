using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Online_Laundry_Management_System.Models
{
    public class Garments
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Rate { get; set; }

        public decimal Weight { get; set; }
        public int Status { get; set; }
    }
}