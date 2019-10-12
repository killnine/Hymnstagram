using System.Collections.Generic;

namespace Hymnstagram.Web.Models.Api.Song
{
#pragma warning disable CS1591
    public class SongCollectionResult : LinkedResourceBase
    {
        public IEnumerable<SongResult> Results { get; set; } = new List<SongResult>();
    }
#pragma warning restore CS1591
}
