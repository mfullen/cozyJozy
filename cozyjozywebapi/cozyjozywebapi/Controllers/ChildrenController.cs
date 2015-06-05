using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using cozyjozywebapi.Infrastructure.Core;
using cozyjozywebapi.Models;
using Microsoft.AspNet.Identity;
using MoreLinq;

namespace cozyjozywebapi.Controllers
{
    public class ChildrenController : BaseTrackingController
    {
        public ChildrenController(IUnitOfWork uow)
            : base(uow)
        {
        }

        protected IQueryable<ChildPermissions> FilteredChildren()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            return _unitOfWork.ChildPermissionsRepository.All().Where(c => c.IdentityUserId == userId).DistinctBy(p => new { p.IdentityUserId, p.ChildId }).AsQueryable();
        }

        // GET: api/Children
        public async Task<IQueryable<Permission>> GetChild()
        {

            var permissions = await Task.WhenAll(FilteredChildren().Select(c => Convert(c)));
            return permissions.AsQueryable();
        }

        // GET: api/Children/5
        [ResponseType(typeof(Permission))]
        public async Task<IHttpActionResult> GetChild(int id)
        {
            var child =  FilteredChildren().FirstOrDefault(c => c.ChildId == id);
            if (child == null)
            {
                return NotFound();
            }
            var p = await Convert(child);
            return Ok(p);
        }

        // PUT: api/Children/5
        [ResponseType(typeof(Permission))]
        public async Task<IHttpActionResult> PutChild(Child child)
        {
            var id = child.Id;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != child.Id)
            {
                return BadRequest();
            }

            if (!FilteredChildren().Any(c => c.ChildId == id))
            {
                return BadRequest();
            }

            _unitOfWork.ChildRepository.Update(child, c => c.Id);

            try
            {
                await _unitOfWork.CommitAsync();
                var matchingChild = FilteredChildren().FirstOrDefault(c => c.ChildId == child.Id);
                var p = await Convert(matchingChild);
                return Ok(p);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChildExists(id))
                {
                    return NotFound();
                }
                throw;
            }
        }

        // POST: api/Children
        [ResponseType(typeof(Permission))]
        public async Task<IHttpActionResult> PostChild(Child child)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var savedChild = _unitOfWork.ChildRepository.Add(child);
            await _unitOfWork.CommitAsync();
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var cp = _unitOfWork.ChildPermissionsRepository.Add(new ChildPermissions()
             {
                 ChildId = savedChild.Id,
                 IdentityUserId = userId,
                 TitleId = _unitOfWork.TitleRepository.Where(t => t.Name.Equals("Parent/Guardian")).FirstOrDefault().Id,
                 FeedingWriteAccess = true,
                 DiaperChangeWriteAccess = true,
                 SleepWriteAccess = true,
                 MeasurementWriteAccess = true,
                 ChildManagementWriteAccess = true,
                 PermissionsWriteAccess = true
             });
            await _unitOfWork.CommitAsync();
            var myUri = Request.RequestUri + cp.ChildId.ToString();
            var p = await Convert(cp);
            return Created(myUri, p);
        }

        // DELETE: api/Children/5
        [ResponseType(typeof(Permission))]
        public async Task<IHttpActionResult> DeleteChild(int id)
        {
            if (!FilteredChildren().Any(c => c.ChildId == id))
            {
                return BadRequest();
            }

            Child child = await _unitOfWork.ChildRepository.FindAsync(c => c.Id == id);
            if (child == null)
            {
                return NotFound();
            }

            _unitOfWork.ChildRepository.Delete(child);
            await _unitOfWork.CommitAsync();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ChildExists(int id)
        {
            return _unitOfWork.ChildRepository.All().Count(e => e.Id == id) > 0;
        }
    }
}