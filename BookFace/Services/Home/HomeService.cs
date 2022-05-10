using BookFace.Models.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookFace.Services.Home
{
    public class HomeService : IHomeService
    {
        public IndexPostSuggestionModel GetIndexModel(string userId)
        {
            return new IndexPostSuggestionModel() { };
        }
    }
}
