using System.Collections.Generic;
using LCManager.Infrastructure.Data;

namespace LCManager.Infrastructure.Response
{
    public class CardBonusesResponse
    {
        public List<CardBonus> CardBonuses { get; set; }
        /// <summary>
        /// код ошибки
        /// </summary>
        public int ErrorCode { get; set; }
        /// <summary>
        /// сообщение об ошибке
        /// </summary>
        public string Message { get; set; }
        public CardBonusesResponse()
        {
            CardBonuses = new List<CardBonus>();
        }
    }
}
