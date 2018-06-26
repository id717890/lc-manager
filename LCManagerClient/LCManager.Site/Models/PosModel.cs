namespace LC_Manager.Models
{
    using Implementation.Data;
    using System.Collections.Generic;

    public class PosModel
    {
        public class PosData
        {
            public List<Pos> data { get; set; }
            public PosData()
            {
                data = new List<Pos>();
            }
        }

        public class PosListData
        {
            public List<Implementation.Data.OperatorPosList> data { get; set; }

            public PosListData()
            {
                data = new List<Implementation.Data.OperatorPosList>();
            }
        }


        //public class PosViewModel
        //{
        //    //public IEnumerable<Pos> Poses { get; set; }
        //    //public IEnumerable<OperatorPosList> OperatorPosLists { get; set; }

        //    public PosViewModel()
        //    {
        //        //Poses=new List<Pos>();
        //        //OperatorPosLists=new List<OperatorPosList>();
        //    }
        //}
    }
}