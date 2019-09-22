using System;
using System.Collections.Generic;
using Model;

namespace DataAccess.Memory
{
    public static class DataSource
    {
        public static IList<Songbook> Songbooks = new List<Songbook>()
        {
            Songbook.From(new SongbookDto()
            {
                Id = Guid.NewGuid(),
                Title = "Psalms, Hymns, and Spiritual Songs",
                ISBN10 = "1584273526",
                ISBN13 = "9781584273523",
                Publisher = "Sumphonia Productions LLD (2012)",
                Creators = new List<CreatorDto>()
                {
                    new CreatorDto() { Id = Guid.NewGuid(), FirstName = "David", LastName = "Maravilla", CreativeTypeId = (int)CreativeType.Editor },
                    new CreatorDto() { Id = Guid.NewGuid(), FirstName = "Matt", LastName = "Bassford", CreativeTypeId = (int)CreativeType.TechnicalEditor }
                },
                Songs = new List<SongDto>()
                {
                    new SongDto()
                    {
                        Id = Guid.NewGuid(),
                        Title = "Nearer, My God, to Thee",
                        SongNumber = 328,
                        Key = "G",
                        SolfaTypeId = (int)SolfaType.Mi,
                        TimeSignature = "4/4",
                        Tune = "Bethany",
                        Creators = new List<CreatorDto>()
                        {
                            new CreatorDto()
                            {
                                Id = Guid.NewGuid(),
                                FirstName = "Sarah Flower",
                                LastName = "Adams",
                                CreativeTypeId = (int)CreativeType.Writer
                            }
                        }
                    }
                }
            })
        };
    }
}
