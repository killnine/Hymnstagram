using System.Collections.Generic;

namespace Hymnstagram.Web.Models.Api
{
    public abstract class LinkedResourceBase
    {
        public List<Link> Links { get; set; } = new List<Link>();
    }
}
