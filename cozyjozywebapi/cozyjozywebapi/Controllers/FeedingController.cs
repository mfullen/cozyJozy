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
using cozyjozywebapi.Models;

namespace cozyjozywebapi.Controllers
{
    [ChildPermissionFilter]
    public class FeedingController : ApiController
    {
        private readonly CozyJozyContext context = new CozyJozyContext();
        private const int MaxPageSize = 100;
        // GET api/<controller>


        public IHttpActionResult Get(List<int> authorthizedChildren, int pagesize = 25, int page = 0, int childId = 0)
        {
            if (pagesize > MaxPageSize)
            {
                pagesize = MaxPageSize;
            }

            var data = context.Feedings.Where(x => authorthizedChildren.Contains(x.ChildId)).OrderBy(v => v.StartTime).Skip(page * pagesize).Take(pagesize);

            if (childId > 0)
            {
                data = data.Where(c => c.ChildId == childId);
            }
            return Ok(data.ToList());
        }

        // GET api/<controller>/5
        public IHttpActionResult Get(List<int> authorthizedChildren, int id)
        {
            var data = context.Feedings.FirstOrDefault(f => f.Id == id);
            if (data == null || !authorthizedChildren.Contains(data.ChildId))
                return NotFound();

            return Ok(data);
        }

        // POST api/<controller>
        public IHttpActionResult Post(List<int> authorthizedChildren, Feedings feeding)
        {
            if (!authorthizedChildren.Contains(feeding.ChildId))
                return BadRequest();

            feeding.Id = 0;
            feeding.DateReported = DateTime.Now;
            var entity = context.Feedings.Add(feeding);
            context.SaveChanges();
            var myUri = Request.RequestUri + entity.Id.ToString();
            return Created(myUri, entity);
        }

        // PUT api/<controller>/5
        public IHttpActionResult Put(List<int> authorthizedChildren, Feedings feeding)
        {
            if (!authorthizedChildren.Contains(feeding.ChildId))
                return BadRequest();

            feeding.DateReported = DateTime.Now;
            var existingFeed = context.Feedings.FirstOrDefault(i => i.Id == feeding.Id);
            if (existingFeed == null || existingFeed.Id < 1)
            {
                return Post(authorthizedChildren, feeding);
            }

            context.Entry(existingFeed).CurrentValues.SetValues(feeding);
            context.SaveChanges();
            return Ok(existingFeed);
        }

        // DELETE api/<controller>/5
        public IHttpActionResult Delete(List<int> authorthizedChildren, int id)
        {
            var existingFeed = context.Feedings.FirstOrDefault(i => i.Id == id);
            if (existingFeed == null || !authorthizedChildren.Contains(existingFeed.ChildId))
                return NotFound();

            context.Feedings.Remove(existingFeed);
            context.SaveChanges();
            return Ok();
        }
    }
}