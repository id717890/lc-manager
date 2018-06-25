namespace Site.Infrastrucure.Data
{
    using System;

    public class Bonus
    {
        /// <summary>
        /// источник бонусов
        /// </summary>
        public string BonusSource { get; set; }
        /// <summary>
        /// дата зачисления
        /// </summary>
        public DateTime BonusDate { get; set; }
        /// <summary>
        /// начислено баллов
        /// </summary>
        public decimal BonusAdded { get; set; }
        /// <summary>
        /// списано баллов
        /// </summary>
        public decimal BonusRedeemed { get; set; }
        /// <summary>
        /// сгоревшие баллы
        /// </summary>
        public decimal BonusBurn { get; set; }
        /// <summary>
        /// Номер карты на которую начислены бонусы
        /// </summary>
        public long BonusCard { get; set; }
    }
}
