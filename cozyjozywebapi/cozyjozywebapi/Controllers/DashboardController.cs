using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using cozyjozywebapi.Entity;
using cozyjozywebapi.Filters;
using cozyjozywebapi.Models.Stats;

namespace cozyjozywebapi.Controllers
{
    [ChildPermissionFilter]
    public class DashboardController : ApiController
    {
        private readonly CozyJozyContext _context = new CozyJozyContext();
        private const string Authorthizedchildren = "authorthizedChildren";

        // GET: api/Dashboard/5
        public IHttpActionResult Get(int childId = 0)
        {
            var authorthizedChildren = HttpContext.Current.Items[Authorthizedchildren] as List<int>;

            if (authorthizedChildren != null && !authorthizedChildren.Contains(childId))
            {
                return BadRequest(ModelState);
            }

            var recent = DateTime.Now.AddDays(-1);

            var filteredFeedings = _context.Feedings.Where(c => c.ChildId == childId);
            var filteredDiapers = _context.DiaperChanges.Where(c => c.ChildId == childId);

            var recentAmountPerFeed = filteredFeedings.Where(d => d.EndTime > recent).Sum(s => s.Amount);

            var totalAmountPerFeed = filteredFeedings.Sum(s => s.Amount);

            var lastFeed =
                filteredFeedings.OrderByDescending(o => o.EndTime);

            var lastDiaper = filteredDiapers.OrderByDescending(o => o.OccurredOn);

            var stats = new DashboardStatistics
            {
                DateOfBirth = _context.Child.Where(x => x.Id == childId).Select(x => x.DateOfBirth).FirstOrDefault(),
                LastFeeding = lastFeed.Any() ? lastFeed.Take(1).First().EndTime : (DateTime?) null,
                LastDiaperChange = lastDiaper.Any() ? lastDiaper.Take(1).First().OccurredOn : (DateTime?)null,
                TotalFeedings = filteredFeedings.Count(c => c.ChildId == childId),
                TotalDiaperChanges = filteredDiapers.Count(c => c.ChildId == childId),
                ChildId = childId,
                NumberOfRecentDiaperChanges = filteredDiapers.Count(d => d.OccurredOn > recent),
                NumberOfRecentFeedings = filteredFeedings.Count(d => d.EndTime > recent),
                RecentAmountPerFeed = recentAmountPerFeed != null ? recentAmountPerFeed.Value / filteredFeedings.Count(d => d.EndTime > recent) : 0,
                TotalAmountPerFeed = totalAmountPerFeed != null ? totalAmountPerFeed.Value / filteredFeedings.Count() : 0,
                TotalRecentAmount = recentAmountPerFeed != null ? recentAmountPerFeed.Value : 0
            };


            return Ok(stats);
        }
    }
}
