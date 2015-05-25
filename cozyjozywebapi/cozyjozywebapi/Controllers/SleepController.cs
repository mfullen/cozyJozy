using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using cozyjozywebapi.Filters;
using cozyjozywebapi.Infrastructure.Core;
using cozyjozywebapi.Models;
using Microsoft.AspNet.Identity;

namespace cozyjozywebapi.Controllers
{
     [ChildPermissionFilter]
    public class SleepController : BaseTrackingController
    {
        public SleepController(IUnitOfWork uow) : base(uow)
        {

        }

         public class SleepResponse
         {
             public int Id { get; set; }
             public DateTime StartTime { get; set; }
             public DateTime EndTime { get; set; }
             public string Notes { get; set; }
             public string UserId { get; set; }
             public int ChildId { get; set; }

             public UserRestModel ReportedByUser { get; set; }

             public SleepResponse(SleepSession model)
             {
                 Id = model.Id;
                 StartTime = model.StartTime;
                 EndTime = model.EndTime;
                 Notes = model.Notes;
                 UserId = model.UserId;
                 ChildId = model.ChildId;
             }
         }

        public async Task<IHttpActionResult> Get(int childId,
         int pagesize = 25, int page = 0,
         DateTime? startDate = null,
         DateTime? endDate = null)
        {
            if (childId <= 0)
            {
                return BadRequest("ChildId is not valid.");
            }

            if (startDate == null)
            {
                startDate = DateTime.Today;
            }

            if (endDate == null)
            {
                endDate = DateTime.Today;
            }

            var isDateRange = startDate != endDate;

            var authorthizedChildren = HttpContext.Current.Items[Authorthizedchildren] as List<int>;

            if (pagesize > MaxPageSize)
            {
                pagesize = MaxPageSize;
            }


            var data = _unitOfWork.SleepRepository.All()
                .OrderByDescending(v => v.EndTime)
                .Where(x => authorthizedChildren.Contains(x.ChildId))
                .Where(c => c.ChildId == childId)
                .Where(d => DbFunctions.TruncateTime(d.StartTime) >= startDate);

            if (isDateRange)
            {
                data = data.Where(d => DbFunctions.TruncateTime(d.EndTime) <= endDate);
            }

            else
            {
                //only looking at 1 day worth of data
                endDate = endDate.Value.AddDays(1);
                data = data.Where(d => DbFunctions.TruncateTime(d.StartTime) < endDate);
            }

            var results = data.Skip(page * pagesize).Take(pagesize).ToList();

            var userResults = await Task.WhenAll(results.Select(async s =>
            {
                var f = new SleepResponse(s);
                var userResponse = await GetById(s.UserId, childId);
                f.ReportedByUser = userResponse;
                return f;
            }));

            return Ok(userResults);
        }

        public bool HasWritePermission(int childId)
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var hasWritePermission = _unitOfWork.ChildPermissionsRepository
               .Where(c => c.ChildId == childId)
               .Where(c => c.IdentityUserId == userId).Any(c => c.ReadOnly == false);
            return hasWritePermission;
        }


        public async Task<IHttpActionResult> Post(SleepSession sleepSession)
        {
            var authorthizedChildren = HttpContext.Current.Items[Authorthizedchildren] as List<int>;
            if (!authorthizedChildren.Contains(sleepSession.ChildId))
                return BadRequest();

            var newFeeding = new SleepSession()
            {
                Child = _unitOfWork.ChildRepository.All().FirstOrDefault(f => f.Id == sleepSession.ChildId),
                StartTime = sleepSession.StartTime,
                EndTime = sleepSession.EndTime,
                Notes = sleepSession.Notes
            };

            if (!HasWritePermission(newFeeding.Child.Id))
            {
                return BadRequest();
            }

            newFeeding.ChildId = newFeeding.Child.Id;
            var userId = HttpContext.Current.User.Identity.GetUserId();
            newFeeding.UserId = userId;
            var entity = _unitOfWork.SleepRepository.Add(newFeeding);
            _unitOfWork.Commit();
            var myUri = Request.RequestUri + entity.Id.ToString();

            var item = new SleepResponse(entity);
            var userResponse = await GetById(entity.UserId, sleepSession.ChildId);
            item.ReportedByUser = userResponse;
            return Created(myUri, item);
        }

        public async Task<IHttpActionResult> Put(SleepSession sleepSession)
        {
            var authorthizedChildren = HttpContext.Current.Items[Authorthizedchildren] as List<int>;

            if (!authorthizedChildren.Contains(sleepSession.ChildId))
                return BadRequest();

            var userId = HttpContext.Current.User.Identity.GetUserId();
            sleepSession.UserId = userId;
            var existingFeed = _unitOfWork.SleepRepository.All().FirstOrDefault(i => i.Id == sleepSession.Id);
            if (existingFeed == null || existingFeed.Id < 1)
            {
                return await Post(sleepSession);
            }

            if (!HasWritePermission(existingFeed.ChildId))
            {
                return BadRequest();
            }

            _unitOfWork.SleepRepository.Update(sleepSession, f => f.Id);
            _unitOfWork.Commit();

            var item = new SleepResponse(existingFeed);
            var userResponse = await GetById(existingFeed.UserId, sleepSession.ChildId);
            item.ReportedByUser = userResponse;
            return Ok(item);
        }

        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            var authorthizedChildren = HttpContext.Current.Items[Authorthizedchildren] as List<int>;

            var existingFeed = _unitOfWork.SleepRepository.All().FirstOrDefault(i => i.Id == id);
            if (existingFeed == null || !authorthizedChildren.Contains(existingFeed.ChildId))
                return NotFound();


            if (!HasWritePermission(existingFeed.ChildId))
            {
                return BadRequest();
            }

            _unitOfWork.SleepRepository.Delete(existingFeed);
            _unitOfWork.Commit();
            return Ok();
        }
    }
}
