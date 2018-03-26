namespace LCManagerPartner
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading.Tasks;
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.Infrastructure;

    /// <summary>
    /// Провайдер обеспечивающий генерацию refresh_token
    /// </summary>
    public class AuthRefreshTokenProvider : IAuthenticationTokenProvider
    {
        private static ConcurrentDictionary<string, AuthenticationTicket> _refreshTokens = new ConcurrentDictionary<string, AuthenticationTicket>();

        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            var guid = Guid.NewGuid().ToString();

            // maybe only create a handle the first time, then re-use for same client
            // copy properties and set the desired lifetime of refresh token
            var refreshTokenProperties = new AuthenticationProperties(context.Ticket.Properties.Dictionary)
            {
                IssuedUtc = context.Ticket.Properties.IssuedUtc,
                ExpiresUtc = DateTime.UtcNow.AddYears(1)
            };
            var refreshTokenTicket = new AuthenticationTicket(context.Ticket.Identity, refreshTokenProperties);

            //_refreshTokens.TryAdd(guid, context.Ticket);
            _refreshTokens.TryAdd(guid, refreshTokenTicket);

            // consider storing only the hash of the handle
            context.SetToken(guid);
        }

        /// <summary>
        /// Извлекает токен из хранилища, если он есть
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            //var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");
            //context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            AuthenticationTicket ticket;
            if (_refreshTokens.TryRemove(context.Token, out ticket))
            {
                context.SetTicket(ticket);
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