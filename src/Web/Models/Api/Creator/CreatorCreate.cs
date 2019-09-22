using System;
namespace Hymnstagram.Api.Models.Creator
{
    public class CreatorCreate
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int CreativeTypeId { get; set; }
    }
}
