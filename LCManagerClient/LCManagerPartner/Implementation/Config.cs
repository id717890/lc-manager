namespace LCManagerPartner.Implementation
{
    using System.Configuration;

    /// <summary>
    /// 
    /// </summary>
    public class Config
    {
        /// <summary>
        /// Получает из конфигурации название дополнительного кастомного заголовка где может храниться ключ для JWT авторизации
        /// </summary>
        /// <returns></returns>
        public static string GetCustomHeaderAuthorization()
        {
            return ConfigurationManager.AppSettings["HeaderAuthorization"];
        }
    }
}