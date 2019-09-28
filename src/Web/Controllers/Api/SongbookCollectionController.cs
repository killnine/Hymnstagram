using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Hymnstagram.Model.DataAccess;
using Hymnstagram.Model.DataAccess.Criteria;
using Hymnstagram.Model.DataTransfer;
using Hymnstagram.Model.Domain;
using Hymnstagram.Web.Helpers;
using Hymnstagram.Web.Models.Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Hymnstagram.Web.Controllers.Api
{
    [Route("api/songbookcollections")]
    public class SongbookCollectionController : Controller
    {
        private readonly ILogger<SongbookCollectionController> _logger;
        private readonly IMapper _mapper;
        private readonly ISongbookRepository _repository;

        public SongbookCollectionController(ILogger<SongbookCollectionController> logger, IMapper mapper, ISongbookRepository repository)
        {
            _logger = logger;
            _mapper = mapper;
            _repository = repository;
        }

        [HttpGet("({ids})", Name = "GetSongbookCollection")]
        public IActionResult GetSongbookCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))]IEnumerable<Guid> ids)
        {
            if(ids == null)
            {
                return BadRequest();
            }

            _logger.LogDebug("SongbookCollectionController.Get called for ids {@ids}.", ids);
            var songbooks = _repository.GetSongbookByCriteria(new SongbookSearchCriteria() { Ids = ids.ToList() }, 1, ids.Count());

            if(ids.Count() != songbooks.Count())
            {
                return NotFound();
            }

            var songbooksToReturn = _mapper.Map<IEnumerable<SongbookResult>>(songbooks);
            return Ok(songbooksToReturn);
        }

        [HttpPost]
        public IActionResult Post([FromBody]IEnumerable<SongbookCreate> songbookCollection)
        {
            if(songbookCollection == null)
            {
                return BadRequest();
            }

            _logger.LogDebug("SongbookCollectionController.Post called to add a new collection of songbooks {@songbookCollection}", songbookCollection);
            var songbooks = _mapper.Map<IEnumerable<SongbookDto>>(songbookCollection).Select(Songbook.From).ToList();
            foreach(var songbook in songbooks)
            {                
                _repository.Save(songbook);
            }

            var idsAsString = string.Join(",", songbooks.Select(sb => sb.Id));

            return CreatedAtRoute("GetSongbookCollection", new { ids = idsAsString }, _mapper.Map<IEnumerable<SongbookResult>>(songbooks));
        }
    }
}
