using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace cozyjozywebapi.Models
{
    public class DiaperChanges
    {
        public int Id { get; set; }
        public DateTime OccurredOn { get; set; }
        public string Notes { get; set; }
        public bool Urine { get; set; }
        public bool Stool { get; set; }
        public int ChildId { get; set; }
        [JsonIgnore]
        public virtual Child Child { get; set; }
    }
}