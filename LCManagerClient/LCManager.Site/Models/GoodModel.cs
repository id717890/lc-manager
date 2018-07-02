namespace LC_Manager.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class GoodModel
    {
        public class GoodListData
        {
            public List<Implementation.Data.OperatorGoodList> data { get; set; }

            public GoodListData()
            {
                data = new List<Implementation.Data.OperatorGoodList>();
            }
        }
    }

    

}