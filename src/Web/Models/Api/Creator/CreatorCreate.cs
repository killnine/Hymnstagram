using System.ComponentModel.DataAnnotations;

namespace Hymnstagram.Web.Models.Api
{
    /// <summary>
    /// Client-facing object for creating new Creators for Songs and Songbooks.
    /// </summary>
    public class CreatorCreate
    {
        /// <summary>
        /// The first name of the creator.
        /// </summary>
        [Required]
        [MaxLength(15)]
        public string FirstName { get; set; }

        /// <summary>
        /// The surname of the creator.
        /// </summary>
        [Required]
        [MaxLength(25)]
        public string LastName { get; set; }

        /// <summary>
        /// The type of creator.
        /// </summary>
        /// <example>Editor, TechnicalEditor, AssociateEditor</example>
        [Required]
        public int CreativeTypeId { get; set; }
    }
}
