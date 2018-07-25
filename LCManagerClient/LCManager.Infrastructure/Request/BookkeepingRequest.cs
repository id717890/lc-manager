using System;

namespace LCManager.Infrastructure.Request
{
    public class BookkeepingRequest
    {
        public Int16 Operator { get; set; }
        public Int16 Partner { get; set; }
        public Int16 Pos { get; set; }

        public long Page { get; set; }
        public long PageSize { get; set; }

        public string DateStart { get; set; }
        public string DateEnd { get; set; }
        public string Name { get; set; }
        public string PosName { get; set; }
        public string PurchasesMore { get; set; }
        public string PurchasesLess { get; set; }
        public string AddedMore { get; set; }
        public string AddedLess { get; set; }
        public string RedeemedMore { get; set; }
        public string RedeemedLess { get; set; }
        public string ClientsMore { get; set; }
        public string ClientsLess { get; set; }
    }
}
