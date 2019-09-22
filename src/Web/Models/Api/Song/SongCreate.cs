using System;
using System.Collections.Generic;
using Hymnstagram.Api.Models.Creator;

namespace Hymnstagram.Api.Models.Song
{
    public class SongCreate
    {
        public int? SongNumber { get; set; }
        public string Tune { get; set; }
        public string Title { get; set; }
        public string Key { get; set; }
        public string TimeSignature { get; set; }
        public int? SolfaTypeId { get; set; }
        public IList<CreatorCreate> Creators { get; set; }
    }
}
