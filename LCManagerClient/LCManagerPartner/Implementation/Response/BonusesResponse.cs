namespace LCManagerPartner.Implementation.Response
{
    using Abstractions;

    public class BonusesResponse : BaseResponse
    {
        /// <summary>
        /// Начислено бонусов
        /// </summary>
        public decimal AddedBonus { get; set; }
        /// <summary>
        /// Кол-во начислени
        /// </summary>
        public decimal AddedBonusCount { get; set; }
        /// <summary>
        /// Среднее начисление
        /// </summary>
        public decimal AvgCharge { get; set; }
        /// <summary>
        /// Списано бонусов
        /// </summary>
        public decimal RedeemedBonus { get; set; }
        /// <summary>
        /// Кол-во списаний
        /// </summary>
        public decimal RedeemedBonusCount { get; set; }
        /// <summary>
        /// Среднее списание
        /// </summary>
        public decimal AvgRedeem { get; set; }
        /// <summary>
        /// Средний баланс
        /// </summary>
        public decimal AvgBalance { get; set; }
        /// <summary>
        /// Фактическая скидка
        /// </summary>
        public decimal AvgDiscount { get; set; }
        /// <summary>
        /// Кол-во клиентов
        /// </summary>
        public decimal ClientCount { get; set; }
    }
}