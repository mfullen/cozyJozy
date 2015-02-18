using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using cozyjozywebapi.Entity;
using cozyjozywebapi.Models;

namespace cozyjozywebapi.Controllers
{
    public class ChildrenController : ApiController
    {
        private CozyJozyContext db = new CozyJozyContext();

        // GET: api/Children
        public IQueryable<Child> GetChild()
        {
            return db.Child;
        }

        // GET: api/Children/5
        [ResponseType(typeof(Child))]
        public async Task<IHttpActionResult> GetChild(int id)
        {
            Child child = await db.Child.FindAsync(id);
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

            db.Entry(child).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
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

            db.Child.Add(child);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = child.Id }, child);
        }

        // DELETE: api/Children/5
        [ResponseType(typeof(Child))]
        public async Task<IHttpActionResult> DeleteChild(int id)
        {
            Child child = await db.Child.FindAsync(id);
            if (child == null)
            {
                return NotFound();
            }

            db.Child.Remove(child);
            await db.SaveChangesAsync();

            return Ok(child);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ChildExists(int id)
        {
            return db.Child.Count(e => e.Id == id) > 0;
        }
    }
}