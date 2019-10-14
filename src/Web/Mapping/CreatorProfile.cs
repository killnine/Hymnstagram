using AutoMapper;
using Hymnstagram.Model.DataTransfer;
using Hymnstagram.Model.Domain;
using Hymnstagram.Web.Models.Api;

namespace Hymnstagram.Web.Mapping
{
    /// <summary>
    /// Automapper profile for converting Creator object types to their 
    /// client-facing equivalents and back.
    /// </summary>
    public class CreatorProfile : Profile
    {
        /// <summary>
        /// Profile constructor. 
        /// Do your mapping here for Creator types.
        /// </summary>
        public CreatorProfile()
        {
            CreateMap<CreatorCreate, CreatorDto>();
            CreateMap<Creator, CreatorResult>();
        }
    }
}
