using System;
using System.Collections.Generic;
using System.Linq;
using Hymnstagram.Model.DataAccess;
using Hymnstagram.Model.DataAccess.Criteria;
using Hymnstagram.Model.DataAccess.Daos;
using Microsoft.Extensions.Logging;
using Model;

namespace DataAccess.Memory
{
    public class SongbookRepository : ISongbookRepository
    {
        private ILogger<SongbookRepository> _logger;
        private ISongbookDao _songbookDao;
        private ISongDao _songDao;
        private ICreatorDao _creatorDao;

        public SongbookRepository(ILogger<SongbookRepository> logger, ISongbookDao songbookDao, ISongDao songDao, ICreatorDao creatorDao)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _songbookDao = songbookDao ?? throw new ArgumentNullException(nameof(songbookDao));
            _songDao = songDao ?? throw new ArgumentNullException(nameof(songDao));
            _creatorDao = creatorDao ?? throw new ArgumentNullException(nameof(creatorDao));
        }

        //TODO: Don't return domain object. Return Model
        public Songbook GetById(Guid id)
        {
            return Songbook.From(_songbookDao.Get(id));
        }

        //TODO: Don't return domain object. Return Model
        public IList<Songbook> GetSongbooks(int pageNumber = 1, int pageSize = 10)
        {
            //TODO: Implement maximum page size check

            return _songbookDao.Get(pageNumber, pageSize).Select(Songbook.From).ToList();
        }

        //TODO: Don't return domain object. Return Model
        public IList<Songbook> GetSongbooksByTitleWildcard(string partialTitle, int pageNumber = 1, int pageSize = 10)
        {
            //TODO: Implement maximum page size check

            return _songbookDao.GetByCriteria(new SongbookSearchCriteria { Title = partialTitle }, pageNumber, pageSize)
                             .OrderBy(sb => sb.Title)
                             .Skip(pageSize * (pageNumber - 1))
                             .Take(pageSize)
                             .Select(Songbook.From)
                             .ToList();
        }
        
        public void Save(Songbook songbook)
        {
            //TODO: Implement me.
            throw new NotImplementedException();
        }

        private Songbook Hydrate(SongbookDto dto)
        {
            //TODO: Hydrate into complete domain objects here
            throw new NotImplementedException();
        }
    }
}
