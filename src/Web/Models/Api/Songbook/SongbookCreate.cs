using System.Collections.Generic;

namespace Hymnstagram.Web.Models.Api
{
    /// <summary>
    /// Client-facing object for creating new Songbooks.
    /// </summary>
    public class SongbookCreate
    {
        /// <summary>
        /// The title of the songbook.
        /// </summary>
        /// <example>Psalms, Hymns, and Spiritual Songs</example>        
        public string Title { get; set; }

        /// <summary>
        /// The publishing body for the songbook.
        /// </summary>        
        public string Publisher { get; set; }              

        /// <summary>
        /// The 10-digit ISBN for the title.
        /// </summary>        
        public string ISBN10 { get; set; }

        /// <summary>
        /// The 13-digit ISBN for the title.
        /// </summary>        
        public string ISBN13 { get; set; }

        /// <summary>
        /// A list of various creator who helped create the songbook. 
        /// Includes editor, technical editor, associate editor.
        /// </summary>        
        public IList<CreatorCreate> Creators { get; set; } = new List<CreatorCreate>();

        /// <summary>
        /// A list of songs encompassed within the songbook.
        /// </summary>
        public IList<SongCreate> Songs { get; set; } = new List<SongCreate>();
    }
}
