using Hymnstagram.Web.Models.Api;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Hymnstagram.Web.Controllers.Api
{
    [Route("api")]
    public class RootController : Controller
    {
        private const string VENDOR_MEDIA_TYPE = "application/vnd.hymnstagram.hateoas+json";
        private readonly IUrlHelper _urlHelper;

        public RootController(IUrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
        }

        [HttpGet(Name = "GetRoot")]
        public IActionResult GetRoot([FromHeader(Name = "Accept")] string mediaType)
        {
            if(mediaType == VENDOR_MEDIA_TYPE)
            {
                var links = new List<Link>();

                links.Add(new Link(_urlHelper.Link("GetRoot", new { }), "self", "GET"));
                links.Add(new Link(_urlHelper.Link("GetSongbooks", new { }), "songbooks", "GET"));
                links.Add(new Link(_urlHelper.Link("CreateSongbook", new { }), "create_songbook", "POST"));

                return Ok(links);
            }

            return NoContent();
        }
    }
}
