using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using cozyjozywebapi.Infrastructure.Core;
using cozyjozywebapi.Models;
using Microsoft.AspNet.Identity;

namespace cozyjozywebapi.Controllers
{
    public class ChildPermissionController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private const int MaxPageSize = 100;

        public ChildPermissionController(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }

        public class Permission
        {
            public int Id { get; set; }
            public class PUser
            {
                public string Id { get; set; }
                public string UserName { get; set; }
            }

            public PUser User { get; set; }

            public Child Child { get; set; }
            public bool ReadOnly { get; set; }
        }

        public IHttpActionResult Get(int childId, int pagesize = 25, int page = 0)
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

                 _unitOfWork.ChildPermissionsRepository.All()
                   .Where(c => c.ChildId == childId)
                   .Where(c => c.IdentityUserId != userId).ToList()
                   .ForEach(c => permissions.Add(Convert(c)));
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

        public IHttpActionResult Post(Permission permission)
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

            var newCp = Convert(userToGivePermission.Id, child.Id, permission.ReadOnly);

            var entity = _unitOfWork.ChildPermissionsRepository.Add(newCp);
            _unitOfWork.Commit();

            var p = Convert(entity);

            var myUri = Request.RequestUri + "/" + p.Id;
            return Created(myUri, p);
        }

        protected Permission Convert(ChildPermissions cp)
        {
            var p = new Permission()
            {
                Id = cp.Id,
                Child = cp.Child,
                ReadOnly = cp.ReadOnly,
                User = new Permission.PUser
                {
                    Id = cp.IdentityUser.Id,
                    UserName = cp.IdentityUser.UserName
                }
            };
            return p;
        }

        protected ChildPermissions Convert(String userId, int childId, bool readO)
        {
            var newCp = new ChildPermissions()
            {
                ChildId = childId,
                IdentityUserId = userId,
                ReadOnly = readO
            };
            return newCp;
        }

        // PUT api/<controller>/5
        public IHttpActionResult Put(Permission permission)
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

            var existingFeed = _unitOfWork.ChildPermissionsRepository.Where(cp => cp.Id == permission.Id).FirstOrDefault();
            if (existingFeed == null || existingFeed.Id < 1)
            {
                return Post(permission);
            }

            existingFeed.Id = permission.Id;
            existingFeed.ChildId = permission.Child.Id;
            existingFeed.IdentityUserId = permission.User.Id;
            existingFeed.ReadOnly = permission.ReadOnly;

            _unitOfWork.ChildPermissionsRepository.Update(existingFeed, f => f.Id);
            _unitOfWork.Commit();
            return Ok(Convert(existingFeed));
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
