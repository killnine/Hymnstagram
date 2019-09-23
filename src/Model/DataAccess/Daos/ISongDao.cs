using System;
using System.Collections.Generic;
using Hymnstagram.Model.DataAccess.Criteria;
using Hymnstagram.Model.DataTransfer;

namespace Hymnstagram.Model.DataAccess
{
    public interface ISongDao
    {
        SongDto Get(Guid id);
        IList<SongDto> GetByCriteria(SongSearchCriteria criteria);
        IList<SongDto> GetByCriteria(SongSearchCriteria criteria, int pageNumber, int pageSize);
        void Insert(SongDto dto);
        void Update(SongDto dto);
        void Delete(Guid id);
    }
}
