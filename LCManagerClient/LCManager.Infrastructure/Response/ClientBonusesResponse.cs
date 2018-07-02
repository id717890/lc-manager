using System.Collections.Generic;
using LCManager.Infrastructure.Data;

namespace LCManager.Infrastructure.Response
{
    public class ClientBonusesResponse: BaseResponse
    {
        public List<ClientBonus> ClientBonuses { get; set; }
       
        public ClientBonusesResponse()
        {
            ClientBonuses = new List<ClientBonus>();
        }
    }
}
