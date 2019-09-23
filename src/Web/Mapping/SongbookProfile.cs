using AutoMapper;
using Hymnstagram.Model.DataTransfer;
using Hymnstagram.Model.Domain;
using Hymnstagram.Web.Models.Api;

namespace Hymnstagram.Web.Mapping
{
    public class SongbookProfile : Profile
    {
        public SongbookProfile()
        {
            CreateMap<Songbook, SongbookResult>();                        
        }
    }
}
