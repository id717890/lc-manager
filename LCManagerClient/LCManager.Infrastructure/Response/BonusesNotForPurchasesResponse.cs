namespace Site.Infrastrucure.Response
{
    using System.Collections.Generic;
    using Data;

    /// <inheritdoc />
    /// <summary>
    /// Класс ответа возвращающий бонусы клиента не за покупки
    /// </summary>
    public class BonusesNotForPurchasesResponse : BaseResponse
    {
        /// <summary>
        /// Список бонусов не за покупки
        /// </summary>
        public IEnumerable<Bonus> Bonuses { get; set; }

        public int RecordTotal { get; set; }
        public int RecordFilterd { get; set; }
    }
}
