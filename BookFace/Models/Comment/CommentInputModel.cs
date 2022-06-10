using System.ComponentModel.DataAnnotations;
using static BookFace.Data.DataConstants.Comment;

namespace BookFace.Models.Comment
{
    public class CommentInputModel
    {
        [Required]
        [StringLength(ContentMaxLength, MinimumLength = ContentMinLength)]
        public string Content { get; set; }

        [Required]
        public string PostId { get; set; }
    }
}
