﻿using System;
using System.Collections.Generic;
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
using cozyjozywebapi.Infrastructure.Core;
using cozyjozywebapi.Models;
using Microsoft.AspNet.Identity;

namespace cozyjozywebapi.Controllers
{
    [ChildPermissionFilter]
    public class MeasurementController : BaseTrackingController
    {
        private readonly CozyJozyContext context = new CozyJozyContext();

        public MeasurementController(IUnitOfWork uow)
            : base(uow)
        {

        }

        // GET: api/Measurement
        public IHttpActionResult Get(int pagesize = 25, int page = 0, int childId = 0)
        {
            var authorthizedChildren = HttpContext.Current.Items[Authorthizedchildren] as List<int>;

            if (pagesize > MaxPageSize)
            {
                pagesize = MaxPageSize;
            }

            var data = context.Measurements.OrderByDescending(v => v.DateRecorded).Where(x => authorthizedChildren.Contains(x.ChildId));

            if (childId > 0)
            {
                data = data.Where(c => c.ChildId == childId);
            }
            return Ok(data.Skip(page * pagesize).Take(pagesize).ToList());
        }

        // GET: api/Measurement/5
        [ResponseType(typeof(Measurement))]
        public async Task<IHttpActionResult> GetMeasurement(int id)
        {
            var authorthizedChildren = HttpContext.Current.Items[Authorthizedchildren] as List<int>;

            Measurement items = await context.Measurements.FindAsync(id);
            if (items == null || !authorthizedChildren.Contains(items.ChildId))
            {
                return NotFound();
            }

            return Ok(items);
        }

        // PUT: api/Measurement/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutMeasurement(Measurement measurement)
        {
            var id = measurement.Id;
            var authorthizedChildren = HttpContext.Current.Items[Authorthizedchildren] as List<int>;

            if (!ModelState.IsValid || !authorthizedChildren.Contains(measurement.ChildId))
            {
                return BadRequest(ModelState);
            }

            if (id != measurement.Id)
            {
                return BadRequest();
            }

            if (!HasWritePermission(measurement.ChildId))
            {
                return BadRequest();
            }


            context.Entry(measurement).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MeasurementExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(measurement);
        }

        // POST: api/Measurement
        [ResponseType(typeof(Measurement))]
        public async Task<IHttpActionResult> PostMeasurement(Measurement measurement)
        {
            var authorthizedChildren = HttpContext.Current.Items[Authorthizedchildren] as List<int>;

            if (!ModelState.IsValid || !authorthizedChildren.Contains(measurement.ChildId))
            {
                return BadRequest(ModelState);
            }

            if (!HasWritePermission(measurement.ChildId))
            {
                return BadRequest();
            }


            context.Measurements.Add(measurement);

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                if (MeasurementExists(measurement.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw e;
                }
            }
            var myUri = Request.RequestUri + measurement.Id.ToString();
            return Created(myUri, measurement);
        }

        // DELETE: api/Measurement/5
        [ResponseType(typeof(Measurement))]
        public async Task<IHttpActionResult> DeleteMeasurement(int id)
        {
            var authorthizedChildren = HttpContext.Current.Items[Authorthizedchildren] as List<int>;

            Measurement measurement = await context.Measurements.FindAsync(id);
            if (measurement == null || !authorthizedChildren.Contains(measurement.ChildId))
            {
                return NotFound();
            }

            if (!HasWritePermission(measurement.ChildId))
            {
                return BadRequest();
            }

            context.Measurements.Remove(measurement);
            await context.SaveChangesAsync();

            return Ok(measurement);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MeasurementExists(int id)
        {
            return context.Measurements.Count(e => e.Id == id) > 0;
        }

        public bool HasWritePermission(int childId)
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var hasWritePermission = _unitOfWork.ChildPermissionsRepository
               .Where(c => c.ChildId == childId)
               .Where(c => c.IdentityUserId == userId).Any(c => c.MeasurementWriteAccess == true);
            return hasWritePermission;
        }
    }
}
