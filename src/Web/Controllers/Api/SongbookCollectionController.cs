using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FluentValidation.AspNetCore;
using Hymnstagram.Model.DataAccess;
using Hymnstagram.Model.DataAccess.Criteria;
using Hymnstagram.Model.DataTransfer;
using Hymnstagram.Model.Domain;
using Hymnstagram.Web.Helpers;
using Hymnstagram.Web.Models.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Hymnstagram.Web.Controllers.Api
{
    /// <summary>
    /// The SongbookCollection controller enables users to submit multiple songbooks into the system
    /// via a single API call.
    /// </summary>
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("api/songbookcollections")]
    public class SongbookCollectionController : Controller
    {
        private readonly ILogger<SongbookCollectionController> _logger;
        private readonly IMapper _mapper;
        private readonly ISongbookRepository _repository;        

        /// <summary>
        /// SongbookCollection constructor.
        /// </summary>
        /// <param name="logger">Logging object (Microsoft.Extensions.Logging interface) for logging behavior and exceptions.</param>
        /// <param name="mapper">Automapper object for converting domain objects to models and vice versa for communicating with the client.</param>
        /// <param name="repository">Data access repository.</param>
        public SongbookCollectionController(ILogger<SongbookCollectionController> logger, IMapper mapper, ISongbookRepository repository)
        {
            _logger = logger;
            _mapper = mapper;
            _repository = repository;            
        }

        /// <summary>
        /// Retrieves a collection of songbooks, given a list of songbook ids.
        /// </summary>        
        /// <param name="ids">Comma-separated list of songbook guids, wrapped in parenthesis.</param>
        /// <returns></returns>
        [HttpGet("({ids})", Name = "GetSongbookCollection")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<SongbookResult>> GetSongbookCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))]IEnumerable<Guid> ids)
        {
            if(ids == null)
            {
                return BadRequest();
            }

            _logger.LogDebug("SongbookCollectionController.Get called for ids {@ids}.", ids);
            var songbooks = _repository.GetSongbookByCriteria(new SongbookSearchCriteria() { Ids = ids.ToList(), PageNumber = 1, PageSize = ids.Count() });

            if(ids.Count() != songbooks.Count())
            {
                return NotFound();
            }

            var songbooksToReturn = _mapper.Map<IEnumerable<SongbookResult>>(songbooks);
            return Ok(songbooksToReturn.Select(CreateLinksForSongbook));
        }

        /// <summary>
        /// Submit a list of songbooks to the API at once.
        /// </summary>
        /// <param name="songbookCollection">An array of Songbook creation objects.</param>
        /// <returns></returns>
        [HttpPost]        
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public ActionResult<IEnumerable<SongbookResult>> Post([FromBody]IEnumerable<SongbookCreate> songbookCollection)
        {
            if(songbookCollection == null)
            {
                return BadRequest();
            }

            _logger.LogDebug("SongbookCollectionController.Post called to add a new collection of songbooks {@songbookCollection}", songbookCollection);
            var songbooks = _mapper.Map<IEnumerable<SongbookDto>>(songbookCollection).Select(Songbook.From).ToList();

            //Perform validation
            foreach(var songbook in songbooks)
            {
                if (!songbook.IsValid)
                {
                    songbook.Validate().AddToModelState(ModelState, $"({songbook.Title})");
                    _logger.LogWarning("{method} failed model validation (ModelState: {@modelState}), returning Unprocessable Entity", nameof(Post), ModelState.Values.SelectMany(v => v.Errors));                    
                }                
            }
            if (!ModelState.IsValid)
            {
                return InvalidModelStateResponseFactory.GenerateResponseForInvalidModelState(ModelState, HttpContext);
            }

            foreach (var songbook in songbooks)
            {                
                _repository.Save(songbook);
            }

            var idsAsString = string.Join(",", songbooks.Select(sb => sb.Id));

            return CreatedAtRoute("GetSongbookCollection", new { ids = idsAsString }, _mapper.Map<IEnumerable<SongbookResult>>(songbooks).Select(CreateLinksForSongbook));
        }

        private SongbookResult CreateLinksForSongbook(SongbookResult songbook)
        {
            songbook.Links.Add(new Link(Url.Link("GetSongbook", new { id = songbook.Id }), "self", "GET"));
            songbook.Links.Add(new Link(Url.Link("DeleteSongbook", new { id = songbook.Id }), "delete_songbook", "DELETE"));

            return songbook;
        }
    }
}
