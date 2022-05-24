using BookFace.Models.User;
using System;

namespace BookFace.Models.Comment
{
    public class HomePostCommentModel
    {
        public HomeOwnerModel Owner { get; set; }

        public string Content { get; set; }

        public DateTime DateDiff { get; set; }
    }
}