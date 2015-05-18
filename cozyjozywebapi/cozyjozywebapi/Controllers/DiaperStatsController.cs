using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using cozyjozywebapi.Entity;
using cozyjozywebapi.Filters;
using cozyjozywebapi.Infrastructure.Core;

namespace cozyjozywebapi.Controllers
{
    [RoutePrefix("api/diaperstats")]
    [ChildPermissionFilter]
    public class DiaperStatsController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private const string Authorthizedchildren = "authorthizedChildren";

        public DiaperStatsController(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }

        protected bool CanViewChild(int childId)
        {
            var authorthizedChildren = HttpContext.Current.Items[Authorthizedchildren] as List<int>;

            return (authorthizedChildren != null && authorthizedChildren.Contains(childId));
        }

        class ChartResult
        {
            public double? Total { get; set; }
            public double? Pee { get; set; }
            public double? Poop { get; set; }
            public DateTime? Date { get; set; }
        }

        class PieResult
        {
            public string UserName { get; set; }
            public string Title { get; set; }
            public int Amount { get; set; }
        }

        #region Feedings Amount over time
        IEnumerable<ChartResult> GetDiapersPerDay(int childId, DateTime startRange, DateTime endRange)
        {
            return _unitOfWork.DiaperChangesRepository.All()
                 .Where(c => c.ChildId == childId)
                 .Where(s => s.OccurredOn >= startRange)
                 .Where(s => s.OccurredOn <= endRange)
                 .GroupBy(x => DbFunctions.TruncateTime(x.OccurredOn))
                 .Select(x => new ChartResult { Total = x.Count(), Pee = x.Count(c => c.Urine), Poop = x.Count(c => c.Stool), Date = x.Key })
                 .OrderBy(o => o.Date);
        }

        IEnumerable<ChartResult> GetDiapersPerMonth(int childId, DateTime startRange, DateTime endRange)
        {
            return _unitOfWork.DiaperChangesRepository.All()
                 .Where(c => c.ChildId == childId)
                 .Where(s => s.OccurredOn >= startRange)
                 .Where(s => s.OccurredOn <= endRange)
                 .GroupBy(x => new { x.OccurredOn.Month, x.OccurredOn.Year })
                 .Select(x => new ChartResult { Total = x.Count(), Pee = x.Count(c => c.Urine), Poop = x.Count(c => c.Stool), Date = DbFunctions.CreateDateTime(x.Key.Year, x.Key.Month, 1, 0, 0, 0) })
                 .OrderBy(o => o.Date);
        }

        protected IHttpActionResult GetDiaperOverTime(int childId, DateTime? startRange, DateTime? endRange)
        {
            if (!CanViewChild(childId))
            {
                return BadRequest(ModelState);
            }
            var child = _unitOfWork.ChildRepository.Where(c => c.Id == childId).FirstOrDefault();

            if (child == null)
            {
                return BadRequest("Child not found");
            }

            if (startRange == null)
            {
                startRange = child.DateOfBirth;
            }

            if (endRange == null)
            {
                endRange = DateTime.Now;
            }

            var duration = (endRange.Value.Date - startRange.Value.Date).TotalDays;



            var feedingsQuery = duration >= 30 ?
                  GetDiapersPerMonth(childId, startRange.Value, endRange.Value)
                : GetDiapersPerDay(childId, startRange.Value, endRange.Value);

            return Ok(feedingsQuery.ToList());
        }
        #endregion

        [HttpGet]
        [Route("birth/diaperovertime")]
        public IHttpActionResult GetOverTimeSinceBirth(int childId)
        {
            //Null StartRange = Child DOB
            //Null EndRange = Now
            return GetDiaperOverTime(childId, null, null);
        }

        [HttpGet]
        [Route("week/diaperovertime")]
        public IHttpActionResult GetOverTimeWeek(int childId)
        {
            var end = DateTime.Now;
            var start = end.AddDays(-7);
            return GetDiaperOverTime(childId, start, end);
        }

        [HttpGet]
        [Route("month/diaperovertime")]
        public IHttpActionResult GetOverTimeMonth(int childId)
        {
            var end = DateTime.Now;
            var start = end.AddMonths(-1);
            return GetDiaperOverTime(childId, start, end);
        }

        [HttpGet]
        [Route("most/pee")]
        public IHttpActionResult GetUserWhoChangedMostPoops(int childId)
        {
            if (!CanViewChild(childId))
            {
                return BadRequest(ModelState);
            }
            var child = _unitOfWork.ChildRepository.Where(c => c.Id == childId).FirstOrDefault();

            if (child == null)
            {
                return BadRequest("Child not found");
            }

            using (var ctx = new CozyJozyContext())
            {
                var items = ctx.DiaperChanges
              .Where(c => c.ChildId == childId)
              .GroupBy(g => g.ReportedBy)
              .Select(x => new PieResult
              {
                  UserName = x.Key.UserName,
                  Amount = x.Count(c => c.Urine),
                  Title = ctx.ChildPermissions.FirstOrDefault(c => c.IdentityUserId == x.Key.Id).Title.Name
              });

                return Ok(items.ToList());
            }
        }

        [HttpGet]
        [Route("most/poops")]
        public IHttpActionResult GetUserWhoChangedMostPee(int childId)
        {
            if (!CanViewChild(childId))
            {
                return BadRequest(ModelState);
            }
            var child = _unitOfWork.ChildRepository.Where(c => c.Id == childId).FirstOrDefault();

            if (child == null)
            {
                return BadRequest("Child not found");
            }

            using (var ctx = new CozyJozyContext())
            {
                var items = ctx.DiaperChanges
              .Where(c => c.ChildId == childId)
              .GroupBy(g => g.ReportedBy)
              .Select(x => new PieResult
              {
                  UserName = x.Key.UserName,
                  Amount = x.Count(c => c.Stool),
                  Title = ctx.ChildPermissions.FirstOrDefault(c => c.IdentityUserId == x.Key.Id).Title.Name
              });

                return Ok(items.ToList());
            }

        }
    }
}
