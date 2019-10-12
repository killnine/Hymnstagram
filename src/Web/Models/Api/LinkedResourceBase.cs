using System.Collections.Generic;

namespace Hymnstagram.Web.Models.Api
{
    /// <summary>
    /// Base class for resources that implement HATEOAS linking
    /// </summary>
    public abstract class LinkedResourceBase
    {
        /// <summary>
        /// Collection of HATEOAS links to various resource actions.
        /// </summary>
        public List<Link> Links { get; set; } = new List<Link>();
    }
}
