using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cozyjozywebapi.Models.Stats
{
    public class DashboardStatistics
    {
        /// <summary>
        /// Day the child was born
        /// </summary>
        public DateTime DateOfBirth { get; set; }
        /// <summary>
        /// DateTime of the last feeding
        /// </summary>
        public DateTime? LastFeeding { get; set; }
        /// <summary>
        /// DateTime of the last DiaperChange
        /// </summary>
        public DateTime? LastDiaperChange { get; set; }
        /// <summary>
        /// Total Feedings since birth
        /// </summary>
        public int TotalFeedings { get; set; }
        /// <summary>
        /// Total Diaper changes since birth
        /// </summary>
        public int TotalDiaperChanges { get; set; }
        /// <summary>
        /// Number of Feedings since the recent date. Recent date is default 24 hours
        /// </summary>
        public int NumberOfRecentFeedings { get; set; }
        /// <summary>
        /// Number of diaper changes since the recent date. Recent date is default 24 hour period.
        /// </summary>
        public int NumberOfRecentDiaperChanges{ get; set; }
        /// <summary>
        /// The Avergatge amount per feeding in the last recent period.
        /// </summary>
        public double RecentAmountPerFeed { get; set; }
        /// <summary>
        /// The total amount per feed since birth
        /// </summary>
        public double TotalAmountPerFeed { get; set; }
        /// <summary>
        /// The total amount fed in the last recent period.
        /// </summary>
        public double TotalRecentAmount { get; set; }
        public int ChildId { get; set; }
    }
}