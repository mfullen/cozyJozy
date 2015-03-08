using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cozyjozywebapi.Models.Stats
{
    public class DashboardStatistics
    {
        public DateTime DateOfBirth { get; set; }
        public DateTime LastFeeding { get; set; }
        public DateTime LastDiaperChange { get; set; }
        public int TotalFeedings { get; set; }
        public int TotalDiaperChanges { get; set; }
        public int NumberOfRecentFeedings { get; set; }
        public int NumberOfRecentDiaperChanges{ get; set; }
        public double RecentAmountPerFeed { get; set; }
        public double TotalAmountPerFeed { get; set; }
        public int ChildId { get; set; }
    }
}