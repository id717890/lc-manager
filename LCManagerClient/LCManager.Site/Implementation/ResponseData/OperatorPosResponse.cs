namespace LC_Manager.Implementation.ResponseData
{
    using Abstractions;
    using System.Collections.Generic;
    using Data;

    public class OperatorPosResponse : BaseResponse
    {
        /// <summary>
        /// адреса торговых точек
        /// </summary>
        public List<Pos> Poses { get; set; }
        public List<OperatorPosList> PosLists { get; set; }

        public OperatorPosResponse()
        {
            Poses = new List<Pos>();
            PosLists = new List<OperatorPosList>();
        }
    }

    public class OperatorPosListResponse : BaseResponse
    {
        public IEnumerable<Pos> Poses { get; set; }
        public IEnumerable<OperatorPosList> PosLists { get; set; }
    }

}