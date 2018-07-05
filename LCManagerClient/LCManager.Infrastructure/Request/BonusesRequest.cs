using System;

namespace LCManager.Infrastructure.Request
{
    /// <summary>
    /// Запрос для получения бонусов не за покупки по номеру карты
    /// </summary>
    public class BonusesNotForPurchasesRequest
    {
        /// <summary>
        /// Номер карты
        /// </summary>
        public long Card { get; set; }
        public Int16 Operator { get; set; }
        public Int16 Partner { get; set; }
        public Int16 Pos { get; set; }

        public long Page { get; set; }
        public long PageSize { get; set; }

        public string Date { get; set; }
        public string DateStart { get; set; }
        public string DateEnd { get; set; }
        public string Name { get; set; }
        public string CardStr { get; set; }
        public string Phone { get; set; }
        public string AddedMore { get; set; }
        public string AddedLess { get; set; }
        public string RedeemedMore { get; set; }
        public string RedeemedLess { get; set; }
        public string BurnMore { get; set; }
        public string BurnLess { get; set; }
    }
    
}
