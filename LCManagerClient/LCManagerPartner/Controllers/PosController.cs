namespace LCManagerPartner.Controllers
{
    using System.Web.Http;
    using Implementation.Request;
    using Implementation.Response;
    using Implementation.Services;

    [RoutePrefix("api/pos")]
    public class PosController : ApiController
    {
        private readonly PosService _operatorPosService;

        public PosController()
        {
            _operatorPosService=new PosService();
        }

        /// <summary>
        /// Выборка адресов торговых точек Оператора
        /// </summary>
        [HttpPost]
        [Route("OperatorPos")]
        public OperatorPosResponse OperatorPos(OperatorPosRequest request)
        {
            return _operatorPosService.GetPosByOperator(request);
        }

        /// <summary>
        /// Сохранение списка магазинов
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveOperatorPosList")]
        public OperatorPosListResponse OperatorPos(OperatorPosListCreateRequest request)
        {
            return _operatorPosService.SaveOperatorPosList(request);
        }

        /// <summary>
        /// Получает списка магазинов, отдельного оператора
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("OperatorPosList")]
        public OperatorPosListResponse GetPosListByOperator(OperatorPosListCreateRequest request)
        {
            return _operatorPosService.GetPosListByOperator(request);
        }
    }
}
