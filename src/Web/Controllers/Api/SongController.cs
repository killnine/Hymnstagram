using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Hymnstagram.Model.DataAccess;
using Hymnstagram.Web.Models.Api;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Hymnstagram.Web.Controllers.Api
{
    public class SongController : Controller
    {
        private ISongbookRepository _repository;
        private IMapper _mapper;

        public SongController(ISongbookRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // GET: api/values
        [HttpGet("api/songbook/{songbookId}/song")]
        public IActionResult Get(Guid songbookId)
        {
            var songbook = _repository.GetById(songbookId);
            if (songbook == null)
            {
                return NotFound();
            }

            var results = _mapper.Map<IEnumerable<SongResult>>(songbook.Songs);

            return Ok(results);
        }

        // GET api/values/5
        [HttpGet("api/songbook/{songbookId}/songs/{id}")]
        public IActionResult Get(Guid songbookId, Guid id)
        {
            var songbook = _repository.GetById(songbookId);
            if(songbook == null)
            {
                return NotFound();
            }

            return Ok();
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
