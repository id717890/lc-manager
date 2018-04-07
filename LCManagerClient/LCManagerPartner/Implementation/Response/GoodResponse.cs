using LCManagerPartner.Implementation.Abstractions;
using LCManagerPartner.Implementation.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LCManagerPartner.Implementation.Response
{
    /// <summary>
    /// 
    /// </summary>
    public class GoodResponse
    {
    }

    /// <summary>
    /// 
    /// </summary>
    public class OperatorGoodListResponse : BaseResponse
    {
        /// <summary>
        /// Списки ТТ
        /// </summary>
        public IEnumerable<OperatorGoodList> GoodLists { get; set; }
    }

    /// <inheritdoc />
    /// <summary>
    /// Ответ на импорт товаром из файла
    /// </summary>
    public class OperatorGoodImportResponse : BaseResponse
    {
        /// <summary>
        /// Кол-во импортированных строк
        /// </summary>
        public int ImportedRows { get; set; }
    }
}