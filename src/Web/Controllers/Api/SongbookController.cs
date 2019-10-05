using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Hymnstagram.Model.DataAccess;
using System;
using AutoMapper;
using Hymnstagram.Web.Models.Api;
using Hymnstagram.Model.Domain;
using Hymnstagram.Model.DataTransfer;
using Hymnstagram.Web.Helpers.Parameters;
using Hymnstagram.Model.DataAccess.Criteria;
using Hymnstagram.Web.Helpers;
using System.Text.Json;
using System.Linq;
using Hymnstagram.Web.Services;

namespace Hymnstagram.Web.Controllers.Api
{
    [Route("api/songbooks")]
    public class SongbookController : Controller
    {
        private readonly ILogger<SongbookController> _logger;
        private readonly IMapper _mapper;
        private readonly ISongbookRepository _repository;
        private readonly IUrlHelper _urlHelper;
        private readonly IPropertyMappingService _propertyMappingService;
        private const string VENDOR_MEDIA_TYPE = "application/vnd.hymnstagram.hateoas+json";

        public SongbookController(ILogger<SongbookController> logger, IMapper mapper, ISongbookRepository repository, IUrlHelper urlHelper, IPropertyMappingService propertyMappingService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _urlHelper = urlHelper ?? throw new ArgumentNullException(nameof(urlHelper));
            _propertyMappingService = propertyMappingService ?? throw new ArgumentNullException(nameof(propertyMappingService));
        }

        [HttpGet]
        public IActionResult Get(SongbookResourceParameters parameters, [FromHeader(Name = "Accept")] string mediaType)
        {
            _logger.LogDebug("SongbookController.Get called with pageNumber {@pageNumber} and {@pageSize}");            

            if(!_propertyMappingService.ValidMappingExistsFor<SongbookDto, Songbook>(parameters.OrderBy))
            {
                return BadRequest();
            }

            var songbooks = _repository.GetSongbookByCriteria(_mapper.Map<SongbookSearchCriteria>(parameters));


            var previousPageLink = songbooks.HasPrevious ? CreateSongbookResourceUri(parameters, ResourceUriType.PreviousPage) : null;
            var nextPageLink = songbooks.HasNext ? CreateSongbookResourceUri(parameters, ResourceUriType.NextPage) : null;

            var paginationMetadata = new
            {
                totalCount = songbooks.TotalCount,
                pageSize = songbooks.PageSize,
                currentPage = songbooks.CurrentPage,
                totalPages = songbooks.TotalPages,
                previousPageLink = previousPageLink,
                nextPageLink = nextPageLink
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            var results = _mapper.Map<IEnumerable<SongbookResult>>(songbooks);

            return Ok(results.Select(CreateLinksForSongbook));
        }

        [HttpGet("{id}", Name = "GetSongbook")]
        public IActionResult GetById(Guid id)
        {
            _logger.LogDebug("SongbookController.GetById called on id {@id}", id);
            var songbook = _repository.GetById(id);
            if(songbook == null)
            {
                return NotFound();
            }

            var result = _mapper.Map<SongbookResult>(songbook);
            return Ok(CreateLinksForSongbook(result));
        }

        [HttpPost]
        public IActionResult Post([FromBody]SongbookCreate songbook)
        {
            if(songbook == null)
            {
                return BadRequest();
            }

            _logger.LogDebug("SongbookController.Post called to create new songbook: {@songbook}", songbook);
            var dto = _mapper.Map<SongbookDto>(songbook);
            var newSongbook = Songbook.From(dto);

            //TODO: Add validation

            _repository.Save(newSongbook);

            return CreatedAtRoute("GetSongbook", new { id = newSongbook.Id }, newSongbook);
        }

        [HttpDelete("{id}", Name = "DeleteSongbook")]
        public IActionResult Delete(Guid id)
        {
            if (id == null || id == Guid.Empty)
            {
                return BadRequest();
            };

            _logger.LogDebug("SongbookController.Delete called to remove songbook {@id}", id);
            var songbook = _repository.GetById(id);
            if(songbook == null)
            {
                _logger.LogWarning("SongbookController.Delete failed to remove songbook {@id}. It was not found.", id);
                return NotFound();
            }

            songbook.Destroy();
            _repository.Save(songbook);

            return Ok();
        }

        private string CreateSongbookResourceUri(SongbookResourceParameters parameters, ResourceUriType type)
        {
            switch(type)
            {
                case ResourceUriType.PreviousPage:
                    return _urlHelper.Link("GetSongbooks", new
                    {
                        pageNumber = parameters.PageNumber - 1,
                        pageSize = parameters.PageSize
                    });
                case ResourceUriType.NextPage:
                    return _urlHelper.Link("GetSongbooks", new
                    {
                        pageNumber = parameters.PageNumber + 1,
                        pageSize = parameters.PageSize
                    });
                default:
                    return _urlHelper.Link("GetAuthors", new
                    {
                        pageNumber = parameters.PageNumber,
                        pageSize = parameters.PageSize
                    });
            }
        }

        private SongbookResult CreateLinksForSongbook(SongbookResult songbook)
        {
            songbook.Links.Add(new Link(_urlHelper.Link("GetSongbook", new { id = songbook.Id }), "self", "GET"));
            songbook.Links.Add(new Link(_urlHelper.Link("DeleteSongbook", new { id = songbook.Id }), "delete_songbook", "DELETE"));            

            return songbook;
        }
    }
}
