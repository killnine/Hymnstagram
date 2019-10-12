using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        /// <summary>
        /// The publishing body for the songbook.
        /// </summary>
        [MaxLength(60)]
        public string Publisher { get; set; }

        /// <summary>
        /// The date which the songbook was most recently published.
        /// </summary>
        /// <remarks>Only the date component is referenced. Time is disregarded.</remarks>
        public DateTime PublicationDate { get; set; }

        /// <summary>
        /// The 10-digit ISBN for the title.
        /// </summary>
        [MaxLength(10, ErrorMessage = "The ISBN10 field must be exactly 10 characters")]
        [MinLength(10, ErrorMessage = "The ISBN10 field must be exactly 10 characters")]
        public string ISBN10 { get; set; }

        /// <summary>
        /// The 13-digit ISBN for the title.
        /// </summary>
        [MaxLength(13, ErrorMessage = "The ISBN13 field must be exactly 13 characters")]
        [MinLength(13, ErrorMessage = "The ISBN13 field must be exactly 13 characters")]
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
