using System;
using Microsoft.Build.Framework;

namespace LCManager.Infrastructure.Request
{
    public class AddEmailRequest
    {
        /// <summary>
        /// ID участника программы лояльности
        /// </summary>
        public int ClientID { get; set; }
        /// <summary>
        /// адрес электронной почты участника программы лояльности
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// ID оператора
        /// </summary>
        [Required]
        public Int16 Operator { get; set; }
    }
}
