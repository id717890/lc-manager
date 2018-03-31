using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LCManagerPartner.Implementation.Request
{
    public class GoodRequest
    {
    }

    /// <summary>
    /// Запрос на создание списка товаров
    /// </summary>
    public class OperatorGoodListCreateRequest
    {
        /// <summary>
        /// Идентификатор оператора
        /// </summary>
        public Int16 Operator { get; set; }

        /// <summary>
        /// Наименование создаваемого списка товаров
        /// </summary>
        public string GoodListName { get; set; }

        /// <summary>
        /// Перечень ID ТТ для списка
        /// </summary>
        public int[] GoodList { get; set; }
    }
}