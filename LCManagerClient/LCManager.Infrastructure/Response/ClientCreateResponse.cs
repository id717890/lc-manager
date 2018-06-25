namespace Site.Infrastrucure.Response
{
    using System;

    public class ClientCreateResponse
    {
        /// <summary>
        /// код ошибки
        /// </summary>
        public int ErrorCode { get; set; }
        /// <summary>
        /// сообщение об ошибке
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// номер телефона
        /// </summary>
        public Int64 Phone { get; set; }
        /// <summary>
        /// номер карты
        /// </summary>
        public Int64 Card { get; set; }
        /// <summary>
        /// идентификатор участника программы лояльности
        /// </summary>
        public int Client { get; set; }
    }
}
