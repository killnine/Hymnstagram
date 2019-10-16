using AutoMapper;
using Hymnstagram.Model.DataTransfer;
using Hymnstagram.Model.Domain;
using Hymnstagram.Web.Models.Api;
using Hymnstagram.Web.Models.Api.Song;

namespace Hymnstagram.Web.Mapping
{
    /// <summary>
    /// Automapper profile for converting Song object types to their 
    /// client-facing equivalents and back.
    /// </summary>
    public class SongProfile : Profile
    {
        /// <summary>
        /// Profile constructor. 
        /// Do your mapping here for Song types.
        /// </summary>
        public SongProfile()
        {
            CreateMap<SongPatch, SongDto>();
            CreateMap<SongCreate, SongDto>();
            CreateMap<Song, SongResult>();
        }
    }
}
