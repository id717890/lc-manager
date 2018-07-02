using System;

namespace LCManager.Infrastructure.Request
{
    public class GetClientInfoRequest
    {
        /// <summary>
        /// идентификатор Оператора программы лояльности
        /// </summary>
        public Int16 Operator { get; set; }
        /// <summary>
        /// номер карты
        /// </summary>
        public Int64 Card { get; set; }
        /// <summary>
        /// номер телефона
        /// </summary>
        public Int64 Phone { get; set; }
    }
}
