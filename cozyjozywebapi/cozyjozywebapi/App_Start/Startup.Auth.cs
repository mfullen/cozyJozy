using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.WebPages;
using cozyjozywebapi.Entity;
using cozyjozywebapi.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;
using Owin;
using cozyjozywebapi.Providers;
using Owin.Security.Providers.OpenID;
using Owin.Security.Providers.Reddit;
using Owin.Security.Providers.Yahoo;

namespace cozyjozywebapi
{
    public partial class Startup
    {
        static Startup()
        {
            PublicClientId = "self";

            UserManagerFactory = () => new UserManager<User>(new UserStore<User>(new CozyJozyContext()));


            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/Token"),
                Provider = new ApplicationOAuthProvider(PublicClientId, UserManagerFactory),
                AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
                AllowInsecureHttp = true
            };
        }

        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        public static Func<UserManager<User>> UserManagerFactory { get; set; }

        public static string PublicClientId { get; private set; }

        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseCookieAuthentication(new CookieAuthenticationOptions());
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enable the application to use bearer tokens to authenticate users
            app.UseOAuthBearerTokens(OAuthOptions);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            var twitterSecret = ConfigurationManager.AppSettings["twitterSecretKey"];
            var twitterconsumer = ConfigurationManager.AppSettings["twitterConsumerKey"];

            if (!(twitterconsumer.IsEmpty() && twitterSecret.IsEmpty()))
            {
                app.UseTwitterAuthentication(
                    consumerKey: twitterconsumer,
                    consumerSecret: twitterSecret);
            }


            var fbsecret = ConfigurationManager.AppSettings["facebookSecretKey"];
            var fbconsumer = ConfigurationManager.AppSettings["facebookConsumerKey"];

            if (!(fbconsumer.IsEmpty() && fbsecret.IsEmpty()))
            {
                app.UseFacebookAuthentication(new FacebookAuthenticationOptions
               {
                   AppId = fbconsumer,
                   AppSecret = fbsecret,
                   Scope = { "email" },
                   Provider = new FacebookAuthenticationProvider()
                   {
                       OnAuthenticated = async (context) =>
                       {
                           context.Identity.AddClaim(new System.Security.Claims.Claim("FacebookAccessToken", context.AccessToken));
                           foreach (var claim in context.User)
                           {
                               var claimType = string.Format("urn:facebook:{0}", claim.Key);
                               string claimValue = claim.Value.ToString();
                               if (!context.Identity.HasClaim(claimType, claimValue))
                                   context.Identity.AddClaim(new System.Security.Claims.Claim(claimType, claimValue, "XmlSchemaString", "Facebook"));
                           }
                       }
                   }
               });
            }

            #region google login
            var googleSecret = ConfigurationManager.AppSettings["googleSecretKey"];
            var googleconsumer = ConfigurationManager.AppSettings["googleConsumerKey"];

            if (!(googleconsumer.IsEmpty() && googleSecret.IsEmpty()))
            {
                app.UseGoogleAuthentication(
                clientId: googleconsumer,
                clientSecret: googleSecret);
            }
            #endregion

            #region yahoo login
            var yahooSecret = ConfigurationManager.AppSettings["yahooSecretKey"];
            var yahooconsumer = ConfigurationManager.AppSettings["yahooConsumerKey"];

            if (!(yahooconsumer.IsEmpty() && yahooSecret.IsEmpty()))
            {
                //app.UseYahooAuthentication(
                //consumerKey: yahooconsumer,
                //consumerSecret: yahooSecret);
                app.UseOpenIDAuthentication("http://me.yahoo.com/", "Yahoo");
            }

            #endregion

            #region reddit login
            var redditSecret = ConfigurationManager.AppSettings["redditSecretKey"];
            var redditconsumer = ConfigurationManager.AppSettings["redditConsumerKey"];

            if (!(redditconsumer.IsEmpty() && redditSecret.IsEmpty()))
            {
                var redditOptions = new RedditAuthenticationOptions()
                {
                    ClientId = redditconsumer,
                    ClientSecret = redditSecret,
                    CallbackPath = new PathString("/Account/ExternalLogin")
                };
                app.UseRedditAuthentication(redditOptions);
            }
            #endregion

 


        }
    }
}
