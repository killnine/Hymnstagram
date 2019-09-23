using System;
using System.Collections.Generic;

namespace Hymnstagram.Web.Models.Api
{
    public class SongbookResult
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Publisher { get; private set; }
        public string ISBN10 { get; private set; }
        public string ISBN13 { get; private set; }
        public IList<CreatorResult> Creators { get; private set; } = new List<CreatorResult>();
        public IList<SongResult> Songs { get; private set; } = new List<SongResult>();
    }
}
