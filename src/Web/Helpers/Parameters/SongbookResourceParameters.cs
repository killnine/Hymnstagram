namespace Hymnstagram.Web.Helpers.Parameters
{
    public class SongbookResourceParameters
    {
        private const int MAX_SONGBOOK_PAGE_SIZE = 50;

        public int PageNumber { get; set; } = 1;

        private int _pageSize = 20;
        public int PageSize
        {
            get { return _pageSize; }
            set
            {
                _pageSize = (value > MAX_SONGBOOK_PAGE_SIZE) ? MAX_SONGBOOK_PAGE_SIZE : value;
            }
        }
                
        public string Title { get; set; }
        public string Publisher { get; set; }
        public string ISBN10 { get; set; }
        public string ISBN13 { get; set; }

        public string OrderBy { get; set; } = "Title";
    }
}
