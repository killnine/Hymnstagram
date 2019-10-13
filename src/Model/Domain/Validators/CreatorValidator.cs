using FluentValidation;

namespace Hymnstagram.Model.Domain.Validators
{
    public class CreatorValidator : AbstractValidator<Creator>
    {
        public CreatorValidator()
        {
            RuleFor(c => c.FirstName).NotEmpty().MaximumLength(15);
            RuleFor(c => c.LastName).NotEmpty().MaximumLength(25);            
        }
    }
}
