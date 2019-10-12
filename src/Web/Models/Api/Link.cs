namespace Hymnstagram.Web.Models.Api
{
    /// <summary>
    /// Link object used by Result classes for implementing HATEOAS
    /// (Hypertext as the engine of application state) links to actions
    /// on a resource.
    /// </summary>
    public class Link
    {
        /// <summary>
        /// Url to action.
        /// </summary>
        public string Href { get; private set; }

        /// <summary>
        /// Description of action
        /// </summary>
        /// <remarks>Should be lower-case with words separated by underscores.</remarks>
        /// <example>delete_song</example>
        public string Rel { get; private set; }

        /// <summary>
        /// HTTP verb of action
        /// </summary>
        public string Method { get; private set; }

        /// <summary>
        /// Link constructor.
        /// </summary>
        /// <param name="href">Url to action.</param>
        /// <param name="rel">Description of action</param>
        /// <param name="method">HTTP verb of action</param>
        public Link(string href, string rel, string method)
        {
            Href = href;
            Rel = rel;
            Method = method;
        }
    }
}
