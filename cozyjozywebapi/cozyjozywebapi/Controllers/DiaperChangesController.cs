using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using cozyjozywebapi.Entity;
using cozyjozywebapi.Filters;
using cozyjozywebapi.Infrastructure;
using cozyjozywebapi.Infrastructure.Core;
using cozyjozywebapi.Models;
using Microsoft.AspNet.Identity;

namespace cozyjozywebapi.Controllers
{
    [ChildPermissionFilter]
    public class DiaperChangesController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private const int MaxPageSize = 100;
        private const string Authorthizedchildren = "authorthizedChildren";

        public DiaperChangesController(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }

        // GET: api/DiaperChanges
        public IHttpActionResult Get(int childId, int pagesize = 25, int page = 0,
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
            var authorthizedChildren = HttpContext.Current.Items[Authorthizedchildren] as List<int>;

            if (pagesize > MaxPageSize)
            {
                pagesize = MaxPageSize;
            }

            var data = _unitOfWork.DiaperChangesRepository.All()
               .OrderByDescending(v => v.OccurredOn)
               .Where(x => authorthizedChildren.Contains(x.ChildId))
               .Where(c => c.ChildId == childId)
               .Where(d => DbFunctions.TruncateTime(d.OccurredOn) >= startDate)
               .Where(d => DbFunctions.TruncateTime(d.OccurredOn) <= endDate);
          
            return Ok(data.Skip(page * pagesize).Take(pagesize).ToList());
        }

        // GET: api/DiaperChanges/5
        [ResponseType(typeof(DiaperChanges))]
        public async Task<IHttpActionResult> GetDiaperChanges(int id)
        {
            var authorthizedChildren = HttpContext.Current.Items[Authorthizedchildren] as List<int>;

            DiaperChanges diaperChanges = await _unitOfWork.DiaperChangesRepository.FindAsync(c=> c.Id == id);
            if (diaperChanges == null || !authorthizedChildren.Contains(diaperChanges.ChildId))
            {
                return NotFound();
            }

            return Ok(diaperChanges);
        }

        // PUT: api/DiaperChanges/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutDiaperChanges(DiaperChanges diaperChanges)
        {
            var id = diaperChanges.Id;
            var authorthizedChildren = HttpContext.Current.Items[Authorthizedchildren] as List<int>;

            if (!ModelState.IsValid || !authorthizedChildren.Contains(diaperChanges.ChildId))
            {
                return BadRequest(ModelState);
            }

            if (id != diaperChanges.Id)
            {
                return BadRequest();
            }
            var userId = HttpContext.Current.User.Identity.GetUserId();
            diaperChanges.UserId = userId;

            _unitOfWork.DiaperChangesRepository.Update(diaperChanges, d=> d.Id);
            

            try
            {
                await _unitOfWork.CommitAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DiaperChangesExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(diaperChanges);
        }

        // POST: api/DiaperChanges
        [ResponseType(typeof(DiaperChanges))]
        public async Task<IHttpActionResult> PostDiaperChanges(DiaperChanges diaperChanges)
        {
            var authorthizedChildren = HttpContext.Current.Items[Authorthizedchildren] as List<int>;

            if (!ModelState.IsValid || !authorthizedChildren.Contains(diaperChanges.ChildId))
            {
                return BadRequest(ModelState);
            }

            var userId = HttpContext.Current.User.Identity.GetUserId();
            diaperChanges.UserId = userId;
            _unitOfWork.DiaperChangesRepository.Add(diaperChanges);

            try
            {
               await _unitOfWork.CommitAsync();
            }
            catch (DbUpdateException e)
            {
                if (DiaperChangesExists(diaperChanges.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw e;
                }
            }
            var myUri = Request.RequestUri + diaperChanges.Id.ToString();
            return Created(myUri, diaperChanges);
        }

        // DELETE: api/DiaperChanges/5
        [ResponseType(typeof(DiaperChanges))]
        public async Task<IHttpActionResult> DeleteDiaperChanges(int id)
        {
            var authorthizedChildren = HttpContext.Current.Items[Authorthizedchildren] as List<int>;

            DiaperChanges diaperChanges = await _unitOfWork.DiaperChangesRepository.FindAsync(d=> d.Id == id);
            if (diaperChanges == null || !authorthizedChildren.Contains(diaperChanges.ChildId))
            {
                return NotFound();
            }

            _unitOfWork.DiaperChangesRepository.Delete(diaperChanges);

            await _unitOfWork.CommitAsync();

            return Ok(diaperChanges);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DiaperChangesExists(int id)
        {
            return _unitOfWork.DiaperChangesRepository.All().Count(e => e.Id == id) > 0;
        }
    }
}