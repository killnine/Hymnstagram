using System.Collections.Generic;

namespace Hymnstagram.Web.Models.Api.Songbook
{
#pragma warning disable CS1591
    public class SongbookCollectionResult : LinkedResourceBase
    {
        public IEnumerable<SongbookResult> Results { get; set; } = new List<SongbookResult>();
    }
#pragma warning restore CS1591
}
