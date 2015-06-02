using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using cozyjozywebapi.Infrastructure.Core;
using cozyjozywebapi.Models;
using Microsoft.AspNet.Identity;

namespace cozyjozywebapi.Controllers
{
    public class ChildPermissionController : BaseTrackingController
    {

        public ChildPermissionController(IUnitOfWork uow)
            : base(uow)
        {
        }

        public async Task<IHttpActionResult> Get(int childId, int pagesize = 25, int page = 0)
        {

            if (pagesize > MaxPageSize)
            {
                pagesize = MaxPageSize;
            }

            var userId = HttpContext.Current.User.Identity.GetUserId();
            var hasWritePermission = _unitOfWork.ChildPermissionsRepository.All()
                .Where(c => c.ChildId == childId)
                .Where(c => c.IdentityUserId == userId).Any(c => c.ReadOnly == false);

            var permissions = new List<Permission>();

            //if the user requesting who has permissions to view their child, they must have write permissions (AKA NOT READ ONLY)
            if (hasWritePermission)
            {
                //We only want to select all users who belong to permissions but we don't want to select our own user. This will prevent
                //the user from accidently deleting their own permissions and eventually not be able to see their child again

                var cpItems = _unitOfWork.ChildPermissionsRepository.All()
                   .Where(c => c.ChildId == childId)
                   .Where(c => c.IdentityUserId != userId).ToList();
                foreach (var childPermissionse in cpItems)
                {
                    var c = await Convert(childPermissionse);
                    permissions.Add(c);
                }
            }

            var results = permissions.Skip(page * pagesize).Take(pagesize);
            return Ok(results);
        }

        // GET api/<controller>/5
        public IHttpActionResult Get(int id)
        {
            var data = _unitOfWork.ChildPermissionsRepository.All().FirstOrDefault(c => c.Id == id);
            if (data == null)
                return NotFound();

            return Ok(data);
        }

        public async Task<IHttpActionResult> Post(Permission permission)
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var hasWritePermission = _unitOfWork.ChildPermissionsRepository.All()
                .Where(c => c.ChildId == permission.Child.Id)
                .Where(c => c.IdentityUserId == userId).Any(c => c.ReadOnly == false);

            if (!hasWritePermission)
                return BadRequest();

            var child = _unitOfWork.ChildRepository.Where(c => c.Id == permission.Child.Id).FirstOrDefault();
            var userToGivePermission = _unitOfWork.UserRepository.Where(u => u.Id == permission.User.Id).FirstOrDefault();

            if (child == null || userToGivePermission == null)
            {
                return BadRequest();
            }

            var permisionAlreadyExists = _unitOfWork.ChildPermissionsRepository.Where(c => c.ChildId == child.Id).Any(u => u.IdentityUserId == userToGivePermission.Id);

            if (permisionAlreadyExists)
            {
                return StatusCode(HttpStatusCode.Conflict);
            }

            var newCp = Convert(userToGivePermission.Id, child.Id, permission);

            var entity = _unitOfWork.ChildPermissionsRepository.Add(newCp);
            _unitOfWork.Commit();

            var p = await Convert(entity);

            var myUri = Request.RequestUri + "/" + p.Id;
            return Created(myUri, p);
        }

       

        // PUT api/<controller>/5
        public async Task<IHttpActionResult> Put(Permission permission)
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var hasWritePermission = _unitOfWork.ChildPermissionsRepository.All()
                .Where(c => c.ChildId == permission.Child.Id)
                .Where(c => c.IdentityUserId == userId).Any(c => c.ReadOnly == false);

            if (!hasWritePermission)
                return BadRequest();

            var child = _unitOfWork.ChildRepository.Where(c => c.Id == permission.Child.Id).FirstOrDefault();
            var userToGivePermission = _unitOfWork.UserRepository.Where(u => u.Id == permission.User.Id).FirstOrDefault();

            if (child == null || userToGivePermission == null)
            {
                return BadRequest();
            }

            var existingPermission = _unitOfWork.ChildPermissionsRepository.Where(cp => cp.Id == permission.Id).FirstOrDefault();
            if (existingPermission == null || existingPermission.Id < 1)
            {
                return await Post(permission);
            }

            existingPermission.Id = permission.Id;
            existingPermission.ChildId = permission.Child.Id;
            existingPermission.IdentityUserId = permission.User.Id;
            existingPermission.ReadOnly = permission.ReadOnly;
            existingPermission.Title = _unitOfWork.TitleRepository.Where(t => t.Name == permission.Title).FirstOrDefault();
            existingPermission.FeedingStatAccess = permission.FeedingStatAccess;
            existingPermission.FeedingWriteAccess = permission.FeedingWriteAccess;
            existingPermission.DiaperChangeWriteAccess = permission.DiaperChangeWriteAccess;
            existingPermission.DiaperStatAccess = permission.DiaperStatAccess;
            existingPermission.SleepWriteAccess = permission.SleepWriteAccess;
            existingPermission.MeasurementWriteAccess = permission.MeasurementWriteAccess;
            existingPermission.ChildManagementWriteAccess = permission.ChildManagementWriteAccess;
            existingPermission.PermissionsWriteAccess = permission.PermissionsWriteAccess;


            _unitOfWork.ChildPermissionsRepository.Update(existingPermission, f => f.Id);
            _unitOfWork.Commit();

            var p = await Convert(existingPermission);
            return Ok(p);
        }

        // DELETE api/<controller>/5
        public IHttpActionResult Delete(int id)
        {
            var permission = _unitOfWork.ChildPermissionsRepository.Where(d => d.Id == id).FirstOrDefault();

            if (permission == null)
            {
                return NotFound();
            }

            var userId = HttpContext.Current.User.Identity.GetUserId();
            var hasWritePermission = _unitOfWork.ChildPermissionsRepository
                .Where(c => c.ChildId == permission.ChildId)
                .Where(c => c.IdentityUserId == userId).Any(c => c.ReadOnly == false);

            if (!hasWritePermission)
                return BadRequest();

            var existingFeed = _unitOfWork.ChildPermissionsRepository.Where(c => c.Id == id).FirstOrDefault();
            if (existingFeed == null)
                return NotFound();

            _unitOfWork.ChildPermissionsRepository.Delete(existingFeed);
            _unitOfWork.Commit();
            return Ok();
        }
    }
}
