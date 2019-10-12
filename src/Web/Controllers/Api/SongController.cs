using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using AutoMapper;
using Hymnstagram.Model.DataAccess;
using Hymnstagram.Model.DataTransfer;
using Hymnstagram.Model.Domain;
using Hymnstagram.Web.Helpers;
using Hymnstagram.Web.Helpers.Parameters;
using Hymnstagram.Web.Models.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Hymnstagram.Web.Controllers.Api
{
    /// <summary>
    /// The Song controller enables users to create, read, and delete songs from a specific songbook.
    /// </summary>
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
                totalPages = songs.TotalPages,
                previousPageLink = songs.HasPrevious ? CreateSongResourceUri(parameters, ResourceUriType.PreviousPage) : null,
                nextPageLink = songs.HasNext ? CreateSongResourceUri(parameters, ResourceUriType.NextPage) : null
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            var results = _mapper.Map<IEnumerable<SongResult>>(songs);

            return Ok(results.Select(CreateLinksForSong));
        }

        /// <summary>
        /// Retrieves a single song and all child content
        /// </summary>
        /// <param name="songbookId">Guid-based identifier for the songbook (song parent)</param>
        /// <param name="id">Guid-based identifier for the song</param>
        /// <returns>Returns song object and related creators.</returns>
        [HttpGet("{id}", Name = "GetSong")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]        
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<SongResult> GetById(Guid songbookId, Guid id)
        {
            _logger.LogDebug("SongController.GetById called on id {@id} (songbook: {@songbookId})", id, songbookId);
            var songbook = _repository.GetById(songbookId);
            if(songbook == null)
            {
                return NotFound();
            }

            var song = songbook.Songs.FirstOrDefault(s => s.Id == id);
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

            songbook.Songs.Add(newSong);

            _repository.Save(songbook);

            return CreatedAtRoute("GetSong", new { songbookId = newSong.SongbookId, id = newSong.Id }, newSong);
        }

        /// <summary>
        /// Deletes a song from the system based on a specific songbook id and song id.
        /// </summary>
        /// <param name="songbookId">Guid-based identifier for the songbook (song parent)</param>
        /// <param name="id">Guid-based identifier for the song</param>
        /// <returns></returns>
        [HttpDelete("{id}", Name = "DeleteSong")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]        
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult Delete(Guid songbookId, Guid id)
        {
            if (id == null || id == Guid.Empty)
            {
                return BadRequest();
            };

            _logger.LogDebug("SongController.Delete called to remove song {@id} from songbook {@songbookId}", id, songbookId);
            var songbook = _repository.GetById(songbookId);
            if (songbook == null)
            {
                _logger.LogWarning("SongController.Delete failed to remove song {@id} because songbook {@songbookId} was not found.", id, songbookId);
                return NotFound();
            }

            var song = songbook.Songs.FirstOrDefault(s => s.Id == id);
            if(song == null)
            {
                _logger.LogWarning("Song {@id} was not removed because it was not found on songbook {@songbookId}.", id, songbookId);
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
    }
}
