using System.Collections.Generic;

namespace LC_Manager.Implementation.RequestData
{
    using System;

    public class OperatorPosRequest
    {
        public Int16 Operator { get; set; }
    }

    /// <summary>
    /// Запрос на удаление списка ТТ оператора
    /// </summary>
    public class OperatorPosRemoveRequest
    {
        /// <summary>
        /// идентификатор удаляемого списка
        /// </summary>
        public Int16 OperatorPosList { get; set; }
    }

    /// <summary>
    /// Запрос на создание списка
    /// </summary>
    public class OperatorPosListCreateRequest
    {
        /// <summary>
        /// Идентификатор оператора
        /// </summary>
        public Int16 Operator { get; set; }

        /// <summary>
        /// Наименование создаваемого списка
        /// </summary>
        public string PosListName { get; set; }

        /// <summary>
        /// Перечень ID ТТ для списка
        /// </summary>
        public List<int> PosList { get; set; }
    }
}