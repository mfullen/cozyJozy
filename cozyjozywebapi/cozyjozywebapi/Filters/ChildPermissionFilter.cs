using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using cozyjozywebapi.Entity;
using cozyjozywebapi.Infrastructure;
using cozyjozywebapi.Infrastructure.Core;
using Microsoft.AspNet.Identity;
using Ninject;

namespace cozyjozywebapi.Filters
{
    public class ChildPermissionFilter : ActionFilterAttribute
    {
        private  IUnitOfWork _unitOfWork;
        private readonly CozyJozyContext context = new CozyJozyContext();

        [Inject]
        public void SetUow(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            // pre-processing
            //Debug.WriteLine("ACTION 1 DEBUG pre-processing logging");
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var validChildIds = context.ChildPermissions.Where(w => w.IdentityUserId == userId).Select(x => x.ChildId).ToList();
          
            //var validChildIds = _unitOfWork.ChildPermissionsRepository.All().Where(w => w.IdentityUserId == userId).Select(x => x.ChildId).ToList();

            if (!validChildIds.Any())
            {
                throw new HttpException(403, "Forbidden");
            }
            HttpContext.Current.Items["authorthizedChildren"] = validChildIds;
            //actionContext.ActionArguments["authorthizedChildren"] = validChildIds;
        }
    }
}