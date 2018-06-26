namespace Site.Infrastrucure.Request
{
    using System;

    public class ClientCreateRequest
    {
        /// <summary>
        /// идентификатор Оператора программы лояльности
        /// </summary>
        public Int16 Operator { get; set; }
        /// <summary>
        /// идентификатор Партнера программы лояльноси
        /// </summary>
        public Int16 Partner { get; set; }
        /// <summary>
        /// номер карты
        /// </summary>
        public Int64 Card { get; set; }
        /// <summary>
        /// номер телефона
        /// </summary>
        public Int64 Phone { get; set; }
        /// <summary>
        /// имя
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// фамилия
        /// </summary>
        public string Surname { get; set; }
        /// <summary>
        /// отчество
        /// </summary>
        public string Patronymic { get; set; }
        /// <summary>
        /// адрес электронной почты
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// дата рождения
        /// </summary>
        public DateTime? Birthdate { get; set; }
        /// <summary>
        /// получать уведомления по SMS
        /// </summary>
        public bool AllowSms { get; set; }
        /// <summary>
        /// получать уведомления по E-mail
        /// </summary>
        public bool AllowEmail { get; set; }
        /// <summary>
        /// пол (1 - муж., -1 - жен., 0 - не определен)
        /// </summary>
        public int Gender { get; set; }
        /// <summary>
        /// согласие на обработку персональных данных
        /// </summary>
        public bool AgreePersonalData { get; set; }
        /// <summary>
        /// код торговой точки
        /// </summary>
        public string PosCode { get; set; }
        /// <summary>
        /// телефон друга/подруги для механики “Приведи друга”
        /// </summary>
        public Int64? FriendPhone { get; set; }
        /// <summary>
        /// пароль клиента
        /// </summary>
        public bool ClientSetPassword { get; set; }
        /// <summary>
        /// промокод
        /// </summary>
        public string Promocode { get; set; }
    }
}
