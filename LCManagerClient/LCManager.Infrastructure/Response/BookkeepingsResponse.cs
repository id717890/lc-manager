using System.Collections.Generic;
using LCManager.Infrastructure.Data;

namespace LCManager.Infrastructure.Response
{
    public class BookkeepingsResponse: BaseResponse
    {
        public IEnumerable<Bookkeeping> Bookkeepings { get; set; }
        public int RecordTotal { get; set; }
        public int RecordFilterd { get; set; }
    }
}
