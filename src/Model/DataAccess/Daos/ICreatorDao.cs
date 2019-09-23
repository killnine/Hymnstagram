using System;
using System.Collections.Generic;
using Hymnstagram.Model.DataAccess.Criteria;
using Hymnstagram.Model.DataTransfer;

namespace Hymnstagram.Model.DataAccess
{
    public interface ICreatorDao
    {
        CreatorDto Get(Guid id);
        IList<CreatorDto> GetByCriteria(CreatorSearchCriteria criteria, int pageNumber, int pageSize);
        IList<CreatorDto> GetByCriteria(CreatorSearchCriteria criteria);
        void Insert(CreatorDto dto);
        void Update(CreatorDto dto);
        void Delete(Guid id);
    }
}
