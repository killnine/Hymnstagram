using System;
using System.Collections.Generic;
using Hymnstagram.Model.DataAccess.Criteria;
using Model;

namespace Hymnstagram.Model.DataAccess.Daos
{
    public interface ISongbookDao
    {
        SongbookDto Get(Guid id);
        IList<SongbookDto> Get(int pageNumber, int pageSize);
        IList<SongbookDto> GetByCriteria(SongbookSearchCriteria criteria, int pageNumber, int pageSize);
        void Insert(SongbookDto dto);
        void Update(SongbookDto dto);
        void Delete(Guid id);
    }
}
