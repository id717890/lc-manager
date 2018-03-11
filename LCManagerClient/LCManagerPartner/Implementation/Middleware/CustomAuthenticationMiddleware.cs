namespace LCManagerPartner.Implementation.Middleware
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Constants;
    using Microsoft.Owin;

    /// <summary>
    /// Этот посредник обрабатывает все ответы OwinMiddleware
    /// Здесь мы проверяем заголоки (в данном случае создана константа ServerGlobalVariables.OwinStatusFlag, которая добавляется при ошибках авторизации)
    /// Реализация взята здесь https://stackoverflow.com/questions/25032513/how-to-get-error-message-returned-by-dotnetopenauth-oauth2-on-client-side
    /// </summary>
    public class CustomAuthenticationMiddleware : OwinMiddleware
    {
        public CustomAuthenticationMiddleware(OwinMiddleware next)
            : base(next)
        {
        }

        public override async Task Invoke(IOwinContext context)
        {
            await Next.Invoke(context);

            if (context.Response.StatusCode == 400
                && context.Response.Headers.ContainsKey(
                    ServerGlobalVariables.OwinStatusFlag))
            {
                var headerValues = context.Response.Headers.GetValues
                    (ServerGlobalVariables.OwinStatusFlag);

                context.Response.StatusCode =
                    Convert.ToInt16(headerValues.FirstOrDefault());

                context.Response.Headers.Remove(
                    ServerGlobalVariables.OwinStatusFlag);
            }

        }
    }
}