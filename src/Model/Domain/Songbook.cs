using System;
using System.Collections.Generic;
using System.Linq;

namespace Model
{

    public class Songbook
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Publisher { get; private set; }
        public string ISBN10 { get; private set; }
        public string ISBN13 { get; private set; }
        public IList<Creator> Creators { get; private set; } = new List<Creator>();
        public IList<Song> Songs { get; private set; } = new List<Song>();

        private Songbook() { }        

        public static Songbook Create()
        {
            return new Songbook()
            {
                Id = Guid.NewGuid(),
                Title = string.Empty,
                Publisher = string.Empty,
                ISBN10 = string.Empty,
                ISBN13 = string.Empty,
                Creators = new List<Creator>(),
                Songs = new List<Song>()
            };
        }

        public static Songbook From(SongbookDto dto)
        {
            return new Songbook()
            {
                Id = dto.Id,
                Title = dto.Title,
                Publisher = dto.Publisher,
                ISBN10 = dto.ISBN10,
                ISBN13 = dto.ISBN13,
                Creators = dto.Creators?.Select(Creator.From).ToList() ?? new List<Creator>(),
                Songs = dto.Songs?.Select(Song.From).ToList() ?? new List<Song>()
            };
        }
    }
}
