using FluentValidation;
using FluentValidation.Results;
using Hymnstagram.Model.DataTransfer;
using Hymnstagram.Model.Domain.Validators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hymnstagram.Model.Domain
{
    public class Song : EntityBase
    {
        private readonly AbstractValidator<Song> _validator = new SongValidator();

        public Guid SongbookId { get; set; }
        public int? SongNumber { get; private set; }
        public string Tune { get; private set; }
        public string Title { get; private set; }
        public string Key { get; private set; }
        public string TimeSignature { get; private set; }
        public SolfaType? Solfa { get; private set; }
        public IList<Creator> Creators { get; private set; } = new List<Creator>();
        
        private Song() { }

        public static Song Create(Guid songbookId)
        {
            return new Song()
            {
                SongbookId = songbookId,
                SongNumber = null,
                Tune = string.Empty,
                Title = string.Empty,
                Key = string.Empty,
                TimeSignature = string.Empty,
                Solfa = null,
                Creators = new List<Creator>()
            };
        }

        public static Song From(SongDto dto)
        {
            return new Song()
            {
                Id = dto.Id,
                SongbookId = dto.SongbookId,
                SongNumber = dto.SongNumber,
                Tune = dto.Tune,
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
                SongbookId = SongbookId,
                SongNumber = SongNumber,
                Tune = Tune,
                Title = Title,
                Key = Key,
                TimeSignature = TimeSignature,
                SolfaTypeId = (int?)Solfa,
                Creators = Creators.Select(c => c.ToDto()).ToList()
            };
        }

        public bool IsValid => _validator.Validate(this).IsValid;
        public ValidationResult Validate()
        {
            return _validator.Validate(this);
        }
    }
}
