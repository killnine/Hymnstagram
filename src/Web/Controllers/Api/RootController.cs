using Hymnstagram.Web.Models.Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Hymnstagram.Web.Controllers.Api
{
    [Route("api")]
    public class RootController : Controller
    {
        private const string VENDOR_MEDIA_TYPE = "application/vnd.hymnstagram.hateoas+json";
        private readonly ILogger<RootController> _logger;

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
        public IActionResult GetRoot([FromHeader(Name = "Accept")] string mediaType)
        {
            _logger.LogDebug("RootController.GetRoot called.");

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
