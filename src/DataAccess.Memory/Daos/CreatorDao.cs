using System;
using Hymnstagram.Model.DataAccess;
using Microsoft.Extensions.Logging;
using Model;

namespace DataAccess.Memory.Daos
{
    public class CreatorDao : ICreatorDao 
    {
        private ILogger<CreatorDao> _logger;

        public CreatorDao(ILogger<CreatorDao> logger)
        {
            _logger = logger;
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public CreatorDto Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Insert(CreatorDto dto)
        {
            throw new NotImplementedException();
        }

        public void Update(CreatorDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
