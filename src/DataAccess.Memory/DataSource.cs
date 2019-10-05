using System;
using System.Collections.Generic;
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
                Id = new Guid("21924d66-dac6-43a5-beee-206de4d35216"),
                Title = "Psalms, Hymns, and Spiritual Songs",
                ISBN10 = "1584273526",
                ISBN13 = "9781584273523",
                Publisher = "Sumphonia Productions LLD (2012)"
            };

            var defaultSongbookCreators = new List<CreatorDto>()
            {
                new CreatorDto() { Id = new Guid("fdfba83c-08aa-42a4-b1c1-46b8e154f297"), ParentId = defaultSongbook.Id, FirstName = "David", LastName = "Maravilla", ParentTypeId = (int)CreativeType.Editor, TypeId = (int)CreatorParentType.Songbook },
                new CreatorDto() { Id = new Guid("e2495b99-9fd0-4be6-b2db-893ce69a6f38"), ParentId = defaultSongbook.Id, FirstName = "Matt", LastName = "Bassford", ParentTypeId = (int)CreativeType.TechnicalEditor, TypeId = (int)CreatorParentType.Songbook }
            };

            var defaultSong = new SongDto()
            {
                Id = new Guid("6767f7f5-0dc9-4b78-a0f0-e450e4b60889"),
                SongbookId = defaultSongbook.Id,
                Title = "Nearer, My God, to Thee",
                SongNumber = 328,
                Key = "G",
                SolfaTypeId = (int)SolfaType.Mi,
                TimeSignature = "4/4",
                Tune = "Bethany"
            };

            var defaultSongCreator = new CreatorDto() { Id = new Guid("d3788116-90f7-40d4-9a82-a3cef6e769df"), ParentId = defaultSong.Id, FirstName = "Sarah Flower", LastName = "Adams", ParentTypeId = (int)CreativeType.Writer, TypeId = (int)CreatorParentType.Song };

            Songbooks.Add(defaultSongbook);
            Songs.Add(defaultSong);
            Creators.AddRange(defaultSongbookCreators);
            Creators.Add(defaultSongCreator);
        }
    }
}
