using System.Web.Http;
using LCManager.Infrastructure.Request;
using LCManager.Infrastructure.Response;
using LCManagerPartner.Implementation.Services;

namespace LCManagerPartner.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// СВЕРКА или БУХГАЛТЕРИЯ
    /// </summary>
    [Authorize]
    [RoutePrefix("api/bookkeeping")]
    public class BookkeepingController : ApiController
    {
        private readonly BookkeepingService _bookkeepingService;

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        public BookkeepingController()
        {
            _bookkeepingService = new BookkeepingService();
        }


        /// <summary>
        /// Получает сверку по оператору, партнеру или торговой точке
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetBookkeepings")]
        public BookkeepingsResponse GetBookkeepings(BookkeepingRequest request)
        {
            return _bookkeepingService.GetAllBookkeeping(request);
        }
    }
}