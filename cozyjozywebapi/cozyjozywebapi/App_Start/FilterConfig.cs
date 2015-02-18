using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using AuthorizeAttribute = System.Web.Mvc.AuthorizeAttribute;

namespace cozyjozywebapi
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //filters.Add(new AuthorizeAttribute());
           // filters.Add(new HostAuthenticationFilter(Startup.OAuthOptions.AuthenticationType));
            filters.Add(new RequireHttpsAttribute());
        }
    }
}
