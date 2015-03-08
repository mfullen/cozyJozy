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

            var recentAmountPerFeed =
                _context.Feedings.Where(c => c.ChildId == childId).Where(d => d.EndTime > recent).Sum(s => s.Amount);

            var totalAmountPerFeed = _context.Feedings.Where(c => c.ChildId == childId).Sum(s => s.Amount);
            var stats = new DashboardStatistics
            {
                DateOfBirth = _context.Child.Where(x => x.Id == childId).Select(x => x.DateOfBirth).FirstOrDefault(),
                LastFeeding = _context.Feedings.OrderByDescending(o => o.EndTime).Where(c => c.ChildId == childId).Take(1).First().EndTime,
                LastDiaperChange = _context.DiaperChanges.OrderByDescending(o => o.OccurredOn).Take(1).First().OccurredOn,
                TotalFeedings = _context.Feedings.Count(c => c.ChildId == childId),
                TotalDiaperChanges = _context.DiaperChanges.Count(c => c.ChildId == childId),
                ChildId = childId,
                NumberOfRecentDiaperChanges = _context.DiaperChanges.Where(c=>c.ChildId == childId).Count(d => d.OccurredOn > recent),
                NumberOfRecentFeedings = _context.Feedings.Where(c => c.ChildId == childId).Count(d => d.EndTime > recent),
                RecentAmountPerFeed = recentAmountPerFeed != null ? recentAmountPerFeed.Value / _context.Feedings.Where(c => c.ChildId == childId).Count(d => d.EndTime > recent) : 0,
                TotalAmountPerFeed = totalAmountPerFeed != null ? totalAmountPerFeed.Value / _context.Feedings.Count(c => c.ChildId == childId) : 0,
                TotalRecentAmount = recentAmountPerFeed != null ? recentAmountPerFeed.Value : 0
            };


            return Ok(stats);
        }
    }
}
