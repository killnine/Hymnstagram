using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Hymnstagram.Model.DataAccess.Criteria;
using Hymnstagram.Model.Domain;
using Hymnstagram.Model.DataTransfer;

namespace Hymnstagram.Model.DataAccess
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

        public Songbook GetById(Guid id)
        {
            //TODO: Profile me
            var songbook = _songbookDao.Get(id);

            Hydrate(new List<SongbookDto> { songbook });

            return Songbook.From(songbook);
        }

        public IList<Songbook> GetSongbooks(int pageNumber = 1, int pageSize = 10)
        {
            //TODO: Profile me
            var songbookDtos = _songbookDao.Get(pageNumber, pageSize).ToList();

            Hydrate(songbookDtos);

            return songbookDtos.Select(Songbook.From).ToList();
        }

        public IList<Songbook> GetSongbooksByTitleWildcard(string partialTitle, int pageNumber = 1, int pageSize = 10)
        {
            //TODO: Profile me
            var songbookDtos = _songbookDao.GetByCriteria(new SongbookSearchCriteria { Title = partialTitle }, pageNumber, pageSize)
                             .OrderBy(sb => sb.Title)
                             .Skip(pageSize * (pageNumber - 1))
                             .Take(pageSize)                             
                             .ToList();

            Hydrate(songbookDtos);

            return songbookDtos.Select(Songbook.From).ToList();
        }
        
        public void Save(Songbook songbook)
        {
            //TODO: Profile me
            if(songbook.IsDestroyed)
            {
                foreach(var song in songbook.Songs)
                {
                    foreach(var creator in song.Creators)
                    {
                        _creatorDao.Delete(creator.Id);
                    }

                    _songDao.Delete(song.Id);
                }

                _songbookDao.Delete(songbook.Id);
            }

            if(songbook.IsNew)
            {
                songbook.Id = Guid.NewGuid();
                _songbookDao.Insert(songbook.ToDto());
            }
            else
            {
                _songbookDao.Update(songbook.ToDto());
            }

            foreach(var creator in songbook.Creators)
            {
                SaveCreator(creator, songbook.Id);
            }

            foreach(var song in songbook.Songs)
            {
                SaveSong(song);
            }
        }

        private void SaveCreator(Creator creator, Guid parentId)
        {
            if(creator.IsDestroyed)
            {
                _creatorDao.Delete(creator.Id);
            }

            if(creator.IsNew)
            {
                creator.Id = Guid.NewGuid();
                creator.ParentId = parentId;
                _creatorDao.Insert(creator.ToDto());
            }
            else
            {
                _creatorDao.Update(creator.ToDto());
            }
        }

        private void SaveSong(Song song)
        {
            if(song.IsDestroyed)
            {
                foreach (var creator in song.Creators)
                {
                    _creatorDao.Delete(creator.Id);
                }

                _songDao.Delete(song.Id);
            }

            if (song.IsNew)
            {
                song.Id = Guid.NewGuid();
                _songDao.Insert(song.ToDto());                
            }
            else
            {
                _songDao.Update(song.ToDto());
            }

            foreach (var creator in song.Creators)
            {
                SaveCreator(creator, song.Id);
            }
        }

        private void Hydrate(IList<SongbookDto> dtos)
        {
            //NOTE: Very naive way to populate objects. Batching would be much more performant. Let's use Miniprofiler and Benchmark.net to test that theory.
            foreach(var dto in dtos)
            {
                var songs = _songDao.GetByCriteria(new SongSearchCriteria { SongbookId = dto.Id });
                dto.Songs = songs;
                foreach (var song in songs)
                {
                    var songCreators = _creatorDao.GetByCriteria(new CreatorSearchCriteria { ParentId = song.Id, ParentType = CreatorParentType.Song });
                    song.Creators = songCreators;
                }                

                var songbookCreators = _creatorDao.GetByCriteria(new CreatorSearchCriteria { ParentId = dto.Id, ParentType = CreatorParentType.Songbook });
                dto.Creators = songbookCreators;
            }      
        }
    }
}
