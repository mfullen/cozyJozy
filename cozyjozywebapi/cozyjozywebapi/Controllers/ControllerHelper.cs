using System;
using System.Linq;
using System.Threading.Tasks;
using cozyjozywebapi.Models;
using Microsoft.AspNet.Identity;


namespace cozyjozywebapi.Controllers
{
    public class UserRestModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string ProfileImageUrl { get; set; }
        public string Title { get; set; }
    }

    public class ExternalAccountHelper
    {
        public static async Task<string> GetProfileImageUrl(UserManager<User> usermanager, string userId)
        {
            string profileImageUrl = null;
            try
            {
                var logins = await usermanager.GetLoginsAsync(userId);

                var fbLogin = logins.FirstOrDefault(l => l.LoginProvider.Equals("Facebook"));
                if (fbLogin != null)
                {
                    profileImageUrl = "https://graph.facebook.com/" + fbLogin.ProviderKey + "/picture";
                }
            }
            catch
            {
                //do nothing
            }
            return profileImageUrl;
        }
    }

}