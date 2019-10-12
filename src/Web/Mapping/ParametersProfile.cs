using AutoMapper;
using Hymnstagram.Model.DataAccess.Criteria;
using Hymnstagram.Web.Helpers.Parameters;

namespace Hymnstagram.Web.Mapping
{
    /// <summary>
    /// Automapper profile for converting Parameter object types to their 
    /// client-facing equivalents and back.
    /// </summary>
    public class ParametersProfile : Profile
    {
        /// <summary>
        /// Profile constructor. 
        /// Do your mapping here for Parameter types.
        /// </summary>
        public ParametersProfile()
        {
            CreateMap<SongbookResourceParameters, SongbookSearchCriteria>();
            CreateMap<SongResourceParameters, SongSearchCriteria>();
        }
    }
}
