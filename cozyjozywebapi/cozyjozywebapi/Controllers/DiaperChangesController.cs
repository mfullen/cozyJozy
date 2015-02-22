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
using cozyjozywebapi.Models;

namespace cozyjozywebapi.Controllers
{
    [ChildPermissionFilter]
    public class DiaperChangesController : ApiController
    {
        private CozyJozyContext context = new CozyJozyContext();
        private const int MaxPageSize = 100;

        // GET: api/DiaperChanges
        public IHttpActionResult Get(int pagesize = 25, int page = 0, int childId = 0)
        {
            var authorthizedChildren = HttpContext.Current.Items["authorthizedChildren"] as List<int>;

            if (pagesize > MaxPageSize)
            {
                pagesize = MaxPageSize;
            }

            var data = context.DiaperChanges.Where(x => authorthizedChildren.Contains(x.ChildId)).OrderBy(v => v.OccurredOn).Skip(page * pagesize).Take(pagesize);

            if (childId > 0)
            {
                data = data.Where(c => c.ChildId == childId);
            }
            return Ok(data.ToList());
        }

        // GET: api/DiaperChanges/5
        [ResponseType(typeof(DiaperChanges))]
        public async Task<IHttpActionResult> GetDiaperChanges(int id)
        {
            var authorthizedChildren = HttpContext.Current.Items["authorthizedChildren"] as List<int>;

            DiaperChanges diaperChanges = await context.DiaperChanges.FindAsync(id);
            if (diaperChanges == null || !authorthizedChildren.Contains(diaperChanges.ChildId))
            {
                return NotFound();
            }

            return Ok(diaperChanges);
        }

        // PUT: api/DiaperChanges/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutDiaperChanges(int id, DiaperChanges diaperChanges)
        {
            var authorthizedChildren = HttpContext.Current.Items["authorthizedChildren"] as List<int>;

            if (!ModelState.IsValid || !authorthizedChildren.Contains(diaperChanges.ChildId))
            {
                return BadRequest(ModelState);
            }

            if (id != diaperChanges.Id)
            {
                return BadRequest();
            }

            context.Entry(diaperChanges).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
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

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/DiaperChanges
        [ResponseType(typeof(DiaperChanges))]
        public async Task<IHttpActionResult> PostDiaperChanges(DiaperChanges diaperChanges)
        {
            var authorthizedChildren = HttpContext.Current.Items["authorthizedChildren"] as List<int>;

            if (!ModelState.IsValid || !authorthizedChildren.Contains(diaperChanges.ChildId))
            {
                return BadRequest(ModelState);
            }

            context.DiaperChanges.Add(diaperChanges);

            try
            {
                await context.SaveChangesAsync();
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

            return CreatedAtRoute("DefaultApi", new { id = diaperChanges.Id }, diaperChanges);
        }

        // DELETE: api/DiaperChanges/5
        [ResponseType(typeof(DiaperChanges))]
        public async Task<IHttpActionResult> DeleteDiaperChanges(int id)
        {
            var authorthizedChildren = HttpContext.Current.Items["authorthizedChildren"] as List<int>;

            DiaperChanges diaperChanges = await context.DiaperChanges.FindAsync(id);
            if (diaperChanges == null || !authorthizedChildren.Contains(diaperChanges.ChildId))
            {
                return NotFound();
            }

            context.DiaperChanges.Remove(diaperChanges);
            await context.SaveChangesAsync();

            return Ok(diaperChanges);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DiaperChangesExists(int id)
        {
            return context.DiaperChanges.Count(e => e.Id == id) > 0;
        }
    }
}