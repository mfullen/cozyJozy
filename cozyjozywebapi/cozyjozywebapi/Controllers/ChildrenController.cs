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

namespace cozyjozywebapi.Controllers
{
    public class ChildrenController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public ChildrenController(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }

        public class ChildResponse
        {
            public Child Child { get; set; }
            public bool ReadOnly { get; set; }
        }

        protected IQueryable<ChildPermissions> FilteredChildren()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            return _unitOfWork.ChildPermissionsRepository.All().Where(c => c.IdentityUserId == userId);
            //.Select(c => new ChildResponse
            //{
            //    Child = c.Child,
            //    ReadOnly = c.ReadOnly
            //});
        }

        // GET: api/Children
        public IQueryable<Child> GetChild()
        {
            return FilteredChildren().Select(c => c.Child);
        }

        // GET: api/Children/5
        [ResponseType(typeof(Child))]
        public async Task<IHttpActionResult> GetChild(int id)
        {
            Child child =  await FilteredChildren().Where(c=> c.ChildId == id).Select(c=> c.Child).FirstAsync();
            if (child == null)
            {
                return NotFound();
            }

            return Ok(child);
        }

        // PUT: api/Children/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutChild(int id, Child child)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != child.Id)
            {
                return BadRequest();
            }

            if(!FilteredChildren().Any(c => c.ChildId == id))
            {
                return BadRequest();
            }

            _unitOfWork.ChildRepository.Update(child, c=> c.Id);

            try
            {
                await _unitOfWork.CommitAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChildExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Children
        [ResponseType(typeof(Child))]
        public async Task<IHttpActionResult> PostChild(Child child)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var savedChild = _unitOfWork.ChildRepository.Add(child);
            await _unitOfWork.CommitAsync();
            var userId = HttpContext.Current.User.Identity.GetUserId();
            _unitOfWork.ChildPermissionsRepository.Add(new ChildPermissions()
            {
                ChildId = child.Id,
                ReadOnly = false,
                IdentityUserId = userId
            });
            await _unitOfWork.CommitAsync();
            return CreatedAtRoute("DefaultApi", new { id = child.Id }, child);
        }

        // DELETE: api/Children/5
        [ResponseType(typeof(Child))]
        public async Task<IHttpActionResult> DeleteChild(int id)
        {
            if (!FilteredChildren().Any(c => c.ChildId == id))
            {
                return BadRequest();
            }

            Child child = await _unitOfWork.ChildRepository.FindAsync(c=> c.Id == id);
            if (child == null)
            {
                return NotFound();
            }

            _unitOfWork.ChildRepository.Delete(child);
            await _unitOfWork.CommitAsync();

            return Ok(child);
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