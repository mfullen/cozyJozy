using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace cozyjozywebapi.Models
{
    public class SleepSession
    {
        [Key]
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Notes { get; set; }
        public string UserId { get; set; }
        [JsonIgnore]
        public virtual User ReportedBy { get; set; }
        public int ChildId { get; set; }
        [JsonIgnore]
        public virtual Child Child { get; set; }
    }
}