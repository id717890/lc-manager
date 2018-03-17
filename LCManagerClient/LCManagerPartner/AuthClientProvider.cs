namespace LCManagerPartner
{
    using Microsoft.Owin.Security.OAuth;
    using System;
    using System.Threading.Tasks;
    using System.Security.Claims;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Net;
    using Implementation.Constants;
    using Models;

    /// <inheritdoc />
    /// <summary>
    /// ClientLogin
    /// </summary>
    public class AuthClientProvider: OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            string connectionString = ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString;
            SqlConnection cnn = new SqlConnection(connectionString);

            ClientLoginRequest request = new ClientLoginRequest
            {
                Login = Convert.ToInt64(context.UserName),
                Password = context.Password
            };

            var result = new ServerClientLoginResponse();
            var authentificationResult = result.ProcessRequest(cnn, request);
            if (authentificationResult.ErrorCode == 0)
            {
                //identity.AddClaim(new Claim(ClaimTypes.Role, authentificationResult.ClientID));
                //identity.AddClaim(new Claim("username", context.UserName));
                identity.AddClaim(new Claim("user", context.UserName));
                context.Validated(identity);
            }
            else
            {
                //Пишем текст ошибки
                context.SetError("invalid_grant", "Provided username and password is incorrect");

                //Добавляем в заголовок наш флаг (константу), он будет проверен посредником CustomAuthenticationMiddleware
                context.Response.Headers.Add(ServerGlobalVariables.OwinStatusFlag, new[] { ((int)HttpStatusCode.Unauthorized).ToString() });
            }
        }
    }
}