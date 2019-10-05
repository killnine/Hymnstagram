using System;
using System.Collections.Generic;

namespace Hymnstagram.Web.Models.Api
{
    public class SongResult : LinkedResourceBase
    {
        public Guid Id { get; set; }
        public Guid SongbookId { get; set; }
        public int? SongNumber { get; set; }
        public string Tune { get; set; }
        public string Title { get; set; }
        public string Key { get; set; }
        public string TimeSignature { get; set; }
        public string Solfa { get; set; }
        public IList<CreatorResult> Creators { get; set; }
    }
}
