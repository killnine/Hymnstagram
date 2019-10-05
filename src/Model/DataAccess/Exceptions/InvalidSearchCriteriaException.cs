using Hymnstagram.Model.DataAccess.Criteria;
using System;

namespace Hymnstagram.Model.DataAccess
{
    public class InvalidSearchCriteriaException : Exception
    {
        private SearchCriteriaBase _criteria;

        public InvalidSearchCriteriaException(SearchCriteriaBase criteria) { _criteria = criteria; }

        public override string Message => _criteria.BrokenRules;
    }
}
