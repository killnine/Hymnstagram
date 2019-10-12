using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hymnstagram.Web.Helpers.Parameters
{
#pragma warning disable CS1591
    public class SongResourceParameters
    {
        private const int MAX_SONG_PAGE_SIZE = 250;
        public int PageNumber { get; set; } = 1;

        private int _pageSize = 20;
        public int PageSize
        {
            get { return _pageSize; }
            set
            {
                _pageSize = (value > MAX_SONG_PAGE_SIZE) ? MAX_SONG_PAGE_SIZE : value;
            }
        }

        [Required]
        public Guid SongbookId { get; set; }
        public IList<Guid> SongbookIds { get; set; } = new List<Guid>();
        public string Tune { get; set; }
        public string Title { get; set; }
        public string Key { get; set; }
        public string TimeSignature { get; set; }
        public string Solfa { get; set; }
        
        public string OrderBy { get; set; } = "SongNumber";
    }
#pragma warning restore CS1591
}
