using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using cozyjozywebapi.Filters;
using cozyjozywebapi.Infrastructure;
using cozyjozywebapi.Infrastructure.Core;
using cozyjozywebapi.Models;
using Microsoft.AspNet.Identity;

namespace cozyjozywebapi.Controllers
{
    [ChildPermissionFilter]
    public class FeedingController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private const string Authorthizedchildren = "authorthizedChildren";
        private const int MaxPageSize = 100;
        private readonly IFeedingRepository _feedingRepository;
        public UserManager<User> UserManager { get; private set; }

        public FeedingController(IUnitOfWork uow)
        {
            _unitOfWork = uow;
            _feedingRepository = uow.FeedingRepository;
            UserManager = Startup.UserManagerFactory();
        }

        class FeedingResponse 
        {
            public int Id { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
            public DeliveryType? DeliveryType { get; set; }
            public double? Amount { get; set; }
            public DateTime DateReported { get; set; }
            public bool SpitUp { get; set; }
            public string Notes { get; set; }
            public int ChildId { get; set; }
            public string UserId { get; set; }

            public FeedingResponse(Feedings model)
            {
                Id = model.Id;
                StartTime = model.StartTime;
                EndTime = model.EndTime;
                DeliveryType = model.DeliveryType;
                Amount = model.Amount;
                DateReported = model.DateReported;
                SpitUp = model.SpitUp;
                Notes = model.Notes;
                ChildId = model.ChildId;
                UserId = model.UserId;
            }

            public UserRestModel ReportedByUser { get; set; }
        }

        async Task<UserRestModel> GetById(string userId, int childId)
        {
            var user = _unitOfWork.UserRepository.GetById(userId);
            var title = _unitOfWork.ChildPermissionsRepository
                .Where(c => c.ChildId == childId)
                .Where(u => u.IdentityUserId == user.Id)
                .Select(t => t.Title.Name).FirstOrDefault();

            var model = new UserRestModel
            {
                Email = user.Email,
                Id = user.Id,
                UserName = user.UserName,
                Title = title
            };

            model.ProfileImageUrl = await ExternalAccountHelper.GetProfileImageUrl(UserManager, model.Id);
            return model;
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


            var data = _feedingRepository.All()
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
                var f = new FeedingResponse(s);
                var userResponse = await GetById(s.UserId, childId);
                f.ReportedByUser = userResponse;
                return f;
            }));

           return Ok(userResults);
        }

        // GET api/<controller>/5
        public async Task<IHttpActionResult> Get(int id)
        {
            var authorthizedChildren = HttpContext.Current.Items[Authorthizedchildren] as List<int>;

            var data = _feedingRepository.All().FirstOrDefault(f => f.Id == id);
            if (data == null || !authorthizedChildren.Contains(data.ChildId))
                return NotFound();

            var feed = new FeedingResponse(data);
            var userResponse = await GetById(data.UserId, id);
            feed.ReportedByUser = userResponse;

            return Ok(feed);
        }

        // POST api/<controller>
        public async Task<IHttpActionResult> Post(Feedings feeding)
        {
            var authorthizedChildren = HttpContext.Current.Items[Authorthizedchildren] as List<int>;
            if (!authorthizedChildren.Contains(feeding.ChildId))
                return BadRequest();

            var newFeeding = new Feedings()
            {
                Amount = feeding.Amount,
                Child = _unitOfWork.ChildRepository.All().FirstOrDefault(f => f.Id == feeding.ChildId),
                DateReported = DateTime.Now,
                SpitUp = feeding.SpitUp,
                StartTime = feeding.StartTime,
                EndTime = feeding.EndTime,
                DeliveryType = feeding.DeliveryType,
                Notes = feeding.Notes
            };

            if (!HasWritePermission(newFeeding.Child.Id))
            {
                return BadRequest();
            }

            newFeeding.ChildId = newFeeding.Child.Id;
            var userId = HttpContext.Current.User.Identity.GetUserId();
            newFeeding.UserId = userId;
            var entity = _feedingRepository.Add(newFeeding);
            _unitOfWork.Commit();
            var myUri = Request.RequestUri + entity.Id.ToString();

            var feed = new FeedingResponse(entity);
            var userResponse = await GetById(entity.UserId, feeding.ChildId);
            feed.ReportedByUser = userResponse;

            return Created(myUri, feed);
        }

        // PUT api/<controller>/5
        public async Task<IHttpActionResult> Put(Feedings feeding)
        {
            var authorthizedChildren = HttpContext.Current.Items[Authorthizedchildren] as List<int>;

            if (!authorthizedChildren.Contains(feeding.ChildId))
                return BadRequest();

            feeding.DateReported = DateTime.Now;
            var userId = HttpContext.Current.User.Identity.GetUserId();
            feeding.UserId = userId;
            var existingFeed = _feedingRepository.All().FirstOrDefault(i => i.Id == feeding.Id);
            if (existingFeed == null || existingFeed.Id < 1)
            {
                return await Post(feeding);
            }

            if (!HasWritePermission(existingFeed.ChildId))
            {
                return BadRequest();
            }

            _feedingRepository.Update(feeding, f => f.Id);
            _unitOfWork.Commit();

            var feed = new FeedingResponse(existingFeed);
            var userResponse = await GetById(existingFeed.UserId, feeding.ChildId);
            feed.ReportedByUser = userResponse;
            return Ok(feed);
        }

        // DELETE api/<controller>/5
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            var authorthizedChildren = HttpContext.Current.Items[Authorthizedchildren] as List<int>;

            var existingFeed = _feedingRepository.All().FirstOrDefault(i => i.Id == id);
            if (existingFeed == null || !authorthizedChildren.Contains(existingFeed.ChildId))
                return NotFound();


            if (!HasWritePermission(existingFeed.ChildId))
            {
                return BadRequest();
            }

            _feedingRepository.Delete(existingFeed);
            _unitOfWork.Commit();
            return Ok();
        }

        public bool HasWritePermission(int childId)
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var hasWritePermission = _unitOfWork.ChildPermissionsRepository
               .Where(c => c.ChildId == childId)
               .Where(c => c.IdentityUserId == userId).Any(c => c.ReadOnly == false);
            return hasWritePermission;
        }

    }
}