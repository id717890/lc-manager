namespace LCManager.Infrastructure.Data
{
    public class ChequeItem
    {
        public string Code { get; set; }
        public decimal Price { get; set; }
        public decimal Qty { get; set; }
        public decimal Amount { get; set; }
        public decimal AddedBonus { get; set; }
        public decimal RedeemedBonus { get; set; }
    }
}
