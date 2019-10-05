using System;
using System.Collections.Generic;
using Hymnstagram.Model.DataAccess.Criteria;
using Hymnstagram.Model.Domain;

namespace Hymnstagram.Model.DataAccess
{
    public interface ISongbookRepository
    {
        Songbook GetById(Guid id);
        PagedList<Songbook> GetSongbookByCriteria(SongbookSearchCriteria criteria);                       
        void Save(Songbook songbook);
    }
}
