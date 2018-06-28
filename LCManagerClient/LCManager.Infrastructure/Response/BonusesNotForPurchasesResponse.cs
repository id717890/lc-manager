using System.Collections.Generic;
using LCManager.Infrastructure.Data;

namespace LCManager.Infrastructure.Response
{
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
