using AutoMapper;
using Hymnstagram.Model.Domain;
using Hymnstagram.Web.Models.Api;

namespace Hymnstagram.Web.Mapping
{
    public class CreatorProfile : Profile
    {
        public CreatorProfile()
        {
            CreateMap<Creator, CreatorResult>();
        }
    }
}
