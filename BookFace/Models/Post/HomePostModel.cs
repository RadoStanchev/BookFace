using BookFace.Models.Comment;
using BookFace.Models.User;
using System;
using System.Collections.Generic;

namespace BookFace.Models.Post
{
    public class HomePostModel
    {
        public string Id { get; set; }

        public HomeOwnerModel Owner { get; set; }

        public string Content { get; set; }

        public string Image { get; set; }

        public string DateDiff { get; set; }

        public IEnumerable<HomePostCommentModel> Comments { get; set; }

        public bool IsLiked { get; set; }
    }
}