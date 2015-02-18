using System.Web.Mvc;

namespace cozyjozywebapi.Areas.Dashboard
{
    public class DashboardAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Dashboard";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Dashboard_default",
                "Dashboard/{action}/{id}",
                new { controller = "Dashboard", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}