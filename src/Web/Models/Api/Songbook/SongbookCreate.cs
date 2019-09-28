using System;
using System.Collections.Generic;

namespace Hymnstagram.Web.Models.Api
{
    public class SongbookCreate
    {
        public string Title { get; set; }
        public string Publisher { get; set; }
        public DateTime PublicationDate { get; set; }
        public string ISBN10 { get; set; }
        public string ISBN13 { get; set; }
        public IList<CreatorCreate> Creators { get; set; } = new List<CreatorCreate>();
        public IList<SongCreate> Songs { get; set; } = new List<SongCreate>();
    }
}
