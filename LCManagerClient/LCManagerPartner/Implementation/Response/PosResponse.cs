namespace LCManagerPartner.Implementation.Response
{
    using System.Collections.Generic;
    using Abstractions;
    using Data;


    public class OperatorPosListResponse : BaseResponse
    {
        /// <summary>
        /// Списки ТТ
        /// </summary>
        public IEnumerable<OperatorPosList> PosLists { get; set; }
    }

    public class OperatorPosResponse : BaseResponse
    {
        /// <summary>
        /// адреса торговых точек
        /// </summary>
        public List<Pos> Poses { get; set; }
        public OperatorPosResponse()
        {
            Poses = new List<Pos>();
        }
    }
}