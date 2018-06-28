using System;

namespace LCManager.Infrastructure.Data
{
    public class CardBonus
    {
        /// <summary>
        /// номер карты
        /// </summary>
        public Int32 Id { get; set; }
        /// <summary>
        /// размер начисления
        /// </summary>
        public decimal Bonus { get; set; }
        /// <summary>
        /// тип бонуса
        /// </summary>
        public string BonusType { get; set; }
        /// <summary>
        /// время начисления
        /// </summary>
        public DateTime BonusTime { get; set; }
    }
}
