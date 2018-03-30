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
    using System.Linq;

    /// <inheritdoc />
    /// <summary>
    /// ClientLogin
    /// </summary>
    public class AuthClientProvider : OAuthAuthorizationServerProvider
    {
        /// <inheritdoc />
        /// <summary>
        /// Валидация запроса на присутствие необходимых данных для авторизации
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            #region Проверяем пришедший запрос на наличие в нем параметра "operator"
            var operatorId = context.Parameters.SingleOrDefault(f => f.Key == "operator");
            if (operatorId.Key != null && operatorId.Value != null)
            {
                try
                {
                    context.OwinContext.Set("operator", operatorId.Value[0]);
                }
                catch (Exception)
                {
                    // ignored
                }
            }
            else
            {
                context.SetError("invalid_operator", "Request do not contain ID of operator");
                return;
            } 
            #endregion
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
                Password = context.Password,
                Operator = Convert.ToInt16(context.OwinContext.Get<string>("operator"))
            };

            var result = new ServerClientLoginResponse();
            var authentificationResult = result.ProcessRequest(cnn, request);
            if (authentificationResult.ErrorCode == 0)
            {
                //identity.AddClaim(new Claim(ClaimTypes.Role, authentificationResult.ClientID));
                //identity.AddClaim(new Claim("username", context.UserName));
                identity.AddClaim(new Claim("clientId", authentificationResult.ClientID.ToString()));
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