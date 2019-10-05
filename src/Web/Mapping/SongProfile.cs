using AutoMapper;
using Hymnstagram.Model.DataTransfer;
using Hymnstagram.Model.Domain;
using Hymnstagram.Web.Models.Api;

namespace Hymnstagram.Web.Mapping
{
    public class SongProfile : Profile
    {
        public SongProfile()
        {
            CreateMap<SongCreate, SongDto>();
            CreateMap<Song, SongResult>();
        }
    }
}
