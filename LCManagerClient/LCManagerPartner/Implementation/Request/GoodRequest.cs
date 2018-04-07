﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using LCManagerPartner.Models;

namespace LCManagerPartner.Implementation.Request
{
    /// <summary>
    /// </summary>
    public class OperatorGoodRequest
    {
        /// <summary>
        /// Идентификатор оператора
        /// </summary>
        public Int16 Operator { get; set; }
    }

    /// <summary>
    /// Запрос на удаление списка ТТ оператора
    /// </summary>
    public class OperatorGoodRemoveRequest
    {
        /// <summary>
        /// идентификатор удаляемого списка товаров
        /// </summary>
        public Int16 OperatorGoodList { get; set; }
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

    /// <summary>
    /// Запрос на импорт товаров из файла
    /// </summary>
    public class OperatorGoodImportRequest
    {
        /// <summary>
        /// Идентификатор партнера
        /// </summary>
        public Int16 Partner { get; set; }

        /// <summary>
        /// Список товаров
        /// </summary>
        public List<Good> Goods { get; set; }
        
    }
}