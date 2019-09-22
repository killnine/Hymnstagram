namespace Hymnstagram.Model.DataAccess.Criteria
{
    public class SongbookSearchCriteria
    {
        public string Title { get; set; }

        public bool IsValid
        {
            get { return string.IsNullOrEmpty(Title); }
        }
    }
}
