using System;

namespace Model
{
    public class CreatorDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int CreativeTypeId { get; set; }
    }
}
