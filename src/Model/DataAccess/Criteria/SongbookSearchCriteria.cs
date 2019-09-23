namespace Hymnstagram.Model.DataAccess.Criteria
{
    public class SongbookSearchCriteria : SearchCriteriaBase
    {
        public string Title { get; set; }

        public override bool IsValid
        {
            get { return string.IsNullOrEmpty(Title); }
        }

        public override string BrokenRules 
        {
            get
            {
                if(string.IsNullOrEmpty(Title)) { return "Title must be populated in Songbook search criteria"; }
                return string.Empty;
            }
        }
    }
}
