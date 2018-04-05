namespace LCManagerPartner
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading.Tasks;
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.Infrastructure;
    using System.Configuration;
    using System.Linq;
    using Implementation.Services;

    /// <summary>
    /// Провайдер обеспечивающий генерацию refresh_token
    /// </summary>
    public class AuthManagerRefreshTokenProvider : IAuthenticationTokenProvider
    {
        private readonly RefreshTokenService _tokenService;

        /// <summary>
        /// Контроллер провайдера, где инициируется TokenService для записи refresh_token в БД
        /// </summary>
        public AuthManagerRefreshTokenProvider()
        {
            _tokenService=new RefreshTokenService();
        }

        private static ConcurrentDictionary<string, AuthenticationTicket> _refreshTokens = new ConcurrentDictionary<string, AuthenticationTicket>();


        /// <summary>
        /// Генерирует новый refresh_token и записывает его в БД.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            //Генерируем refresh_token
            var guid = Guid.NewGuid().ToString();

            var username = context.Ticket.Identity.Claims.SingleOrDefault(x => x.Type== "user");
            if (username?.Value != null)
            {
                //Пишем refresh_token в базу данных
                if (_tokenService.UpdateRefreshTokenForManager(username.Value, guid))
                {
                    //Если токен успешно записан в БД

                    // maybe only create a handle the first time, then re-use for same client
                    // copy properties and set the desired lifetime of refresh token
                    var refreshTokenProperties = new AuthenticationProperties(context.Ticket.Properties.Dictionary)
                    {
                        IssuedUtc = context.Ticket.Properties.IssuedUtc,
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(Convert.ToInt32(ConfigurationManager.AppSettings["expiration_jwt_refresh_token"]))
                    };
                    var refreshTokenTicket = new AuthenticationTicket(context.Ticket.Identity, refreshTokenProperties);
                    _refreshTokens.TryAdd(guid, refreshTokenTicket);
                    context.SetToken(guid);
                }
            }
        }

        /// <summary>
        /// Извлекает токен из хранилища, если он есть
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            if (_tokenService.ReceiveRefreshTokenForManager(context.Token))
            {
                AuthenticationTicket ticket;
                if (_refreshTokens.TryRemove(context.Token, out ticket))
                {
                        context.SetTicket(ticket);
                }
            }
        }

        public void Create(AuthenticationTokenCreateContext context)
        {
            throw new NotImplementedException();
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            throw new NotImplementedException();
        }
    }
}