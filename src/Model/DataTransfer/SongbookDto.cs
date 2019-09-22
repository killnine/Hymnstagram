using System;
using System.Collections.Generic;

namespace Model
{
    public class SongbookDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Publisher { get; set; }
        public string ISBN10 { get; set; }
        public string ISBN13 { get; set; }
        public IList<CreatorDto> Creators { get; set; }
        public IList<SongDto> Songs { get; set; }
    }
}
