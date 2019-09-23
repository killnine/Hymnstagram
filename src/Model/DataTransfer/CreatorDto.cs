using System;

namespace Hymnstagram.Model.DataTransfer
{
    public class CreatorDto
    {
        public Guid Id { get; set; }
        public Guid ParentId { get; set; }
        public int TypeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int ParentTypeId { get; set; }
        
    }
}
