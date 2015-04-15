using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.UI.WebControls;
using cozyjozywebapi.Entity;
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

        public FeedingController(IUnitOfWork uow)
        {
            _unitOfWork = uow;
            _feedingRepository = uow.FeedingRepository;
        }

        public IHttpActionResult Get(int pagesize = 25, int page = 0, int childId = 0)
        {
            var authorthizedChildren = HttpContext.Current.Items[Authorthizedchildren] as List<int>;

            if (pagesize > MaxPageSize)
            {
                pagesize = MaxPageSize;
            }


            var data = _feedingRepository.All().OrderByDescending(v => v.EndTime).Where(x => authorthizedChildren.Contains(x.ChildId));

            if (childId > 0)
            {
                data = data.Where(c => c.ChildId == childId);
            }
            var results = data.Skip(page * pagesize).Take(pagesize).ToList();
            return Ok(results);
        }

        // GET api/<controller>/5
        public IHttpActionResult Get(int id)
        {
            var authorthizedChildren = HttpContext.Current.Items[Authorthizedchildren] as List<int>;

            var data = _feedingRepository.All().FirstOrDefault(f => f.Id == id);
            if (data == null || !authorthizedChildren.Contains(data.ChildId))
                return NotFound();

            return Ok(data);
        }

        // POST api/<controller>
        public IHttpActionResult Post(Feedings feeding)
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
            return Created(myUri, entity);
        }

        // PUT api/<controller>/5
        public IHttpActionResult Put(Feedings feeding)
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
                return Post(feeding);
            }

            if (!HasWritePermission(existingFeed.ChildId))
            {
                return BadRequest();
            }

            _feedingRepository.Update(feeding, f => f.Id);
            _unitOfWork.Commit();
            return Ok(existingFeed);
        }

        // DELETE api/<controller>/5
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