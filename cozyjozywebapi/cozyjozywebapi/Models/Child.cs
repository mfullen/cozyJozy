using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cozyjozywebapi.Models
{
    public class Child
    {
        public int Id { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }

        public virtual ICollection<Feedings> Feedings { get; set; }
        public virtual ICollection<DiaperChanges> DiaperChanges { get; set; }
        public virtual ICollection<Measurement> Measurements { get; set; }
        public virtual ICollection<CjUser> Followers { get; set; } 
    }
}