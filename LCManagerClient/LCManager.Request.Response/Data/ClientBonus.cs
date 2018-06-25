namespace Site.Infrastrucure.Data
{
    using System;

    public class ClientBonus
    {
        public decimal Bonus { get; set; }
        public DateTime BonusTime { get; set; }
        public string BonusOperation { get; set; }
        public string BonusType { get; set; }
    }
}
