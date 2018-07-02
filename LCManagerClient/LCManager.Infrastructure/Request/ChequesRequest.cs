using System;

namespace LCManager.Infrastructure.Request
{
    public class ChequesRequest
    {
        public Int16 Operator { get; set; }
        public int PartnerId { get; set; }
        public Int16 Pos { get; set; }
        public long Client { get; set; }
        public long Card { get; set; }
        public long Page { get; set; }
        public long PageSize { get; set; }

        public string DateBuy { get; set; }
        public string PosStr { get; set; }
        public string Phone { get; set; }
        public string Operation { get; set; }
        public string Number { get; set; }
        public string Sum { get; set; }
        public string Added { get; set; }
        public string Redeemed { get; set; }
        public string DateStart { get; set; }
        public string DateEnd { get; set; }
    }
}
