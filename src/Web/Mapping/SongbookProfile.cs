using AutoMapper;
using Hymnstagram.Model.DataTransfer;
using Hymnstagram.Model.Domain;
using Hymnstagram.Web.Models.Api;

namespace Hymnstagram.Web.Mapping
{
    /// <summary>
    /// Automapper profile for converting Songbook object types to their 
    /// client-facing equivalents and back.
    /// </summary>
    public class SongbookProfile : Profile
    {
        /// <summary>
        /// Profile constructor. 
        /// Do your mapping here for Songbook types.
        /// </summary>
        public SongbookProfile()
        {
            CreateMap<SongbookCreate, SongbookDto>();
            CreateMap<Songbook, SongbookResult>();                        
        }
    }
}
