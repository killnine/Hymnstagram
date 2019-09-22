using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Hymnstagram.Model.DataAccess;
using System;
using System.Linq;
using Model;
using Api.Models.Songbook;

namespace Hymnstagram.Web.Controllers.Api
{
    [Route("api/[controller]")]
    public class SongbookController : Controller
    {
        private const int MAX_SONGBOOK_PAGE_SIZE = 50;
        private readonly ILogger<SongbookController> _logger;
        private readonly ISongbookRepository _repository;

        public SongbookController(ILogger<SongbookController> logger, ISongbookRepository repository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet]
        public IEnumerable<Songbook> Get([FromQuery]int pageNumber = 1, [FromQuery]int pageSize = 20)
        {
            pageSize = (pageSize > MAX_SONGBOOK_PAGE_SIZE) ? MAX_SONGBOOK_PAGE_SIZE : pageSize;

            return _repository.GetSongbooks(pageNumber, pageSize);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public Songbook GetById(Guid id)
        {
            return _repository.GetById(id);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]SongbookCreate songbook)
        {
            //TODO: Convert from CreateDto to real object

            _repository.Save(Songbook.Create()); //TODO: implement me
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]SongbookCreate songbook)
        {
            _repository.Save(null);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            //TODO: Get object, mark deleted, save

            _repository.Save(null);
        }
    }
}
