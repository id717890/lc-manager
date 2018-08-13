using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Site.Infrastrucure
{
    public class Config
    {
        /// <summary>
        /// Получает из конфигурации название дополнительного кастомного заголовка куда будет записываться ключ JWT
        /// *На сервере АПИ также должен быть настроен этот заголовок для проверки в нем токена
        /// </summary>
        /// <returns></returns>
        public static string GetCustomHeaderAuthorization()
        {
            return ConfigurationManager.AppSettings["HeaderAuthorization"];
        }

        /// <summary>
        /// Получает из конфигурации ID оператора
        /// </summary>
        /// <returns></returns>
        public static string GetOperator()
        {
            return ConfigurationManager.AppSettings["OperatorId"];
        }

        /// <summary>
        /// Получает из конфигурации срок действия cookie для jwt
        /// </summary>
        /// <returns></returns>
        public static int GetExpirationForCookieOfAccessToken()
        {
            return Convert.ToInt32(ConfigurationManager.AppSettings["jwt_token_cookie_expiration"]);
        }

        /// <summary>
        /// Получает издателя токена jwt
        /// </summary>
        /// <returns></returns>
        public static string GetIssuer()
        {
            return ConfigurationManager.AppSettings["issuer"];
        }

        /// <summary>
        /// Получает из конфигурации срок действия cookie для jwt refresh token
        /// </summary>
        /// <returns></returns>
        public static int GetExpirationForCookieOfRefreshToken()
        {
            return Convert.ToInt32(ConfigurationManager.AppSettings["jwt_refresh_token_cookie_expiration"]);
        }

        /// <summary>
        /// Получает из конфигурации название cookie для jwt refresh token
        /// </summary>
        /// <returns></returns>
        public static string GetNameCookieOfRefreshToken()
        {
            return ConfigurationManager.AppSettings["RefreshTokenName"];
        }

        /// <summary>
        /// Получает из конфигурации название cookie для jwt
        /// </summary>
        /// <returns></returns>
        public static string GetNameCookieOfAccessToken()
        {
            return ConfigurationManager.AppSettings["AccessTokenName"];
        }

        /// <summary>
        /// Получает из конфигурации url api сервиса
        /// </summary>
        /// <returns></returns>
        public static string GetApiUrl()
        {
            return ConfigurationManager.AppSettings["WebAPI"];
        }

        /// <summary>
        /// Получает из конфигурации PosCode
        /// </summary>
        /// <returns></returns>
        public static string GetPosCode()
        {
            return ConfigurationManager.AppSettings["PosCode"];
        }

        /// <summary>
        /// Получает из конфигурации Pos id
        /// </summary>
        /// <returns></returns>
        public static string GetPos()
        {
            return ConfigurationManager.AppSettings["Pos"];
        }
    }
}
