using System;
using System.Collections.Generic;
using System.Linq;
using Hymnstagram.Model.DataAccess;
using Hymnstagram.Model.DataAccess.Criteria;
using Hymnstagram.Model.DataTransfer;
using Microsoft.Extensions.Logging;

namespace DataAccess.Memory.Daos
{
    public class CreatorDao : ICreatorDao 
    {
        private readonly ILogger<CreatorDao> _logger;

        public CreatorDao(ILogger<CreatorDao> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Delete(Guid id)
        {
            _logger.LogInformation("Deleting 'Creator' record with id {@id}", id);
            var creatorToRemove = DataSource.Creators.FirstOrDefault(c => c.Id == id);
            if (creatorToRemove != null)
            {
                DataSource.Creators.Remove(creatorToRemove);
            }
        }

        public CreatorDto Get(Guid id)
        {
            _logger.LogInformation("Getting 'Creator' record with id {@id}", id);
            return DataSource.Creators.FirstOrDefault(c => c.Id == id);
        }

        public IList<CreatorDto> GetByCriteria(CreatorSearchCriteria criteria)
        {            
            if (!criteria.IsValid)
            {
                _logger.LogWarning("Unable to retrieve 'Creator' records by criteria. It is invalid. ({@criteria})", criteria);
                throw new InvalidSearchCriteriaException(criteria);
            }

            _logger.LogInformation("Getting 'Creator' records with criteria {@criteria}", criteria);
            return DataSource.Creators.Where(c => c.TypeId == (int)criteria.ParentType && c.ParentId == criteria.ParentId)
                .ToList();
        }

        public IList<CreatorDto> GetByCriteria(CreatorSearchCriteria criteria, int pageNumber, int pageSize)
        {
            if (!criteria.IsValid) 
            {
                _logger.LogWarning("Unable to retrieve 'Creator' records by criteria. It is invalid. ({@criteria}, Page Number: {@pageNumber}, Page Size: {@pageSize})", criteria, pageNumber, pageSize);
                throw new InvalidSearchCriteriaException(criteria); 
            }

            _logger.LogInformation("Getting 'Creator' records with criteria {@criteria} (Page Number: {@pageNumber}, Page Size: {@pageSize})", criteria, pageNumber, pageSize);
            return DataSource.Creators.Where(c => c.TypeId == (int)criteria.ParentType && c.ParentId == criteria.ParentId)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToList();
        }        

        public void Insert(CreatorDto dto)
        {
            if (dto == null) { return; }

            _logger.LogInformation("Inserting 'Creator' record {@dto}", dto);
            DataSource.Creators.Add(dto);
        }

        public void Update(CreatorDto dto)
        {
            if (dto == null) { return; }

            _logger.LogInformation("Updating 'Creator' record {@dto}", dto);
            var creatorToUpdate = DataSource.Creators.FirstOrDefault(c => c.Id == dto.Id);
            if(creatorToUpdate != null)
            {
                DataSource.Creators.Remove(creatorToUpdate);
                DataSource.Creators.Add(dto);
            }            
        }        
    }
}
