using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsloBysykkel.Models
{
    public class Availability
    {
        public int Bikes { get; set; }
        public int Locks { get; set; }
        public bool Overflow_Capacity { get; set; }
        public DateTime UpdatedAt { get; set; }
        public decimal RefreshRate { get; set; }
    }
}
