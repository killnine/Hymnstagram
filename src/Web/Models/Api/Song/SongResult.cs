using System;

namespace Hymnstagram.Web.Models.Api
{
    public class SongResult
    {
        public Guid Id { get; set; }
        public int? SongNumber { get; private set; }
        public string Tune { get; private set; }
        public string Title { get; private set; }
        public string Key { get; private set; }
        public string TimeSignature { get; private set; }
        public string Solfa { get; private set; }
    }
}
