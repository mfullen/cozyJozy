using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cozyjozywebapi.Models
{
    public class Measurement
    {
        public int Id { get; set; }
        public double? Height { get; set; }
        public double? Weight { get; set; }
    }
}