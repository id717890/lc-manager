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
    using Serilog;

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
            Log.Information("LCManagerAPI. CreateAsync start");
            //Генерируем refresh_token
            var guid = Guid.NewGuid().ToString();

            var username = context.Ticket.Identity.Claims.SingleOrDefault(x => x.Type== "user");
            Log.Information("LCManagerAPI. CreateAsync username:{user}",username);
            if (username?.Value != null)
            {
                Log.Information("LCManagerAPI. username не пустой");
                //Пишем refresh_token в базу данных
                if (_tokenService.UpdateRefreshTokenForManager(username.Value, guid))
                {
                    //Если токен успешно записан в БД
                    Log.Information("LCManagerAPI. новый токен {guid} для {user} записан в БД", guid, username);

                    // maybe only create a handle the first time, then re-use for same client
                    // copy properties and set the desired lifetime of refresh token
                    var refreshTokenProperties = new AuthenticationProperties(context.Ticket.Properties.Dictionary)
                    {
                        IssuedUtc = context.Ticket.Properties.IssuedUtc,
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(Convert.ToInt32(ConfigurationManager.AppSettings["expiration_jwt_refresh_token"]))
                    };
                    var refreshTokenTicket = new AuthenticationTicket(context.Ticket.Identity, refreshTokenProperties);
                    if (_refreshTokens.TryAdd(guid, refreshTokenTicket))
                    {
                        Log.Information("LCManagerAPI. новый токен {guid} для {user} записан в словарь. ИТОГО:{count}", guid, username, _refreshTokens.Count());
                    }
                    else
                    {
                        Log.Error("LCManagerAPI. новый токен {guid} для {user} ОШИБКА ЗАПИСИ в словарь", guid, username);
                    }
                    context.SetToken(guid);
                }
                else
                {
                    Log.Error("LCManagerAPI. новый токен {guid} для {user} ОШИБКА ЗАПИСИ в БД", guid, username);
                }
            }
            else
            {
                Log.Error("LCManagerAPI. ОШИБКА. username пустой");
            }
        }

        /// <summary>
        /// Извлекает токен из хранилища, если он есть
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            Log.Information("LCManagerAPI. ReceiveAsync start.{token}", context.Token);
            if (_tokenService.ReceiveRefreshTokenForManager(context.Token))
            {
                Log.Information("LCManagerAPI. Токен {token} найден в БД", context.Token);
                AuthenticationTicket ticket;
                if (_refreshTokens.TryRemove(context.Token, out ticket))
                {
                    Log.Information("LCManagerAPI. Токен {token} извлечен из словаря ", context.Token);
                    context.SetTicket(ticket);
                }
                else
                {
                    Log.Information("LCManagerAPI. Токенов в словаре {count} ", _refreshTokens.Count());
                    Log.Information("LCManagerAPI. Наличие токена {token} в словаре = {state}", context.Token,_refreshTokens.Any(x=>x.Key ==context.Token));
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