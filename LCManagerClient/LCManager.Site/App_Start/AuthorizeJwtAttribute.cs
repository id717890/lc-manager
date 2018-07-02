namespace LC_Manager
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Implementation;
    using Controllers;

    public class AuthorizeJwtAttribute : AuthorizeAttribute
    {
        private string[] _allowedRoles = { };
        private List<string> _tokenRoles;

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "Login" }));
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!String.IsNullOrEmpty(base.Roles))
            {
                _allowedRoles = base.Roles.Split(new char[] { ',' });
                for (int i = 0; i < _allowedRoles.Length; i++)
                {
                    _allowedRoles[i] = _allowedRoles[i].Trim();
                }
            }

            //Если токен отсутствует в куках, считаем не авторизованным
            if (!JwtProps.IsExistTokenInCookie()) return false;

            //Если токен истек
            if (JwtProps.IsExpiredToken())
            {
                //1 - проверяем присутствует ли в куках Refresh Token, если нет то считаем не авторизованным
                if (!JwtProps.IsExistRefreshTokenInCookie()) return false;
                //2 - пытаемся обновить токены
                if (!AccountController.RefreshToken(JwtProps.GetUser(), JwtProps.GetRefreshToken()))
                {
                    JwtProps.Logout();
                    return false;
                }
            }
            //Роли указанные в атрибуте
            _tokenRoles = JwtProps.GetRole();
            return Role();
        }

        private bool Role()
        {
            if (_allowedRoles.Length > 0)
            {
                return _allowedRoles.Any(_tokenRoles.Contains);
            }
            return true;
        }
    }
}