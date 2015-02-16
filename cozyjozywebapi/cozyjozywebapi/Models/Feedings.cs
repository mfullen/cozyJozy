using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace cozyjozywebapi.Models
{
    public enum Breast
    {
        Left,
        Right
    }
    public class Feedings
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Breast? Breast { get; set; }
        public double? Amount { get; set; }
        public DateTime DateReported { get; set; }
        public int ChildId { get; set; }
        [ForeignKey("Id")]
        [JsonIgnore]
        public virtual Child Child { get; set; }
    }
}