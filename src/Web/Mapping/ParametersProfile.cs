using AutoMapper;
using Hymnstagram.Model.DataAccess.Criteria;
using Hymnstagram.Web.Helpers.Parameters;

namespace Hymnstagram.Web.Mapping
{
    public class ParametersProfile : Profile
    {
        public ParametersProfile()
        {
            CreateMap<SongbookResourceParameters, SongbookSearchCriteria>();
        }
    }
}
