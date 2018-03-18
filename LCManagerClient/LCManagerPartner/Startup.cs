using System;
using System.Configuration;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.OAuth;
using System.Web.Http;

[assembly: OwinStartup(typeof(LCManagerPartner.Startup))]

namespace LCManagerPartner
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureOAuth(app);
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
            //enable cors origin requests
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            #region Авторизация по адресу api/ManagerLogin
            var managerLoginProvider = new AuthManagerProvider();
            OAuthAuthorizationServerOptions optionsManager = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/ManagerLogin"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(Convert.ToInt32(ConfigurationManager.AppSettings["expiration"])),
                Provider = managerLoginProvider,
                AccessTokenFormat = new Implementation.JwtFormat(ConfigurationManager.AppSettings["issuer"])
            };
            #endregion

            #region Авторизация по адресу api/ClientLogin
            var clientLoginProvider = new AuthClientProvider();
            OAuthAuthorizationServerOptions optionsClient = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/ClientLogin"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(Convert.ToInt32(ConfigurationManager.AppSettings["expiration"])),
                Provider = clientLoginProvider,
                AccessTokenFormat = new Implementation.JwtFormat(ConfigurationManager.AppSettings["issuer"])
            };
            #endregion

            app.UseOAuthAuthorizationServer(optionsManager);
            app.UseOAuthAuthorizationServer(optionsClient);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);
        }
    }
}
