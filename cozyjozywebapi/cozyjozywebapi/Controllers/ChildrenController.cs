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
using cozyjozywebapi.Models;
using Microsoft.AspNet.Identity;

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

            var savedChild = db.Child.Add(child);
            await db.SaveChangesAsync();
            var userId = HttpContext.Current.User.Identity.GetUserId();
            db.ChildPermissions.Add(new ChildPermissions()
            {
                ChildId = child.Id,
                Role = db.Roles.FirstOrDefault(c => c.Id == "PARENT"),
                IdentityUserId = userId
            });
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