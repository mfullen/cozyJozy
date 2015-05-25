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

namespace cozyjozywebapi.Controllers
{
    public class BaseTrackingController : ApiController
    {
        protected readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> UserManager;
        protected const string Authorthizedchildren = "authorthizedChildren";
        protected const int MaxPageSize = 100;

        public BaseTrackingController(IUnitOfWork uow)
        {
            _unitOfWork = uow;
            UserManager = Startup.UserManagerFactory();
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
    }
}
