using BookFace.Models.Home.Comment;
using BookFace.Models.Home.User;
using System;
using System.Collections.Generic;

namespace BookFace.Models.Home.Post
{
    public class IndexPostModel
    {
        public string Id { get; set; }

        public IndexOwnerModel Owner { get; set; }

        public string Content { get; set; }

        public string Image { get; set; }

        public DateTime DateDiff { get; set; }

        public IEnumerable<IndexPostCommentModel> Comments { get; set; }
    }
}