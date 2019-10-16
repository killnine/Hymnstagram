using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using AutoMapper;
using FluentValidation.AspNetCore;
using Hymnstagram.Model.DataAccess;
using Hymnstagram.Model.DataTransfer;
using Hymnstagram.Model.Domain;
using Hymnstagram.Web.Helpers;
using Hymnstagram.Web.Helpers.Parameters;
using Hymnstagram.Web.Models.Api;
using Hymnstagram.Web.Models.Api.Song;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Hymnstagram.Web.Controllers.Api
{
    /// <summary>
    /// The Song controller enables users to create, read, and delete songs from a specific songbook.
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("api/songbooks/{songbookId}/songs")]    
    public class SongController : Controller
    {
        private readonly ILogger<SongController> _logger;
        private readonly ISongbookRepository _repository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Song constructor.
        /// </summary>
        /// <param name="logger">Logging object (Microsoft.Extensions.Logging interface) for logging behavior and exceptions.</param>
        /// <param name="mapper">Automapper object for converting domain objects to models and vice versa for communicating with the client.</param>
        /// <param name="repository">Data access repository.</param>
        public SongController(ILogger<SongController> logger, IMapper mapper, ISongbookRepository repository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));            
        }

        /// <summary>
        /// Retrieves a list of songs based on search, sorting, and filtering criteria.
        /// </summary>
        /// <param name="parameters">Parameters includes pagination settings, search criteria, sorting criteria, and filtering criteria.</param>     
        /// <remarks>A song requires the SongbookId parameter be passed in.</remarks>        
        [HttpGet(Name = "GetSongs")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]        
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<SongResult>> Get(SongResourceParameters parameters)
        {
            _logger.LogDebug("SongController.Get called with pageNumber {@pageNumber} and {@pageSize}", parameters.PageNumber, parameters.PageSize);                       
            
            //TODO: Enable a way to retrieve child objects without returning the whole parent
            var songbook = _repository.GetById(parameters.SongbookId);
            if (songbook == null)
            {
                return NotFound();
            }
                                   
            var songs = PagedList<SongResult>.Create(_mapper.Map<IEnumerable<SongResult>>(songbook.Songs.Skip(parameters.PageNumber - 1).Take(parameters.PageSize)), parameters.PageNumber, parameters.PageSize);

            var paginationMetadata = new
            {
                totalCount = songs.TotalCount,
                pageSize = songs.PageSize,
                currentPage = songs.CurrentPage,
                totalPages = songs.TotalPages                
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            var songResults = _mapper.Map<IEnumerable<SongResult>>(songs);
            var collectionResult = new SongCollectionResult() { Results = songResults, Links = CreateLinksForSongs(parameters, songs.HasNext, songs.HasPrevious) };

            return Ok(collectionResult);
        }

        /// <summary>
        /// Retrieves a single song and all child content
        /// </summary>
        /// <param name="songbookId">Guid-based identifier for the songbook (song parent)</param>
        /// <param name="songId">Guid-based identifier for the song</param>
        /// <returns>Returns song object and related creators.</returns>
        [HttpGet("{songId}", Name = "GetSong")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]        
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<SongResult> GetById(Guid songbookId, Guid songId)
        {
            _logger.LogDebug("SongController.GetById called on id {@id} (songbook: {@songbookId})", songId, songbookId);
            var songbook = _repository.GetById(songbookId);
            if(songbook == null)
            {
                return NotFound();
            }

            var song = songbook.Songs.FirstOrDefault(s => s.Id == songId);
            if(song == null)
            {
                return NotFound();
            }

            var result = _mapper.Map<SongResult>(song);

            return Ok(CreateLinksForSong(result));
        }

        /// <summary>
        /// Submits a new songbook to the system.
        /// </summary>
        /// <param name="songbookId">Guid-based identifier for the songbook the song will be applied to</param>
        /// <param name="song">New song object</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public ActionResult<SongResult> Post(Guid songbookId, [FromBody]SongCreate song)
        {
            if(song == null)
            {
                return BadRequest();
            }

            _logger.LogDebug("SongController.Post called to create new song for songbook {@songbookId}: {@songbook}", songbookId, song);
            var songbook = _repository.GetById(songbookId);
            if(songbook == null)
            {
                _logger.LogWarning("Songbook.Post failed to add song {@song} to songbook {@songbookId}. Songbook was not found.", song, songbookId);
                return NotFound();
            }
            
            var dto = _mapper.Map<SongDto>(song);
            var newSong = Song.From(dto);

            //Perform validation
            if (!newSong.IsValid)
            {
                newSong.Validate().AddToModelState(ModelState, null);
                _logger.LogWarning("{method} failed model validation (ModelState: {@modelState}), returning Unprocessable Entity", nameof(Post), ModelState.Values.SelectMany(v => v.Errors));
                return ValidationProblem(ModelState);
            }

            songbook.Songs.Add(newSong);

            _repository.Save(songbook);

            return CreatedAtRoute("GetSong", new { songbookId = newSong.SongbookId, id = newSong.Id }, newSong);
        }

        [HttpPatch("{songId}")]
        public ActionResult Patch(Guid songbookId, Guid songId, JsonPatchDocument<SongPatch> patchDocument)
        {
            if(patchDocument == null)
            {
                return BadRequest();
            }

            _logger.LogDebug("SongController.Patch called to update song for songbook {@songbookId}: {@patchDocument}", songbookId, patchDocument);
            var songbook = _repository.GetById(songbookId);
            if (songbook == null)
            {
                _logger.LogWarning("Songbook.Patch failed to update song path {@patchDocument} to songbook {@songbookId}. Songbook was not found.", patchDocument, songbookId);
                return NotFound();
            }

            var existingSong = songbook.Songs.FirstOrDefault(s => s.Id == songId);
            if(existingSong == null)
            {
                _logger.LogWarning("Songbook.Patch failed to update song {@patchDocument} to songbook {@songbookId}. Song was not found in songbook.", patchDocument, songbookId);
                return NotFound();
            }

            var songToPatch =_mapper.Map<SongPatch>(existingSong.ToDto());
            patchDocument.ApplyTo(songToPatch, ModelState);

            if(!TryValidateModel(songToPatch))
            {
                return ValidationProblem(ModelState);
            }
            
            songbook.Songs.Remove(existingSong);
            songbook.Songs.Add(Song.From(_mapper.Map<SongDto>(songToPatch)));

            _repository.Save(songbook); 

            return NoContent();            
        }

        /// <summary>
        /// Deletes a song from the system based on a specific songbook id and song id.
        /// </summary>
        /// <param name="songbookId">Guid-based identifier for the songbook (song parent)</param>
        /// <param name="songId">Guid-based identifier for the song</param>
        /// <returns></returns>
        [HttpDelete("{songId}", Name = "DeleteSong")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]        
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult Delete(Guid songbookId, Guid songId)
        {
            if (songId == null || songId == Guid.Empty)
            {
                return BadRequest();
            };

            _logger.LogDebug("SongController.Delete called to remove song {@id} from songbook {@songbookId}", songId, songbookId);
            var songbook = _repository.GetById(songbookId);
            if (songbook == null)
            {
                _logger.LogWarning("SongController.Delete failed to remove song {@id} because songbook {@songbookId} was not found.", songId, songbookId);
                return NotFound();
            }

            var song = songbook.Songs.FirstOrDefault(s => s.Id == songId);
            if(song == null)
            {
                _logger.LogWarning("Song {@id} was not removed because it was not found on songbook {@songbookId}.", songId, songbookId);
                return NotFound();
            }

            song.Destroy();
            _repository.Save(songbook);

            return Ok();
        }

        private string CreateSongResourceUri(SongResourceParameters parameters, ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link("GetSongs", new
                    {
                        pageNumber = parameters.PageNumber - 1,
                        pageSize = parameters.PageSize
                    });
                case ResourceUriType.NextPage:
                    return Url.Link("GetSongs", new
                    {
                        pageNumber = parameters.PageNumber + 1,
                        pageSize = parameters.PageSize
                    });
                default:
                    return Url.Link("GetSongs", new
                    {
                        pageNumber = parameters.PageNumber,
                        pageSize = parameters.PageSize
                    });
            }
        }

        private SongResult CreateLinksForSong(SongResult song)
        {
            song.Links.Add(new Link(Url.Link("GetSong", new { songbookId = song.SongbookId, id = song.Id }), "self", "GET"));
            song.Links.Add(new Link(Url.Link("DeleteSong", new { songbookId = song.SongbookId, id = song.Id }), "delete_song", "DELETE"));            

            return song;
        }

        private List<Link> CreateLinksForSongs(SongResourceParameters parameters, bool hasNext, bool hasPrevious)
        {
            var links = new List<Link>();

            links.Add(new Link(CreateSongResourceUri(parameters, ResourceUriType.Current), "self", "GET"));
            links.Add(new Link(CreateSongResourceUri(parameters, ResourceUriType.Current), "delete_song", "DELETE"));            

            if(hasNext)
            {
                links.Add(new Link(CreateSongResourceUri(parameters, ResourceUriType.NextPage), "next_page", "GET"));
            }

            if(hasPrevious)
            {
                links.Add(new Link(CreateSongResourceUri(parameters, ResourceUriType.PreviousPage), "previous_page", "GET"));
            }

            return links;
        }

        /// <summary>
        /// Overrides default ValidationProblem behavior to allow us to use the ApiBehavior set up 
        /// in Startup.cs
        /// </summary>
        /// <param name="modelStateDictionary">Current ModelState of the controller action</param>
        /// <returns></returns>
        public override ActionResult ValidationProblem([ActionResultObjectValue] ModelStateDictionary modelStateDictionary)
        {
            var options = HttpContext.RequestServices.GetRequiredService<IOptions<ApiBehaviorOptions>>();
            return (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext);
        }
    }
}
