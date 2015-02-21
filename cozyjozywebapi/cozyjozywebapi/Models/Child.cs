using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace cozyjozywebapi.Models
{
    public class Child
    {
        public int Id { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public bool Male { get; set; }

        [JsonIgnore]
        public virtual ICollection<Feedings> Feedings { get; set; }
        [JsonIgnore]
        public virtual ICollection<DiaperChanges> DiaperChanges { get; set; }
        [JsonIgnore]
        public virtual ICollection<Measurement> Measurements { get; set; }
        [JsonIgnore]
        public virtual ICollection<User> Followers { get; set; } 
    }
}