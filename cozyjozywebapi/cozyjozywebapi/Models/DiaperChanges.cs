using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cozyjozywebapi.Models
{
    public class DiaperChanges
    {
        public int Id { get; set; }
        public DateTime OccurredOn { get; set; }
        public string Notes { get; set; }
        public bool Urine { get; set; }
        public bool Stool { get; set; }
    }
}