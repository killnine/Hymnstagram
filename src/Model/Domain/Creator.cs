using FluentValidation;
using FluentValidation.Results;
using Hymnstagram.Model.DataTransfer;
using Hymnstagram.Model.Domain.Validators;
using System;

namespace Hymnstagram.Model.Domain
{
    public class Creator : EntityBase
    {
        private readonly AbstractValidator<Creator> _validator = new CreatorValidator();

        public Guid ParentId { get; set; }
        public CreatorParentType ParentType {get; set;}
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public CreativeType Type { get; private set; }

        private Creator() { }

        public static Creator Create(CreativeType type, CreatorParentType parentType, Guid parentId)
        {
            return new Creator()
            {
                FirstName = string.Empty,
                LastName = string.Empty,
                Type = type,
                ParentId = parentId,
                ParentType = parentType
            };
        }

        public static Creator From(CreatorDto dto)
        {
            if(dto == null) { return null; }

            return new Creator()
            {
                Id = dto.Id,
                ParentId = dto.ParentId,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                ParentType = (CreatorParentType)dto.ParentTypeId,
                Type = (CreativeType)dto.TypeId
            };
        }

        public CreatorDto ToDto()
        {
            return new CreatorDto()
            {
                Id = Id,
                ParentId = ParentId,
                FirstName = FirstName,
                LastName = LastName,
                ParentTypeId = (int)Type,                
                TypeId = (int)ParentType                
            };
        }

        public bool IsValid => _validator.Validate(this).IsValid;
        public ValidationResult Validate()
        {
            return _validator.Validate(this);
        }
    }
}
