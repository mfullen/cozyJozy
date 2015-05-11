using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace cozyjozywebapi
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //filters.Add(new System.Web.Http.AuthorizeAttribute());
           // filters.Add(new HostAuthenticationFilter(Startup.OAuthOptions.AuthenticationType));
            filters.Add(new RequireHttpsAttribute());
        }
    }
}
