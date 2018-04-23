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
}