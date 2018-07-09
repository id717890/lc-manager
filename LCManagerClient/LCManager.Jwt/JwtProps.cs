using System;
using System.Collections.Generic;
using System.Web;
using Site.Infrastrucure;

namespace LCManager.JWT
{
    public class JwtProps
    {
        #region Public methods

        /// <summary>
        /// Получает payload из токена
        /// </summary>
        /// <returns></returns>
        public static dynamic GetPayloadFromToken()
        {
            //var v1 = JwtProvider.GetInstance();
            //var v2 = ReadJwtTokenFromCookie();
            //var v3 = v1.DecodePayload(v2.Value);
            return !IsExistTokenInCookie() ? null : JwtProvider.GetInstance().DecodePayload(ReadJwtTokenFromCookie().Value);
        }

        /// <summary>
        /// Получает токен из куков
        /// </summary>
        /// <returns></returns>
        public static string GetToken()
        {
            return !IsExistTokenInCookie() ? string.Empty : HttpContext.Current.Request.Cookies[Config.GetNameCookieOfAccessToken()].Value;
        }

        /// <summary>
        /// Получает refresh_token из куков браузера
        /// </summary>
        /// <returns></returns>
        public static string GetRefreshToken()
        {
            var cookie = HttpContext.Current.Request.Cookies[Config.GetNameCookieOfRefreshToken()];
            return cookie?.Value;
        }

        /// <summary>
        /// Проверяем есть ли token в куках
        /// </summary>
        /// <returns></returns>
        public static bool IsExistTokenInCookie()
        {
            //var cookie = new HttpCookie("lcmanageruserdata")
            //{
            //    Expires = DateTime.Now.AddDays(-1d)
            //};
            //HttpContext.Current.Request.Cookies.Add(cookie);
            var token = HttpContext.Current.Request.Cookies[Config.GetNameCookieOfAccessToken()];
            if (token?.Value == null) return false;
            return token.Value != string.Empty;
        }

        /// <summary>
        /// Проверяем есть ли token в куках
        /// </summary>
        /// <returns></returns>
        public static bool IsExistRefreshTokenInCookie()
        {
            var token = HttpContext.Current.Request.Cookies[Config.GetNameCookieOfRefreshToken()];
            if (token?.Value == null) return false;
            return token.Value != string.Empty;
        }

        public static HttpCookie ReadJwtTokenFromCookie()
        {
            return HttpContext.Current.Request.Cookies[Config.GetNameCookieOfAccessToken()];
        }

        /// <summary>
        /// Проверка, истек ли срок действия токена
        /// </summary>
        /// <returns></returns>
        public static bool IsExpiredToken()
        {
            long expiredSeconds = 0;
            var payload = GetPayloadFromToken();
            if (long.TryParse(payload.exp.ToString(), out long value)) expiredSeconds = value;
            var expired = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).ToUniversalTime().AddSeconds(expiredSeconds);
            var now = DateTime.Now.ToUniversalTime();
            return expired < now;
            //return expired < DateTime.Now.ToUniversalTime();
        }

        /// <summary>
        /// При выходе из ЛК удаляем куки содержащий токен
        /// </summary>
        public static void Logout()
        {
            HttpContext.Current.Response.SetCookie(new HttpCookie(Config.GetNameCookieOfAccessToken()) { Expires = DateTime.Now.AddDays(-1) });
            HttpContext.Current.Response.SetCookie(new HttpCookie(Config.GetNameCookieOfRefreshToken()) { Expires = DateTime.Now.AddDays(-1) });
        }

        public static List<string> GetRole()
        {
            var role = new List<string>();
            var payload = GetPayloadFromToken();
            if (!string.IsNullOrEmpty(payload.role) && !string.IsNullOrEmpty(payload.role.ToString()))
            {
                //Сделал так, потому что если у пользователя 1 роль, то DeserializeObject выдает ошибку, почему то не парсит
                //Подозрение в том, что если роль одна то приходит строка, если ролей много приходит массив
                try
                {
                    role = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(payload.role.ToString());
                }
                catch (Exception)
                {
                    role = new List<string> { payload.role.ToString() };
                }
            }
            return role;
        }

        public static string GetUser()
        {
            var user = string.Empty;
            var payload = GetPayloadFromToken();
            if (!string.IsNullOrEmpty(payload.user.ToString())) user = payload.user.ToString();
            return user;
        }

        public static short GetOperator()
        {
            short oper = 0;
            var payload = GetPayloadFromToken();
            if (short.TryParse(payload.oper.ToString(), out short value)) oper = value;
            return oper;
        }

        public static int GetClient()
        {
            int client = 0;
            var payload = GetPayloadFromToken();
            if (int.TryParse(payload.client.ToString(), out int value)) client = value;
            return client;
        }

        public static short GetPartner()
        {
            short partner = 0;
            var payload = GetPayloadFromToken();
            try
            {
                if (short.TryParse(payload.partner.ToString(), out short value)) partner = value;
            }
            catch { }
            return partner;
        }

        public static short GetPos()
        {
            short pos = 0;
            var payload = GetPayloadFromToken();
            try
            {
                if (short.TryParse(payload.pos.ToString(), out short value)) pos = value;
            }
            catch { }
            return pos;
        }


        public static string GetPosCode()
        {
            var posCode = string.Empty;
            var payload = GetPayloadFromToken();
            try
            {
                if (!string.IsNullOrEmpty(payload.poscode.ToString())) posCode = payload.poscode.ToString();
            }
            catch { }
            return posCode;
        }

        public static short GetDefaultPartner()
        {
            short defaultPartner = 0;
            var payload = GetPayloadFromToken();
            try
            {
                if (short.TryParse(payload.defaultpartner.ToString(), out short value)) defaultPartner = value;
            }
            catch { }
            return defaultPartner;
        }

        public static short GetDefaultPos()
        {
            short defaultPos = 0;
            var payload = GetPayloadFromToken();
            try
            {
                if (short.TryParse(payload.defaultpos.ToString(), out short value)) defaultPos = value;
            }
            catch { }
            return defaultPos;
        }

        public static string GetDefaultPosCode()
        {
            var defaultPosCode = string.Empty;
            var payload = GetPayloadFromToken();
            try
            {
                if (!string.IsNullOrEmpty(payload.defaultposcode.ToString())) defaultPosCode = payload.defaultposcode.ToString();
            }
            catch { }
            return defaultPosCode;
        }

        public static string GetPermissionCode()
        {
            var permission = string.Empty;
            var payload = GetPayloadFromToken();
            if (!string.IsNullOrEmpty(payload.permissioncode.ToString())) permission = payload.permissioncode.ToString();
            return permission;
        }
        #endregion
    }
}