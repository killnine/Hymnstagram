using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Hymnstagram.Model.DataAccess;
using Hymnstagram.Model.DataTransfer;
using Hymnstagram.Model.Domain;
using Hymnstagram.Web.Models.Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Hymnstagram.Web.Controllers.Api
{
    [Route("api/songbooks/{songbookId}/songs")]
    public class SongController : Controller
    {
        private const int MAX_SONG_PAGE_SIZE = 50;
        private ILogger<SongController> _logger;
        private ISongbookRepository _repository;
        private IMapper _mapper;

        public SongController(ILogger<SongController> logger, IMapper mapper, ISongbookRepository repository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }
        
        [HttpGet]
        public IActionResult Get(Guid songbookId, [FromQuery]int pageNumber = 1, [FromQuery]int pageSize = 20)
        {
            _logger.LogDebug("SongController.Get called with pageNumber {@pageNumber} and {@pageSize}", pageNumber, pageSize);
            //TODO: Enable a way to retrieve child objects without returning the whole parent
            var songbook = _repository.GetById(songbookId);
            if (songbook == null)
            {
                return NotFound();
            }

            pageSize = (pageSize > MAX_SONG_PAGE_SIZE) ? MAX_SONG_PAGE_SIZE : pageSize;
            
            var results = _mapper.Map<IEnumerable<SongResult>>(songbook.Songs.Skip(pageSize * (pageNumber - 1)).Take(pageSize));

            return Ok(results);
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

            return Ok(_mapper.Map<SongResult>(song));
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

        [HttpDelete("{id}")]
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
    }
}
