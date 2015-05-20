using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using cozyjozywebapi.Filters;
using cozyjozywebapi.Infrastructure.Core;

namespace cozyjozywebapi.Controllers
{
    [RoutePrefix("api/feedingstats")]
    [ChildPermissionFilter]
    public class FeedingStatsController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private const string Authorthizedchildren = "authorthizedChildren";

        public FeedingStatsController(IUnitOfWork uow)
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
            public double? Amount { get; set; }
            public DateTime? Date { get; set; }
        }

        #region Feedings Amount over time
        IEnumerable<ChartResult> GetFeedingsPerDay(int childId, DateTime startRange, DateTime endRange)
        {
            return _unitOfWork.FeedingRepository.All()
                 .Where(c => c.ChildId == childId)
                 .Where(s => s.StartTime >= startRange)
                 .Where(s => s.StartTime <= endRange)
                 .GroupBy(x => DbFunctions.TruncateTime(x.StartTime))
                 .Select(x => new ChartResult { Amount = x.Sum(c => c.Amount), Date = x.Key })
                 .OrderBy(o => o.Date);
        }

        IEnumerable<ChartResult> GetFeedingsPerMonth(int childId, DateTime startRange, DateTime endRange)
        {
            return _unitOfWork.FeedingRepository.All()
                 .Where(c => c.ChildId == childId)
                 .Where(s => s.StartTime >= startRange)
                 .Where(s => s.StartTime <= endRange)
                 .GroupBy(x => new { x.StartTime.Month, x.StartTime.Year })
                 .Select(x => new ChartResult { Amount = x.Sum(c => c.Amount), Date = DbFunctions.CreateDateTime(x.Key.Year, x.Key.Month, 1, 0, 0, 0) })
                 .OrderBy(o => o.Date);
        }

        protected IHttpActionResult GetFeedOverTime(int childId, DateTime? startRange, DateTime? endRange)
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
                  GetFeedingsPerMonth(childId, startRange.Value, endRange.Value)
                : GetFeedingsPerDay(childId, startRange.Value, endRange.Value);

            return Ok(feedingsQuery.ToList());
        }
        #endregion

        [HttpGet]
        [Route("birth/feedovertime")]
        public IHttpActionResult GetFeedOverTimeSinceBirth(int childId)
        {
            //Null StartRange = Child DOB
            //Null EndRange = Now
            return GetFeedOverTime(childId, null, null);
        }

        [HttpGet]
        [Route("week/feedovertime")]
        public IHttpActionResult GetFeedOverTime(int childId)
        {
            var end = DateTime.Now;
            var start = end.AddDays(-7);
            return GetFeedOverTime(childId, start, end);
        }

        [HttpGet]
        [Route("week/feedcount")]
        public IHttpActionResult GetFeedCountOverTime(int childId)
        {
            var end = DateTime.Now;
            var start = end.AddDays(-7);
            return GetFeedCountOverTime(childId, start, end);
        }

        [HttpGet]
        [Route("month/feedcount")]
        public IHttpActionResult GetFeedCountOverTimeMonth(int childId)
        {
            var end = DateTime.Now;
            var start = end.AddMonths(-1);
            return GetFeedCountOverTime(childId, start, end);
        }

        [HttpGet]
        [Route("month/feedovertime")]
        public IHttpActionResult GetFeedOverTimeMonth(int childId)
        {
            var end = DateTime.Now;
            var start = end.AddMonths(-1);
            return GetFeedOverTime(childId, start, end);
        }


        [HttpGet]
        [Route("3month/feedovertime")]
        public IHttpActionResult GetFeedOverTime3Month(int childId)
        {
            var end = DateTime.Now;
            var start = end.AddMonths(-3);
            return GetFeedOverTime(childId, start, end);
        }


        #region Feedings Count over time
        IEnumerable<ChartResult> GetFeedingsCountPerDay(int childId, DateTime startRange, DateTime endRange)
        {
            return _unitOfWork.FeedingRepository.All()
                 .Where(c => c.ChildId == childId)
                 .Where(s => s.StartTime >= startRange)
                 .Where(s => s.StartTime <= endRange)
                 .GroupBy(x => DbFunctions.TruncateTime(x.StartTime))
                 .Select(x => new ChartResult { Amount = x.Count(), Date = x.Key })
                 .OrderBy(o => o.Date);
        }

        IEnumerable<ChartResult> GetFeedingsCountPerMonth(int childId, DateTime startRange, DateTime endRange)
        {
            return _unitOfWork.FeedingRepository.All()
                 .Where(c => c.ChildId == childId)
                 .Where(s => s.StartTime >= startRange)
                 .Where(s => s.StartTime <= endRange)
                 .GroupBy(x => new { x.StartTime.Month, x.StartTime.Year })
                 .Select(x => new ChartResult { Amount = x.Count(), Date = DbFunctions.CreateDateTime(x.Key.Year, x.Key.Month, 1, 0, 0, 0) })
                 .OrderBy(o => o.Date);
        }

        protected IHttpActionResult GetFeedCountOverTime(int childId, DateTime? startRange, DateTime? endRange)
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



            var feedingsQuery = duration >= 32 ?
                  GetFeedingsCountPerMonth(childId, startRange.Value, endRange.Value)
                : GetFeedingsCountPerDay(childId, startRange.Value, endRange.Value);

            return Ok(feedingsQuery.ToList());
        }
        #endregion

    }
}
