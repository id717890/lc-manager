namespace LCManager.Infrastructure.Data
{
    public class Bookkeeping
    {
        public long Id { get; set; }
        public string Caption { get; set; }
        public long Purchases { get; set; }
        public decimal Added { get; set; }
        public decimal Redeemed { get; set; }
        public long Clients { get; set; }
    }
}
