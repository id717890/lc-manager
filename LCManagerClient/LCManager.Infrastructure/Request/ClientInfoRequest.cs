namespace Site.Infrastrucure.Request
{
    public class ClientInfoRequest
    {
        /// <summary>
        /// ID участника программы лояльности
        /// </summary>
        public int ClientId { get; set; }
        /// <summary>
        /// ID оператора программы лояльности
        /// </summary>
        public int OperatorId { get; set; }
        /// <summary>
        /// Последняя покупка?
        /// </summary>
        public bool LastPurchase { get; set; }
    }
}
