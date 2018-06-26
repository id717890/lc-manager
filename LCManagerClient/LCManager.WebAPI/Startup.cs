using System;
using System.Configuration;
using System.Threading.Tasks;
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
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(Convert.ToInt32(ConfigurationManager.AppSettings["expiration_jwt_token"])),
                Provider = managerLoginProvider,
                AccessTokenFormat = new Implementation.JwtFormat(ConfigurationManager.AppSettings["issuer"]),
                RefreshTokenProvider = new AuthManagerRefreshTokenProvider()
            };
            #endregion

            #region Авторизация по адресу api/ClientLogin
            var clientLoginProvider = new AuthClientProvider();
            OAuthAuthorizationServerOptions optionsClient = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/ClientLogin"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(Convert.ToInt32(ConfigurationManager.AppSettings["expiration_jwt_token"])),
                Provider = clientLoginProvider,
                AccessTokenFormat = new Implementation.JwtFormat(ConfigurationManager.AppSettings["issuer"]),
                RefreshTokenProvider = new AuthClientRefreshTokenProvider()
            };
            #endregion

            app.UseOAuthAuthorizationServer(optionsManager);
            app.UseOAuthAuthorizationServer(optionsClient);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);
        }

        /// <summary>
        /// Провайдер добавляющий в заголовки запросов кастомный ключ. 
        /// Реализовано для работы с ЭВОТОР
        /// </summary>
        private class HeaderOAuthBearerProvider : OAuthBearerAuthenticationProvider
        {
            readonly string _name;

            public HeaderOAuthBearerProvider(string name)
            {
                _name = name;
            }

            public override Task RequestToken(OAuthRequestTokenContext context)
            {
                var value = context.Request.Headers.Get(_name);
                if (!string.IsNullOrEmpty(value))
                {
                    context.Token = value;
                }
                return Task.FromResult<object>(null);
            }

            //public override Task ValidateIdentity(OAuthValidateIdentityContext context)
            //{
            //    return Task.FromResult<object>(null);
            //}
        }
    }
}
