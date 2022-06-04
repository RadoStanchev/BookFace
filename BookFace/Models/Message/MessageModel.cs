using BookFace.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookFace.Models.Message
{
    public class MessageModel
    {
        public HomeOwnerModel Owner { get; set; }

        public string Content { get; set; }

        public TimeSpan DateDiff { get; set; }

        public string CreatorId { get; set; }
    }
}
