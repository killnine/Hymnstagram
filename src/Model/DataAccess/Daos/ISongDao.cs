using System;
using Model;

namespace Hymnstagram.Model.DataAccess.Daos
{
    public interface ISongDao
    {
        SongDto Get(Guid id);
        void Insert(SongDto dto);
        void Update(SongDto dto);
        void Delete(Guid id);
    }
}
