namespace Site.Infrastrucure.Response
{
    using System.Collections.Generic;
    using Data;
    public class ClientBonusesResponse: BaseResponse
    {
        public List<ClientBonus> ClientBonuses { get; set; }
       
        public ClientBonusesResponse()
        {
            ClientBonuses = new List<ClientBonus>();
        }
    }
}
