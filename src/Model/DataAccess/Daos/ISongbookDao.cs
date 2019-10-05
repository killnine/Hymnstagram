using System;
using System.Collections.Generic;
using Hymnstagram.Model.DataAccess.Criteria;
using Hymnstagram.Model.DataTransfer;

namespace Hymnstagram.Model.DataAccess
{
    public interface ISongbookDao
    {
        SongbookDto Get(Guid id);
        IList<SongbookDto> Get(int pageNumber, int pageSize);
        IList<SongbookDto> GetByCriteria(SongbookSearchCriteria criteria);
        void Insert(SongbookDto dto);
        void Update(SongbookDto dto);
        void Delete(Guid id);
    }
}
