namespace Site.Infrastrucure.Request
{
    using System;

    public class GetSendVerificationCodeRequest
    {
        /// <summary>
        /// номер телефона
        /// </summary>
        public long Phone { get; set; }
        /// <summary>
        /// идентификатор Партнера программы лояльности
        /// </summary>
        public Int16 Partner { get; set; }
        /// <summary>
        /// идентификатор Оператора программы лояльности
        /// </summary>
        public Int16 Operator { get; set; }
    }
}
