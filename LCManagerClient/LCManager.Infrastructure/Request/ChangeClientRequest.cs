using System;
using LCManager.Infrastructure.Data;

namespace LCManager.Infrastructure.Request
{
    public class ChangeClientRequest
    {
        /// <summary>
        /// информация о клиенте
        /// </summary>
        public Client ClientData { get; set; }
        /// <summary>
        /// идентификатор Оператора программы лояльности
        /// </summary>
        public Int16 Operator { get; set; }
    }

}
