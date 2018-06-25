namespace Site.Infrastrucure.Request
{
    public class GetConfirmCodeRequest
    {
        /// <summary>
        /// номер телефона
        /// </summary>
        public long Phone { get; set; }
        /// <summary>
        /// код подтверждения
        /// </summary>
        public string Code { get; set; }
    }
}
