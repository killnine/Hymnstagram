using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hymnstagram.Model.DataAccess.Criteria
{
    public class SongSearchCriteria : SearchCriteriaBase
    {
        public Guid? SongbookId { get; set; }

        public IList<Guid> SongbookIds { get; set; } = new List<Guid>();

        public override bool IsValid
        {
            get { return SongbookId != null || (SongbookIds?.Any() ?? false); }
        }

        public override string BrokenRules 
        {
            get
            {
                if(IsValid) { return string.Empty; }
                return "SongbookId must be populated or there must be at least one element in SongbookIds.";
            }
        }
    }
}
