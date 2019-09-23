using System;
using System.Collections.Generic;
using System.Linq;
using Hymnstagram.Model.DataAccess;
using Hymnstagram.Model.DataAccess.Criteria;
using Hymnstagram.Model.DataTransfer;
using Microsoft.Extensions.Logging;

namespace DataAccess.Memory.Daos
{
    public class SongDao : ISongDao
    {
        private ILogger<SongDao> _logger;

        public SongDao(ILogger<SongDao> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public void Delete(Guid id)
        {
            _logger.LogInformation("Deleting 'Song' record with id {@id}", id);
            var songToRemove = DataSource.Songs.FirstOrDefault(s => s.Id == id);
            if (songToRemove != null)
            {
                DataSource.Songs.Remove(songToRemove);
            }
        }

        public SongDto Get(Guid id)
        {
            _logger.LogInformation("Getting 'Song' record with id {@id}", id);
            return DataSource.Songs.FirstOrDefault(s => s.Id == id);
        }

        public IList<SongDto> GetByCriteria(SongSearchCriteria criteria)
        {
            if (!criteria.IsValid)
            {
                _logger.LogWarning("Unable to retrieve 'Song' records by criteria. It is invalid. ({@criteria})", criteria);
                throw new InvalidSearchCriteriaException(criteria);
            }

            _logger.LogInformation("Getting 'Song' records with criteria {@criteria}", criteria);
            return DataSource.Songs.Where(sb => sb.SongbookId == criteria.SongbookId || (criteria?.SongbookIds.Any(ids => ids == sb.SongbookId) ?? false))
                                       .ToList();
        }

        public IList<SongDto> GetByCriteria(SongSearchCriteria criteria, int pageNumber, int pageSize)
        {
            if (!criteria.IsValid)
            {
                _logger.LogWarning("Unable to retrieve 'Song' records by criteria. It is invalid. ({@criteria}, Page Number: {@pageNumber}, Page Size: {@pageSize})", criteria, pageNumber, pageSize);
                throw new InvalidSearchCriteriaException(criteria);
            }

            _logger.LogInformation("Getting 'Song' records with criteria {@criteria}, Page Number: {@pageNumber}, Page Size: {@pageSize})", criteria, pageNumber, pageSize);
            return DataSource.Songs.Where(sb => sb.SongbookId == criteria.SongbookId || (criteria?.SongbookIds.Any(ids => ids == sb.SongbookId) ?? false))
                                   .Skip(pageSize * (pageNumber - 1))
                                   .Take(pageSize)
                                   .ToList();
        }

        public void Insert(SongDto dto)
        {
            if(dto == null) { return; }

            _logger.LogInformation("Inserting 'Song' record {@dto}", dto);
            DataSource.Songs.Add(dto);
        }

        public void Update(SongDto dto)
        {
            if (dto == null) { return; }

            _logger.LogInformation("Updating 'Song' record {@dto}", dto);
            var songToUpdate = DataSource.Songs.FirstOrDefault(s => s.Id == dto.Id);
            if(songToUpdate != null)
            {
                DataSource.Songs.Remove(songToUpdate);
                DataSource.Songs.Add(dto);
            }
        }
    }
}
