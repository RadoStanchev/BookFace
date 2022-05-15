using BookFace.Models.Home.User;
using System;

namespace BookFace.Models.Home.Comment
{
    public class IndexPostCommentModel
    {
        public IndexOwnerModel Owner { get; set; }

        public string Content { get; set; }

        public DateTime DateDiff { get; set; }
    }
}