using Hymnstagram.Web.Models.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Hymnstagram.Web.Controllers.Api
{
    /// <summary>
    /// The Root controller is a default endpoint where users can get a summary
    /// of the most common actions available on the API.
    /// </summary>
    [Produces("application/json", "application/vnd.hymnstagram.hateoas+json")]
    [Consumes("application/json")]
    [Route("api")]    
    public class RootController : Controller
    {
        private const string VENDOR_MEDIA_TYPE = "application/vnd.hymnstagram.hateoas+json";
        private readonly ILogger<RootController> _logger;

        /// <summary>
        /// Root constructor.
        /// </summary>
        /// <param name="logger">Logging object (Microsoft.Extensions.Logging interface) for logging behavior and exceptions.</param>
        public RootController(ILogger<RootController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Returns the standard endpoints into the API
        /// </summary>
        /// <param name="mediaType">Use 'application/vnd.hymnstagram.hateoas+json' for HATEOAS media details.</param>
        /// <returns></returns>
        [HttpGet(Name = "GetRoot")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult GetRoot([FromHeader(Name = "Accept")] string mediaType)
        {
            _logger.LogDebug($"RootController.GetRoot called. (Header contents: {mediaType})");

            if (mediaType == VENDOR_MEDIA_TYPE)
            {
                var links = new List<Link>();

                links.Add(new Link(Url.Link("GetRoot", new { }), "self", "GET"));
                links.Add(new Link(Url.Link("GetSongbooks", new { }), "songbooks", "GET"));
                links.Add(new Link(Url.Link("CreateSongbook", new { }), "create_songbook", "POST"));

                return Ok(links);
            }

            return NoContent();
        }
    }
}
