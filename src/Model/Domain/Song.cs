using System;
using System.Collections.Generic;
using System.Linq;

namespace Model
{
    public class Song
    {
        public Guid Id { get; set; }
        public int? SongNumber { get; private set; }
        public string Tune { get; private set; }
        public string Title { get; private set; }
        public string Key { get; private set; }
        public string TimeSignature { get; private set; }
        public SolfaType? Solfa { get; private set; }
        public IList<Creator> Creators { get; private set; } = new List<Creator>();
        
        private Song() { }

        public static Song Create()
        {
            return new Song()
            {
                Id = Guid.NewGuid(),
                Tune = string.Empty,
                Title = string.Empty,
                Key = string.Empty,
                TimeSignature = string.Empty,
                Creators = new List<Creator>()
            };
        }

        public static Song From(SongDto dto)
        {
            return new Song()
            {
                Id = dto.Id,
                SongNumber = dto.SongNumber,
                Title = dto.Title,
                Key = dto.Key,
                TimeSignature = dto.TimeSignature,
                Solfa = (SolfaType)dto.SolfaTypeId,
                Creators = dto.Creators?.Select(Creator.From).ToList() ?? new List<Creator>()
            };
        }

        public SongDto ToDto()
        {
            return new SongDto()
            {
                Id = Id,
                SongNumber = SongNumber,
                Tune = Tune,
                Title = Title,
                Key = Key,
                TimeSignature = TimeSignature,
                SolfaTypeId = (int?)Solfa,
                Creators = Creators.Select(c => c.ToDto()).ToList()
            };
        }
    }
}
