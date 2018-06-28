using System;

namespace LCManager.Infrastructure.Request
{
    public class SendEmailCodeRequest
    {
        /// <summary>
        /// адрес электронной почты участника программы лояльности
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Идентификатор оператора программы лояльности
        /// </summary>
        public Int16 Operator { get; set; }
    }
}
