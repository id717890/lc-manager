
using LCManager.Infrastructure.Data;

namespace LCManager.Infrastructure.Response
{
    public class ClientInfoResponse: BaseResponse
    {
        //public string Name { get; set; }
        //public string Surname { get; set; }
        //public string Patronymic { get; set; }
        //public decimal CardFullBalance { get; set; }
        //public string CardStatus { get; set; }
        //public string CardNumber { get; set; }
        //public decimal CashBack { get; set; }

        /// <summary>
        /// Информация об участнике программы лояльности
        /// </summary>
        public Client ClientData { get; set; }
        public ClientInfoResponse()
        {
            ClientData = new Client();
        }
    }
}
