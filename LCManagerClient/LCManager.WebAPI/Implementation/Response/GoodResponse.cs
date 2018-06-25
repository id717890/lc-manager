namespace LCManagerPartner.Implementation.Response
{
    using Abstractions;
    using Data;
    using System.Collections.Generic;
    using Models;

    /// <summary>
    /// 
    /// </summary>
    public class OperatorGoodsResponse
    {
        /// <summary>
        /// код ошибки
        /// </summary>
        public int ErrorCode { get; set; }
        /// <summary>
        /// сообщение об ошибке
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// список товаров?
        /// </summary>
        public List<Good> OperatorGoods { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public OperatorGoodsResponse()
        {
            OperatorGoods = new List<Good>();
        }
    }

    /// <inheritdoc />
    /// <summary>
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