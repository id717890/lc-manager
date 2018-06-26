namespace Site.Infrastrucure.Request
{
    using System;

    public class GetRegistrationUserRequest
    {
        /// <summary>
        /// номер телефона
        /// </summary>
        public long Phone { get; set; }
        /// <summary>
        /// номер карты
        /// </summary>
        public long Card { get; set; }
        /// <summary>
        /// идентификатор Партнера программы лояльности
        /// </summary>
        public int PartnerID { get; set; }
        /// <summary>
        /// код торговой точки
        /// </summary>
        public string PosCode { get; set; }
        /// <summary>
        /// согласие на обработку персональных данных
        /// </summary>
        public string AgreePersonalData { get; set; }
        /// <summary>
        /// идентификатор Оператора программы лояльности
        /// </summary>
        public Int16 Operator { get; set; }
        /// <summary>
        /// телефон друга/подруги для механики “Приведи друга”
        /// </summary>
        public Int64? FriendPhone { get; set; }
        /// <summary>
        /// задать пароль?
        /// </summary>
        public bool ClientSetPassword { get; set; }
        /// <summary>
        /// адрес электронной почты
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// промокод
        /// </summary>
        public string Promocode { get; set; }
    }
}
