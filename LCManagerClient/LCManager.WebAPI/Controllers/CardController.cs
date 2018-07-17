using LCManager.Infrastructure.Request;
using LCManager.Infrastructure.Response;
using LCManagerPartner.Implementation.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LCManagerPartner.Controllers
{
    /// <summary>
    /// Контроллер для работы с картыми ПЛ
    /// </summary>
    [Authorize]
    [RoutePrefix("api/card")]
    public class CardController : ApiController
    {
        private readonly CardService _cardService;

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        public CardController()
        {
            _cardService = new CardService();
        }

        /// <summary>
        /// Получает базовую инфу по карте ПЛ (статус, тип, оператор, баланс, кол-во покупок, сумма покупок, кол-во начисленных бонусов, кол-во списанных бонусов
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetCard")]
        public CardInfoResponse GetCard(CardStatisticsRequest request)
        {
            return _cardService.GetCard(request);
        }
    }
}
