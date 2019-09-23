using System;
using System.Collections.Generic;
using System.Linq;
using Hymnstagram.Model.DataTransfer;
using Hymnstagram.Model.Domain;

namespace DataAccess.Memory
{
    public static class DataSource
    {
        private static Seed _seedData = new Seed();

        public static IList<CreatorDto> Creators { get { return _seedData.Creators; } }
        public static IList<SongDto> Songs { get { return _seedData.Songs; } }
        public static IList<SongbookDto> Songbooks { get { return _seedData.Songbooks; } }
    }

    public class Seed
    {
        public List<CreatorDto> Creators { get; set; } = new List<CreatorDto>();
        public List<SongDto> Songs { get; set; } = new List<SongDto>();
        public List<SongbookDto> Songbooks { get; set; } = new List<SongbookDto>();

        public Seed()
        {
            var defaultSongbook = new SongbookDto()
            {
                Id = Guid.NewGuid(),
                Title = "Psalms, Hymns, and Spiritual Songs",
                ISBN10 = "1584273526",
                ISBN13 = "9781584273523",
                Publisher = "Sumphonia Productions LLD (2012)"
            };

            var defaultSongbookCreators = new List<CreatorDto>()
            {
                new CreatorDto() { Id = Guid.NewGuid(), ParentId = defaultSongbook.Id, FirstName = "David", LastName = "Maravilla", ParentTypeId = (int)CreativeType.Editor, TypeId = (int)CreatorParentType.Songbook },
                new CreatorDto() { Id = Guid.NewGuid(), ParentId = defaultSongbook.Id, FirstName = "Matt", LastName = "Bassford", ParentTypeId = (int)CreativeType.TechnicalEditor, TypeId = (int)CreatorParentType.Songbook }
            };

            var defaultSong = new SongDto()
            {
                Id = Guid.NewGuid(),
                SongbookId = defaultSongbook.Id,
                Title = "Nearer, My God, to Thee",
                SongNumber = 328,
                Key = "G",
                SolfaTypeId = (int)SolfaType.Mi,
                TimeSignature = "4/4",
                Tune = "Bethany"
            };

            var defaultSongCreator = new CreatorDto() { Id = Guid.NewGuid(), ParentId = defaultSong.Id, FirstName = "Sarah Flower", LastName = "Adams", ParentTypeId = (int)CreativeType.Writer, TypeId = (int)CreatorParentType.Song };

            Songbooks.Add(defaultSongbook);
            Songs.Add(defaultSong);
            Creators.AddRange(defaultSongbookCreators);
            Creators.Add(defaultSongCreator);
        }
    }
}
