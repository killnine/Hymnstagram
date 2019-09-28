using System;
using System.Collections.Generic;
using System.Linq;

namespace Hymnstagram.Model.DataAccess.Criteria
{
    public class SongbookSearchCriteria : SearchCriteriaBase
    {
        public string Title { get; set; }

        public IList<Guid> Ids { get; set; } = new List<Guid>();

        public override bool IsValid
        {
            get { return !string.IsNullOrEmpty(Title) || Ids.Any(); }
        }

        public override string BrokenRules 
        {
            get
            {
                var result = string.Empty;
                if (IsValid) { return result; }
                
                if (string.IsNullOrEmpty(Title)) { result += "Title must be populated in Songbook search criteria."; }
                if (!Ids.Any()) { result += "Songbook Ids collection should have at least one element."; }

                return result;                
            }
        }
    }
}
