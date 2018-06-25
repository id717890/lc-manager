namespace Site.Infrastrucure.Data
{
    using System;

    public class Client
    {
        /// <summary>
        /// ID участника программы лояльности
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// Пароль
        /// </summary>
        public string password { get; set; }
        /// <summary>
        /// Имя
        /// </summary>
        public string firstname { get; set; }
        /// <summary>
        /// Отчество
        /// </summary>
        public string middlename { get; set; }
        /// <summary>
        /// Фамилия
        /// </summary>
        public string lastname { get; set; }
        /// <summary>
        /// Полу
        /// </summary>
        public int gender { get; set; }
        /// <summary>
        /// Дата рождения
        /// </summary>
        public DateTime? birthdate { get; set; }
        /// <summary>
        /// Адрес
        /// </summary>
        public string address { get; set; }
        /// <summary>
        /// Есть дети?
        /// </summary>
        public bool haschildren { get; set; }
        /// <summary>
        /// Описание
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// Номер телефона
        /// </summary>
        public Int64 phone { get; set; }
        /// <summary>
        /// Адрес электронной почты
        /// </summary>
        public string email { get; set; }
        /// <summary>
        /// получать уведомления по SMS
        /// </summary>
        public bool allowsms { get; set; }
        /// <summary>
        /// получать уведомления по E-mail
        /// </summary>
        public bool allowemail { get; set; }
        /// <summary>
        /// активный баланс бонусных баллов
        /// </summary>
        public decimal balance { get; set; }
        /// <summary>
        /// получать push-уведомления
        /// </summary>
        public bool? allowpush { get; set; }
        /// <summary>
        /// сумма последней покупки
        /// </summary>
        public decimal lasturchaseamount { get; set; }
        /// <summary>
        /// дата последней покупки
        /// </summary>
        public DateTime lastpurchasedate { get; set; }
        /// <summary>
        /// Номер карты
        /// </summary>
        public Int64 CardNumber { get; set; }
        /// <summary>
        /// Статус карты
        /// </summary>
        public string CardStatus { get; set; }
        /// <summary>
        /// Общий баланс карты
        /// </summary>
        public decimal CardFullBalance { get; set; }
        public Client() { }
    }
}
