﻿namespace Site.Infrastrucure.Response
{
    using System.Collections.Generic;
    using Data;

    public class ChequesResponse
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
        public int PageCount { get; set; }
        public List<Cheque> ChequeData { get; set; }
        public int RecordTotal { get; set; }
        public int RecordFilterd { get; set; }
        public ChequesResponse()
        {
            ChequeData = new List<Cheque>();
        }
    }
}
