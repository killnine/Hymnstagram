using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hymnstagram.Model.DataAccess.Criteria
{
    public class SongSearchCriteria : SearchCriteriaBase
    {
        public Guid? SongbookId { get; set; }

        public IList<Guid> SongbookIds { get; set; }

        public override bool IsValid
        {
            get { return SongbookId != null || (SongbookIds?.Any() ?? false); }
        }

        public override string BrokenRules 
        {
            get
            {
                return "SongbookId must be populated or there must be at least one element in SongbookIds.";
            }
        }
    }
}
