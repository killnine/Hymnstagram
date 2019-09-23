using System;

namespace Hymnstagram.Web.Models.Api
{
    public class CreatorResult
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Type { get; private set; }
    }
}
