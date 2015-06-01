using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using cozyjozywebapi.Entity;
using Microsoft.AspNet.Identity;

namespace cozyjozywebapi.Filters
{
    public class ChildPermissionFilter : ActionFilterAttribute
    {

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            // pre-processing
            //Debug.WriteLine("ACTION 1 DEBUG pre-processing logging");
            var userId = HttpContext.Current.User.Identity.GetUserId();
            List<int> validChildIds = null;
            using (var context = new CozyJozyContext())
            {
                validChildIds = context.ChildPermissions.Where(w => w.IdentityUserId == userId).Select(x => x.ChildId).ToList();
            }

            if (validChildIds != null && !validChildIds.Any())
            {
                throw new HttpException(403, "Forbidden");
            }
            HttpContext.Current.Items["authorthizedChildren"] = validChildIds;
            //actionContext.ActionArguments["authorthizedChildren"] = validChildIds;
        }
    }
}