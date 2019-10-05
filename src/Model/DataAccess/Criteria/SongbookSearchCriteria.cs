using System;
using System.Collections.Generic;
using System.Linq;

namespace Hymnstagram.Model.DataAccess.Criteria
{
    public class SongbookSearchCriteria
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;

        public string Title { get; set; }
        public string Publisher { get; set; }
        public string ISBN10 { get; set; }
        public string ISBN13 { get; set; }
        public IList<Guid> Ids { get; set; } = new List<Guid>();


        public string OrderBy { get; set; } = "Title";        
    }
}
