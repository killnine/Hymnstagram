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
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Hymnstagram.Web.Controllers.Api
{
    [Route("api/songbooks/{songbookId}/songs")]
    public class SongController : Controller
    {
        private const int MAX_SONG_PAGE_SIZE = 50;
        private readonly ILogger<SongController> _logger;
        private readonly ISongbookRepository _repository;
        private readonly IMapper _mapper;        

        public SongController(ILogger<SongController> logger, IMapper mapper, ISongbookRepository repository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));            
        }
        
        [HttpGet(Name = "GetSongs")]
        public IActionResult Get(SongResourceParameters parameters)
        {
            _logger.LogDebug("SongController.Get called with pageNumber {@pageNumber} and {@pageSize}", parameters.PageNumber, parameters.PageSize);                       
            
            //TODO: Enable a way to retrieve child objects without returning the whole parent
            var songbook = _repository.GetById(parameters.SongbookId);
            if (songbook == null)
            {
                return NotFound();
            }

                                   
            var songs = PagedList<SongResult>.Create(_mapper.Map<IEnumerable<SongResult>>(songbook.Songs.Skip(parameters.PageNumber - 1).Take(parameters.PageSize)), parameters.PageNumber, parameters.PageSize);

            var previousPageLink = songs.HasPrevious ? CreateSongResourceUri(parameters, ResourceUriType.PreviousPage) : null;
            var nextPageLink = songs.HasNext ? CreateSongResourceUri(parameters, ResourceUriType.NextPage) : null;

            var paginationMetadata = new
            {
                totalCount = songs.TotalCount,
                pageSize = songs.PageSize,
                currentPage = songs.CurrentPage,
                totalPages = songs.TotalPages,
                previousPageLink = previousPageLink,
                nextPageLink = nextPageLink
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            var results = _mapper.Map<IEnumerable<SongResult>>(songs);

            return Ok(results.Select(CreateLinksForSong));
        }

        [HttpGet("{id}", Name = "GetSong")]
        public IActionResult GetById(Guid songbookId, Guid id)
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

        [HttpPost]
        public IActionResult Post(Guid songbookId, [FromBody]SongCreate song)
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

            //TODO: Add validation
            songbook.Songs.Add(newSong);

            _repository.Save(songbook);

            return CreatedAtRoute("GetSong", new { songbookId = newSong.SongbookId, id = newSong.Id }, newSong);
        }

        [HttpDelete("{id}", Name = "DeleteSong")]
        public IActionResult Delete(Guid songbookId, Guid id)
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
