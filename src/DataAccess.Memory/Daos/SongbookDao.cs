using System;
using System.Collections.Generic;
using System.Linq;
using Hymnstagram.Model.DataAccess;
using Hymnstagram.Model.DataAccess.Criteria;
using Hymnstagram.Model.DataTransfer;
using Microsoft.Extensions.Logging;

namespace DataAccess.Memory.Daos
{
    public class SongbookDao : ISongbookDao
    {
        private ILogger<SongbookDao> _logger;

        public SongbookDao(ILogger<SongbookDao> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Delete(Guid id)
        {
            _logger.LogInformation("Deleting 'Songbook' record with id {@id}", id);

            var songbookToRemove = DataSource.Songbooks.FirstOrDefault(c => c.Id == id);
            if (songbookToRemove != null)
            {
                //Remove the dependent songs
                var songsToRemove = DataSource.Songs.Where(s => s.SongbookId == id);
                if (songsToRemove.Any())
                {
                    foreach (var song in songsToRemove)
                    {
                        //Remove the dependent creators
                        var creatorsToRemove = DataSource.Creators.Where(c => c.ParentId == song.Id);
                        foreach (var creator in creatorsToRemove)
                        {
                            DataSource.Creators.Remove(creator);
                        }

                        DataSource.Songs.Remove(song);
                    }
                }

                DataSource.Songbooks.Remove(songbookToRemove);
            }
        }

        public SongbookDto Get(Guid id)
        {
            _logger.LogInformation("Getting 'Songbook' record with id {@id}", id);
            return DataSource.Songbooks.FirstOrDefault(sb => sb.Id == id);
        }

        public IList<SongbookDto> Get(int pageNumber, int pageSize)
        {
            _logger.LogInformation("Getting paginated 'Songbook' records (Page Number: {@pageNumber}, Page Size: {@pageSize})", pageNumber, pageSize);
            return DataSource.Songbooks.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList();
        }

        public IList<SongbookDto> GetByCriteria(SongbookSearchCriteria criteria, int pageNumber, int pageSize)
        {
            if(!criteria.IsValid) 
            {
                _logger.LogWarning("Unable to retrieve 'Songbook' records by criteria. It is invalid. ({@criteria})", criteria);
                throw new InvalidSearchCriteriaException(criteria); 
            }

            _logger.LogInformation("Getting 'Songbook' records with criteria {@criteria} (pageNumber: {@pageNumber}, pageSize: {@pageSize})", criteria, pageNumber, pageSize);
            return DataSource.Songbooks.Where(sb => (criteria.Title != null && sb.Title.Contains(criteria.Title)) || (criteria?.Ids.Any(id => id == sb.Id) ?? false))
                                       .Skip(pageSize * (pageNumber - 1))
                                       .Take(pageSize)
                                       .ToList();
        }

        public void Insert(SongbookDto dto)
        {
            if (dto == null) { return; }

            _logger.LogInformation("Inserting 'Songbook' record {@dto}", dto);
            DataSource.Songbooks.Add(dto);
        }

        public void Update(SongbookDto dto)
        {
            if (dto == null) { return; }

            _logger.LogInformation("Updating 'Songbook' record {@dto}", dto);
            var existingSongbook = DataSource.Songbooks.FirstOrDefault(sb => sb.Id == dto.Id);
            if(existingSongbook == null)
            {
                return;
            }

            DataSource.Songbooks.Remove(existingSongbook);
            DataSource.Songbooks.Add(dto);
        }
    }
}
