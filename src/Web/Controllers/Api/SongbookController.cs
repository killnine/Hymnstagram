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
using Microsoft.AspNetCore.Http;

namespace Hymnstagram.Web.Controllers.Api
{
    /// <summary>
    /// The Songbook controller enables users to create, read, and delete songbooks from the system. 
    /// </summary>
    [Produces("application/json")]
    [Route("api/songbooks")]
    public class SongbookController : Controller
    {
        private readonly ILogger<SongbookController> _logger;
        private readonly IMapper _mapper;
        private readonly ISongbookRepository _repository;        
        private readonly IPropertyMappingService _propertyMappingService;

        /// <summary>
        /// Songbook constructor.
        /// </summary>
        /// <param name="logger">Logging object (Microsoft.Extensions.Logging interface) for logging behavior and exceptions.</param>
        /// <param name="mapper">Automapper object for converting domain objects to models and vice versa for communicating with the client.</param>
        /// <param name="repository">Data access repository.</param>
        /// <param name="propertyMappingService">The property-mapping service enables sorting by cross-referencing string field names to properties on the songbook objects</param>
        public SongbookController(ILogger<SongbookController> logger, IMapper mapper, ISongbookRepository repository, IPropertyMappingService propertyMappingService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));            
            _propertyMappingService = propertyMappingService ?? throw new ArgumentNullException(nameof(propertyMappingService));
        }

        /// <summary>
        /// Retrieves a list of songbooks based on search, sorting, and filtering criteria.
        /// </summary>
        /// <param name="parameters">Parameters includes pagination settings, search criteria, sorting criteria, and filtering criteria.</param>        
        [HttpGet(Name = "GetSongbooks")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Get(SongbookResourceParameters parameters)
        {
            _logger.LogDebug("SongbookController.Get called with pageNumber {@pageNumber} and {@pageSize}", parameters.PageNumber, parameters.PageSize);            

            if(!_propertyMappingService.ValidMappingExistsFor<SongbookDto, Songbook>(parameters.OrderBy))
            {
                return BadRequest();
            }

            var songbooks = _repository.GetSongbookByCriteria(_mapper.Map<SongbookSearchCriteria>(parameters));           
            
            var paginationMetadata = new
            {
                totalCount = songbooks.TotalCount,
                pageSize = songbooks.PageSize,
                currentPage = songbooks.CurrentPage,
                totalPages = songbooks.TotalPages,
                previousPageLink = songbooks.HasPrevious ? CreateSongbookResourceUri(parameters, ResourceUriType.PreviousPage) : null,
                nextPageLink = songbooks.HasNext ? CreateSongbookResourceUri(parameters, ResourceUriType.NextPage) : null
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            var results = _mapper.Map<IEnumerable<SongbookResult>>(songbooks);

            return Ok(results.Select(CreateLinksForSongbook));
        }

        /// <summary>
        /// Retrieves a single songbook and all child content
        /// </summary>
        /// <param name="id">Guid-based identifier for the songbook</param>
        /// <returns>Returns songbook object, related creators, and related songs.</returns>
        [HttpGet("{id}", Name = "GetSongbook")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
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

        /// <summary>
        /// Submits a new songbook to the system.
        /// </summary>
        /// <param name="songbook">Songbook object with all child references (Creators, Songs)</param>        
        [HttpPost(Name = "CreateSongbook")]        
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status201Created)]
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

        /// <summary>
        /// Deletes a songbook from the system based on a specific songbook id.
        /// </summary>
        /// <param name="id">Guid-based songbook identifier.</param>
        /// <returns></returns>
        [HttpDelete("{id}", Name = "DeleteSongbook")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
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
                    return Url.Link("GetSongbooks", new
                    {
                        pageNumber = parameters.PageNumber - 1,
                        pageSize = parameters.PageSize
                    });
                case ResourceUriType.NextPage:
                    return Url.Link("GetSongbooks", new
                    {
                        pageNumber = parameters.PageNumber + 1,
                        pageSize = parameters.PageSize
                    });
                default:
                    return Url.Link("GetSongbooks", new
                    {
                        pageNumber = parameters.PageNumber,
                        pageSize = parameters.PageSize
                    });
            }
        }

        private SongbookResult CreateLinksForSongbook(SongbookResult songbook)
        {
            songbook.Links.Add(new Link(Url.Link("GetSongbook", new { id = songbook.Id }), "self", "GET"));
            songbook.Links.Add(new Link(Url.Link("DeleteSongbook", new { id = songbook.Id }), "delete_songbook", "DELETE"));            

            return songbook;
        }
    }
}
