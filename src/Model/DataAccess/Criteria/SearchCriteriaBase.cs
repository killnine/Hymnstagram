namespace Hymnstagram.Model.DataAccess.Criteria
{
    public abstract class SearchCriteriaBase
    {
        public abstract bool IsValid { get; }

        public abstract string BrokenRules { get; }
    }
}
