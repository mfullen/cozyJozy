using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace cozyjozywebapi.Models
{
    public class Measurement
    {
        public int Id { get; set; }
        public double? Height { get; set; }
        public double? Weight { get; set; }
        public double? HeadCircumference { get; set; }
        public DateTime DateRecorded { get; set; }
        public int ChildId { get; set; }
        [JsonIgnore]
        public virtual Child Child { get; set; }
    }
}