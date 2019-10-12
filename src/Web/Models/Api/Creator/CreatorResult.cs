using System;

namespace Hymnstagram.Web.Models.Api
{
#pragma warning disable CS1591
    public class CreatorResult
    {
        public Guid Id { get; set; }
        public Guid ParentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Type { get; set; }
    }
#pragma warning restore CS1591
}
