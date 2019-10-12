using System;
using System.Collections.Generic;

namespace Hymnstagram.Web.Models.Api
{
#pragma warning disable CS1591
    public class SongbookResult : LinkedResourceBase
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Publisher { get; set; }
        public string ISBN10 { get; set; }
        public string ISBN13 { get; set; }
        public IList<CreatorResult> Creators { get; set; } = new List<CreatorResult>();
        public IList<SongResult> Songs { get; set; } = new List<SongResult>();
    }
#pragma warning restore CS1591
}
