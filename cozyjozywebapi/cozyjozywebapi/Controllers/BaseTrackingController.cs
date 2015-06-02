using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using cozyjozywebapi.Infrastructure.Core;
using cozyjozywebapi.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Practices.Unity;

namespace cozyjozywebapi.Controllers
{
    public class BaseTrackingController : ApiController
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected const string Authorthizedchildren = "authorthizedChildren";
        protected const int MaxPageSize = 100;
        [Dependency]
        public UserManager<User> UserManager { get; set; }

        public BaseTrackingController(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }

        protected async Task<UserRestModel> GetById(string userId, int childId)
        {
            var user = _unitOfWork.UserRepository.GetById(userId);
            var title = _unitOfWork.ChildPermissionsRepository
                .Where(c => c.ChildId == childId)
                .Where(u => u.IdentityUserId == user.Id)
                .Select(t => t.Title.Name).FirstOrDefault();

            var model = new UserRestModel
            {
                Email = user.Email,
                Id = user.Id,
                UserName = user.UserName,
                Title = title
            };

            model.ProfileImageUrl = await ExternalAccountHelper.GetProfileImageUrl(UserManager, model.Id);
            return model;
        }

        protected async Task<Permission> Convert(ChildPermissions cp)
        {
            var u = await GetById(cp.IdentityUserId, cp.ChildId);
            var p = new Permission()
            {
                Id = cp.Id,
                Child = cp.Child,
                ReadOnly = cp.ReadOnly,
                User = u,
                Title = cp.Title.Name,
                FeedingStatAccess = cp.FeedingStatAccess,
                FeedingWriteAccess = cp.FeedingWriteAccess,
                DiaperChangeWriteAccess = cp.DiaperChangeWriteAccess,
                DiaperStatAccess = cp.DiaperStatAccess,
                SleepWriteAccess = cp.SleepWriteAccess,
                MeasurementWriteAccess = cp.MeasurementWriteAccess,
                ChildManagementWriteAccess = cp.ChildManagementWriteAccess,
                PermissionsWriteAccess = cp.PermissionsWriteAccess
            };
            return p;
        }

        protected ChildPermissions Convert(string userId, int childId, Permission permission)
        {
            var newCp = new ChildPermissions()
            {
                ChildId = childId,
                IdentityUserId = userId,
                ReadOnly = permission.ReadOnly,
                Title = _unitOfWork.TitleRepository.Where(t => t.Name == permission.Title).FirstOrDefault(),
                FeedingStatAccess = permission.FeedingStatAccess,
                FeedingWriteAccess = permission.FeedingWriteAccess,
                DiaperChangeWriteAccess = permission.DiaperChangeWriteAccess,
                DiaperStatAccess = permission.DiaperStatAccess,
                SleepWriteAccess = permission.SleepWriteAccess,
                MeasurementWriteAccess = permission.MeasurementWriteAccess,
                ChildManagementWriteAccess = permission.ChildManagementWriteAccess,
                PermissionsWriteAccess = permission.PermissionsWriteAccess
            };
            return newCp;
        }
    }
}
