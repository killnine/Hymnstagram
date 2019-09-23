using AutoMapper;
using Hymnstagram.Model.Domain;
using Hymnstagram.Web.Models.Api;

namespace Hymnstagram.Web.Mapping
{
    public class SongProfile : Profile
    {
        public SongProfile()
        {
            CreateMap<Song, SongResult>();
        }
    }
}
