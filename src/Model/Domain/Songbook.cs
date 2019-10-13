using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Hymnstagram.Model.DataTransfer;
using Hymnstagram.Model.Domain.Validators;

namespace Hymnstagram.Model.Domain
{

    public class Songbook : EntityBase
    {
        private readonly AbstractValidator<Songbook> _validator = new SongbookValidator();

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
            if(dto == null) { return null; }

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

        internal SongbookDto ToDto()
        {
            return new SongbookDto()
            {
                Id = Id,
                Title = Title,
                Publisher = Publisher,
                ISBN10 = ISBN10,
                ISBN13 = ISBN13,
                Creators = Creators.Select(c => c.ToDto()).ToList(),
                Songs = Songs.Select(s => s.ToDto()).ToList()
            };
        }

        public bool IsValid => _validator.Validate(this).IsValid;
        public ValidationResult Validate()
        {
            return _validator.Validate(this);
        }
    }
}
