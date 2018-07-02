using LC_Manager.Models;

namespace LC_Manager.Implementation.Data
{
    using System;
    using System.Collections.Generic;

    public class OperatorGoodList
    {
        public Int16 Id { get; set; }
        public string Name { get; set; }
        public Int16 Operator { get; set; }
        public string Text { get; set; }
        public string HtmlForDelete { get; set; }
        public IEnumerable<Good> Goods { get; set; }
        public string details { get; set; }

        public OperatorGoodList()
        {
            Goods = new List<Good>();
        }
    }
}