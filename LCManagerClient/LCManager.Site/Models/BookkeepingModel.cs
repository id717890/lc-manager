using System.Collections.Generic;

namespace LC_Manager.Models
{
    public class BookkeepingViewModel
    {
        public long id { get; set; }
        public string caption { get; set; }
        public string posname { get; set; }
        public string purchases { get; set; }
        public string added { get; set; }
        public string redeemed { get; set; }
        public string clients { get; set; }
        public string diagrams { get; set; }
    }

    public class BookkeepingDataTableModel
    {
        public List<BookkeepingViewModel> data { get; set; }
        public int recordsTotal { get; set; }
        public int draw { get; set; }
        public int recordsFiltered { get; set; }

        public BookkeepingDataTableModel()
        {
            data = new List<BookkeepingViewModel>();
        }
    }
}