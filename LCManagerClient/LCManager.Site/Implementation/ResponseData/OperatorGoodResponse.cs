namespace LC_Manager.Implementation.ResponseData
{
    using Abstractions;
    using Data;
    using System.Collections.Generic;

    public class OperatorGoodsResponse
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
        public List<Good> OperatorGoods { get; set; }
        public OperatorGoodsResponse()
        {
            OperatorGoods = new List<Good>();
        }
    }

    public class OperatorGoodListResponse : BaseResponse
    {
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