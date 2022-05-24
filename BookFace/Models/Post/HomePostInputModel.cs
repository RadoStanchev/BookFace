using System.ComponentModel.DataAnnotations;
using static BookFace.Data.DataConstants.Post;

namespace BookFace.Models.Post
{
    public class HomePostInputModel
    {
        [Required]
        [StringLength(ContentMaxLength, MinimumLength = ContentMinLength)]
        public string Content { get; set; }

        [MaxLength(ImageUrlMaxLength)]
        public string Image { get; set; }
    }
}
