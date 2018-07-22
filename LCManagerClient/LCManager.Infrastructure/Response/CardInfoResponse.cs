namespace LCManager.Infrastructure.Response
{
    public class CardInfoResponse: BaseResponse
    {
        /// <summary>
        /// статус карты
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// тип карты
        /// </summary>
        public bool Type { get; set; }
        /// <summary>
        /// наименование оператора
        /// </summary>
        public string OperatorName { get; set; }
        /// <summary>
        /// баланс карты
        /// </summary>
        public decimal Balance { get; set; }
        /// <summary>
        /// полный баланс?
        /// </summary>
        public decimal FullBalance { get; set; }
        /// <summary>
        /// количество покупок
        /// </summary>
        public int Purchases { get; set; }
        /// <summary>
        /// сумма покупок
        /// </summary>
        public decimal PurchaseSum { get; set; }
        
        /// <summary>
        /// начисленно балов
        /// </summary>
        public decimal Charged { get; set; }
        /// <summary>
        /// списано баллов
        /// </summary>
        public decimal Redeemed { get; set; }
    }
}
