using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Http.Results;
using cozyjozywebapi.Entity;
using cozyjozywebapi.Models;

namespace cozyjozywebapi.Controllers
{
    public class FeedingController : ApiController
    {
        private readonly CozyJozyContext context = new CozyJozyContext();
        private const int MaxPageSize = 100;
        // GET api/<controller>

        public IHttpActionResult Get(int pagesize = 25, int page = 0)
        {
            if (pagesize > MaxPageSize)
            {
                pagesize = MaxPageSize;
            }
            var data = context.Feedings.OrderBy(v => v.StartTime).Skip(page * pagesize).Take(pagesize).ToList();

            return Ok(data);
        }

        // GET api/<controller>/5
        public IHttpActionResult Get(int id)
        {
            var data = context.Feedings.FirstOrDefault(f => f.Id == id);
            if (data == null)
                return NotFound();
            return Ok(data);
        }

        // POST api/<controller>
        public IHttpActionResult Post(Feedings feeding)
        {
            feeding.Id = 0;
            feeding.DateReported = DateTime.Now;
            var entity = context.Feedings.Add(feeding);
            context.SaveChanges();
            var myUri = Request.RequestUri + entity.Id.ToString();
            return Created(myUri, entity);
        }

        // PUT api/<controller>/5
        public IHttpActionResult Put(Feedings feeding)
        {
            feeding.DateReported = DateTime.Now;
            var existingFeed = context.Feedings.FirstOrDefault(i => i.Id == feeding.Id);
            if (existingFeed == null || existingFeed.Id < 1)
            {
                return Post(feeding);
            }

            context.Entry(existingFeed).CurrentValues.SetValues(feeding);
            context.SaveChanges();
            return Ok(existingFeed);
        }

        // DELETE api/<controller>/5
        public IHttpActionResult Delete(int id)
        {
            var existingFeed = context.Feedings.FirstOrDefault(i => i.Id == id);
            if (existingFeed == null)
                return NotFound();
            context.Feedings.Remove(existingFeed);
            context.SaveChanges();
            return Ok();
        }
    }
}