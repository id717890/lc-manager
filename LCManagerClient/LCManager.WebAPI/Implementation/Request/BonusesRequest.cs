namespace LCManagerPartner.Implementation.Request
{
    using System;

    public class BonusesRequest
    {
        public Int16 Operator { get; set; }

        public Int16 Partner { get; set; }

        public Int16 Pos { get; set; }

        /// <summary>
        /// Начало периода для расчета аналитики
        /// </summary>
        public DateTime BeginDate { get; set; }

        /// <summary>
        /// Окончание периода для расчета аналитики
        /// </summary>
        public DateTime EndDate { get; set; }
    }

    ///// <summary>
    ///// Запрос для получения бонусов не за покупки по номеру карты
    ///// </summary>
    //public class BonusesNotForPurchasesRequest
    //{
    //    /// <summary>
    //    /// Номер карты
    //    /// </summary>
    //    public long Card { get; set; }
    //    public Int16 Operator { get; set; }
    //    public Int16 Partner { get; set; }
    //    public Int16 Pos { get; set; }

    //    public long Page { get; set; }
    //    public long PageSize { get; set; }

    //    public string Date { get; set; }
    //    public string Name { get; set; }
    //    public string CardStr { get; set; }
    //    public string AddedMore { get; set; }
    //    public string AddedLess { get; set; }
    //    public string RedeemedMore { get; set; }
    //    public string RedeemedLess { get; set; }
    //    public string BurnMore { get; set; }
    //    public string BurnLess { get; set; }
    //}
}