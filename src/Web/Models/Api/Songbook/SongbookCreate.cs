using System;
using System.Collections.Generic;
using Hymnstagram.Api.Models.Creator;
using Hymnstagram.Api.Models.Song;

namespace Api.Models.Songbook
{
    public class SongbookCreate
    {
        public string Title { get; set; }
        public string Publisher { get; set; }
        public DateTime PublicationDate { get; set; }
        public string ISBN10 { get; set; }
        public string ISBN13 { get; set; }
        public IList<CreatorCreate> Creators { get; set; }
        public IList<SongCreate> Songs { get; set; }        
    }
}
