using System.Collections.Generic;

namespace Hymnstagram.Web.Models.Api
{
    /// <summary>
    /// Client-facing object for creating new Songs for Songbooks.
    /// </summary>
    public class SongCreate
    {
        /// <summary>
        /// The song's position in the songbook.
        /// </summary>
        /// <example>127</example>        
        public int? SongNumber { get; set; }

        /// <summary>
        /// The tune to which the song is written.
        /// </summary>        
        public string Tune { get; set; }

        /// <summary>
        /// The name of the song.
        /// </summary>                
        public string Title { get; set; }

        /// <summary>
        /// The key in which the song is written
        /// </summary>
        /// <example>Bb</example>        
        public string Key { get; set; }

        /// <summary>
        /// The time signature in which the song is written
        /// </summary>
        /// <example>4/4</example>        
        public string TimeSignature { get; set; }

        /// <summary>
        /// The starting Solfege note relative to the key of the song
        /// </summary>
        /// <example>Mi</example>        
        public int? SolfaTypeId { get; set; }

        /// <summary>
        /// A list of various creator who helped create the song. 
        /// Includes composer, writer, and arranger type.
        /// </summary>
        public IList<CreatorCreate> Creators { get; set; } = new List<CreatorCreate>();
    }
}
