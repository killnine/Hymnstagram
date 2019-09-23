using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Hymnstagram.Model.DataAccess;
using System;
using AutoMapper;
using Hymnstagram.Web.Models.Api;
using Hymnstagram.Model.Domain;

namespace Hymnstagram.Web.Controllers.Api
{
    [Route("api/[controller]")]
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
        public IEnumerable<SongbookResult> Get([FromQuery]int pageNumber = 1, [FromQuery]int pageSize = 20)
        {
            pageSize = (pageSize > MAX_SONGBOOK_PAGE_SIZE) ? MAX_SONGBOOK_PAGE_SIZE : pageSize;
            
            
            return _mapper.Map<IEnumerable<SongbookResult>>(_repository.GetSongbooks(pageNumber, pageSize));
        }

        [HttpGet("{id}")]
        public SongbookResult GetById(Guid id)
        {
            return _mapper.Map<SongbookResult>(_repository.GetById(id));
        }

        [HttpPost]
        public void Post([FromBody]SongbookCreate songbook)
        {
            //TODO: Convert from CreateDto to real object
            _repository.Save(Songbook.Create()); //TODO: implement me
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody]SongbookUpdate songbook)
        {
            _repository.Save(null);
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            //TODO: Get object, mark deleted, save

            _repository.Save(null);
        }
    }
}
