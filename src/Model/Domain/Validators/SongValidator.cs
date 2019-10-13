using FluentValidation;

namespace Hymnstagram.Model.Domain.Validators
{
    public class SongValidator : AbstractValidator<Song>
    {
        public SongValidator()
        {
            RuleFor(s => s.SongNumber).NotNull().GreaterThan(0);
            RuleFor(s => s.Tune).MaximumLength(60);
            RuleFor(s => s.Title).NotEmpty().MaximumLength(100);
            RuleFor(s => s.Key).MaximumLength(5);
            RuleFor(s => s.TimeSignature).MaximumLength(5);
            RuleFor(s => s.Solfa).NotNull();
            RuleForEach(s => s.Creators).SetValidator(new CreatorValidator())
                                        .Must(t => t.Type == CreativeType.Arranger || t.Type == CreativeType.Composer || t.Type == CreativeType.Writer).WithMessage("Only arrangers, composers, and writers can be creative types for a song."); ;
        }
    }
}
