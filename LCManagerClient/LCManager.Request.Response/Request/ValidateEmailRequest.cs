namespace Site.Infrastrucure.Request
{
    public class ValidateEmailRequest
    {
        /// <summary>
        /// электронная почта
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// полученный в сообщении проверочный код
        /// </summary>
        public string Code { get; set; }
    }
}
