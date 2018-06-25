namespace Site.Infrastrucure.Request
{
    using Microsoft.Build.Framework;
    using System;

    public class ClientPasswordChangeRequest
    {
        /// <summary>
        /// стары пароль
        /// </summary>
        public string OldPassword { get; set; }
        /// <summary>
        /// новый пароль
        /// </summary>
        public string NewPassword { get; set; }
        /// <summary>
        /// ID клиента
        /// </summary>
        public int Client { get; set; }
        /// <summary>
        /// ID оператора
        /// </summary>
        [Required]
        public Int16 Operator { get; set; }
    }
}
