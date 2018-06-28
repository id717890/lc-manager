using System.Collections.Generic;

namespace LC_Manager.Models
{
    public class BonusesViewModel
    {
        public int id { get; set; }
        public string date { get; set; }
        public string type { get; set; }
        public string added { get; set; }
        public string redeemed { get; set; }
        public string fireed { get; set; }
        public string card { get; set; }
        public string lorem { get; set; }
    }

    public class Bonuses
    {
        public List<BonusesViewModel> data { get; set; }
        public int recordsTotal { get; set; }
        public int draw { get; set; }
        public int recordsFiltered { get; set; }

        public Bonuses()
        {
            data = new List<BonusesViewModel>();
        }
    }


    
}