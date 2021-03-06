﻿using System;

namespace LCManager.Infrastructure.Request
{
    public class CardBonusesRequest
    {
        /// <summary>
        /// идентификатор Оператора программы лояльности
        /// </summary>
        public Int16 Operator { get; set; }
        /// <summary>
        /// номер карты
        /// </summary>
        public Int64 Card { get; set; }
    }
}
