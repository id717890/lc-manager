namespace LCManagerPartner.Implementation.Data
{
    using Models;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Объект сущности goodlist
    /// </summary>
    public class OperatorGoodList
    {
        /// <summary>
        /// Идентификатор списка
        /// </summary>
        public Int16 Id { get; set; }
        /// <summary>
        /// Наименование списка
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Идентификатор опертора, которому принадлежит список
        /// </summary>
        public Int16 Operator { get; set; }

        /// <summary>
        /// Список ТТ списка
        /// </summary>
        public IEnumerable<Good> Goods { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public OperatorGoodList()
        {
            Goods = new List<Good>();
        }
    }
}