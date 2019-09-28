using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Hymnstagram.Model.DataAccess;
using System;
using AutoMapper;
using Hymnstagram.Web.Models.Api;
using Hymnstagram.Model.Domain;
using Hymnstagram.Model.DataTransfer;

namespace Hymnstagram.Web.Controllers.Api
{
    [Route("api/songbooks")]
    public class SongbookController : Controller
    {
        private const int MAX_SONGBOOK_PAGE_SIZE = 50;
        private readonly ILogger<SongbookController> _logger;
        private readonly IMapper _mapper;
        private readonly ISongbookRepository _repository;
               
        public SongbookController(ILogger<SongbookController> logger, IMapper mapper, ISongbookRepository repository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet]
        public IActionResult Get([FromQuery]int pageNumber = 1, [FromQuery]int pageSize = 20)
        {
            _logger.LogDebug("SongbookController.Get called with pageNumber {@pageNumber} and {@pageSize}", pageNumber, pageSize);
            pageSize = (pageSize > MAX_SONGBOOK_PAGE_SIZE) ? MAX_SONGBOOK_PAGE_SIZE : pageSize;

            var results = _mapper.Map<IEnumerable<SongbookResult>>(_repository.GetSongbooks(pageNumber, pageSize));

            return Ok(results);
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
            return Ok(result);
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

        [HttpDelete("{id}")]
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
    }
}
