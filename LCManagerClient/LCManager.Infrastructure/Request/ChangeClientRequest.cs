namespace Site.Infrastrucure.Request
{
    using System;
    using Data;

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
