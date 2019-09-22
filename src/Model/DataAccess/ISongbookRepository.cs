using System;
using System.Collections.Generic;
using Model;

namespace Hymnstagram.Model.DataAccess
{
    public interface ISongbookRepository
    {
        Songbook GetById(Guid id);
        IList<Songbook> GetSongbooks(int pageNumber = 1, int pageSize = 10);
        IList<Songbook> GetSongbooksByTitleWildcard(string partialTitle, int pageNumber = 1, int pageSize = 10);
        void Save(Songbook songbook);
    }
}
