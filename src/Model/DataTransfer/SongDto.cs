using System;
using System.Collections.Generic;

namespace Hymnstagram.Model.DataTransfer
{
    public class SongDto
    {
        public Guid Id { get; set; }
        public Guid SongbookId { get; set; }
        public int? SongNumber { get; set; }
        public string Tune { get; set; }
        public string Title { get; set; }
        public string Key { get; set; }
        public string TimeSignature { get; set; }
        public int? SolfaTypeId { get; set; }
        public IList<CreatorDto> Creators { get; set; }        
    }
}
