using System;
using System.Collections.Generic;

namespace BookFace.Models.Home.Post
{
    public class IndexPostModel
    {
        public IndexPostOwnerModel Owner { get; set; }

        public string Content { get; set; }

        public string Image { get; set; }

        public DateTime DateDiff { get; set; }

        public IEnumerable<IndexPostCommentModel> Comment { get; set; }
    }
}