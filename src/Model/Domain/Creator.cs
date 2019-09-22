using System;

namespace Model
{
    public class Creator
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public CreativeType Type { get; private set; }

        private Creator(Guid id, string firstName, string lastName, CreativeType type)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Type = type;
        }

        public static Creator From(CreatorDto dto)
        {
            if(dto == null) { return null; }

            return new Creator(dto.Id, dto.FirstName, dto.LastName, (CreativeType)dto.CreativeTypeId);
        }

        public CreatorDto ToDto()
        {
            return new CreatorDto()
            {
                Id = Id,
                FirstName = FirstName,
                LastName = LastName,
                CreativeTypeId = (int)Type
            };
        }
    }
}
