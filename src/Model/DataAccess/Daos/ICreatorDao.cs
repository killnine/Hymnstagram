using System;
using Model;

namespace Hymnstagram.Model.DataAccess
{
    public interface ICreatorDao
    {
        CreatorDto Get(Guid id);
        void Insert(CreatorDto dto);
        void Update(CreatorDto dto);
        void Delete(Guid id);
    }
}
