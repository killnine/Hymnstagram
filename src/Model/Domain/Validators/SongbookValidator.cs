using FluentValidation;

namespace Hymnstagram.Model.Domain.Validators
{
    public class SongbookValidator : AbstractValidator<Songbook>
    {
        public SongbookValidator()
        {
            RuleFor(sb => sb.Title).NotEmpty().MaximumLength(100);
            RuleFor(sb => sb.Publisher).MaximumLength(60);
            RuleFor(sb => sb.ISBN10).Length(10);
            RuleFor(sb => sb.ISBN13).Length(13);
            RuleForEach(sb => sb.Creators).SetValidator(new CreatorValidator())
                                          .Must(t => t.Type == CreativeType.Editor || t.Type == CreativeType.AssociateEditor || t.Type == CreativeType.TechnicalEditor).WithMessage("Only Editors can be creative types for a songbook.");
            RuleForEach(sb => sb.Songs).SetValidator(new SongValidator());
        }
    }
}
