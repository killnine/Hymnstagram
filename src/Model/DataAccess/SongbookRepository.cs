using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Hymnstagram.Model.DataAccess.Criteria;
using Hymnstagram.Model.Domain;
using Hymnstagram.Model.DataTransfer;
using Hymnstagram.Web.Services;

namespace Hymnstagram.Model.DataAccess
{
    public class SongbookRepository : ISongbookRepository
    {
        private readonly ILogger<SongbookRepository> _logger;
        private readonly ISongbookDao _songbookDao;
        private readonly ISongDao _songDao;
        private readonly ICreatorDao _creatorDao;
        private readonly IPropertyMappingService _propertyMappingService;

        public SongbookRepository(ILogger<SongbookRepository> logger, ISongbookDao songbookDao, ISongDao songDao, ICreatorDao creatorDao, IPropertyMappingService propertyMappingService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _songbookDao = songbookDao ?? throw new ArgumentNullException(nameof(songbookDao));
            _songDao = songDao ?? throw new ArgumentNullException(nameof(songDao));
            _creatorDao = creatorDao ?? throw new ArgumentNullException(nameof(creatorDao));
            _propertyMappingService = propertyMappingService ?? throw new ArgumentNullException(nameof(propertyMappingService));
        }

        public Songbook GetById(Guid id)
        {
            //TODO: Profile me
            var songbook = _songbookDao.Get(id);

            if (songbook != null)
            {
                Hydrate(new List<SongbookDto> { songbook });

                return Songbook.From(songbook);
            }

            return null;
        }

        public PagedList<Songbook> GetSongbookByCriteria(SongbookSearchCriteria criteria)
        {
            var songbookDtos = _songbookDao.GetByCriteria(criteria);

            Hydrate(songbookDtos);

            var songbooks = songbookDtos.Select(Songbook.From);

            return PagedList<Songbook>.Create(songbooks, criteria.PageNumber, criteria.PageSize);
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
                SaveSong(song, songbook.Id);
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

        private void SaveSong(Song song, Guid parentId)
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
                song.SongbookId = parentId;
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
