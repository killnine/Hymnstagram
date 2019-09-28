using System;
using System.Collections.Generic;
using Hymnstagram.Model.DataAccess.Criteria;
using Hymnstagram.Model.Domain;

namespace Hymnstagram.Model.DataAccess
{
    public interface ISongbookRepository
    {
        Songbook GetById(Guid id);
        IList<Songbook> GetSongbookByCriteria(SongbookSearchCriteria criteria, int pageNumber = 1, int pageSize = 10);         
        IList<Songbook> GetSongbooks(int pageNumber = 1, int pageSize = 10);
        IList<Songbook> GetSongbooksByTitleWildcard(string partialTitle, int pageNumber = 1, int pageSize = 10);        
        void Save(Songbook songbook);
    }
}
