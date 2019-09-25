using Hymnstagram.Model.Domain;
using System;

namespace Hymnstagram.Model.DataAccess.Criteria
{
    public class CreatorSearchCriteria : SearchCriteriaBase
    {
        public CreatorParentType? ParentType { get; set; }
        public Guid? ParentId { get; set; }        

        public override bool IsValid
        {
            get
            {
                return (ParentType != null && ParentId != null);
            }
        }

        public override string BrokenRules 
        {
            get
            {
                if (IsValid) { return string.Empty; }
                if (ParentType == null || ParentId == null)
                {
                   return "Both ParentType and ParentId must be defined in the Creator search criteria";
                }
                return string.Empty;
            }    
        }
    }
}
