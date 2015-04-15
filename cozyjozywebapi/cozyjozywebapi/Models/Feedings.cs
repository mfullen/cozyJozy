using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace cozyjozywebapi.Models
{
    public enum DeliveryType
    {
        LeftBreast,
        RightBreast,
        Bottle
    }
    public class Feedings
    {
        [Key]
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DeliveryType? DeliveryType { get; set; }
        public double? Amount { get; set; }
        public DateTime DateReported { get; set; }
        public bool SpitUp { get; set; }
        public string Notes { get; set; }
        public int ChildId { get; set; }
        [JsonIgnore]
        public virtual Child Child { get; set; }
        public string UserId { get; set; }
        [JsonIgnore]
        public virtual User ReportedBy { get; set; }
    }
}