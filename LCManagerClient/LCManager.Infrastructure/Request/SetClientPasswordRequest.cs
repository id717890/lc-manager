namespace LCManager.Infrastructure.Request
{
    public class SetClientPasswordRequest
    {
        /// <summary>
        /// номер телефона
        /// </summary>
        public long Phone { get; set; }
        /// <summary>
        /// код подтверждения
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// пароль
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// ID участника программы лояльности
        /// </summary>
        public int ClientID { get; set; }
        /// <summary>
        /// ID оператора программы лояльности
        /// </summary>
        public int Operator { get; set; }
    }
}
