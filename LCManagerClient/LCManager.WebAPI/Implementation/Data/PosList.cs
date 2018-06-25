namespace LCManagerPartner.Implementation.Data
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Объект сущности poslist
    /// </summary>
    public class OperatorPosList
    {
        /// <summary>
        /// Идентификатор списка
        /// </summary>
        public Int16 Id { get; set; }
        /// <summary>
        /// Наименование списка
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// Идентификатор опертора, которому принадлежит список
        /// </summary>
        public Int16 Operator { get; set; }


        /// <summary>
        /// Список ТТ списка
        /// </summary>
        public IEnumerable<Pos> Poses { get; set; }

        public OperatorPosList()
        {
            Poses=new List<Pos>();
        }
    }
}