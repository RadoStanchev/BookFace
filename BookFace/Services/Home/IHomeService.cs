using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookFace.Models.Home;

namespace BookFace.Services.Home
{
    public interface IHomeService
    {
        HomePostSuggestionModel IndexModel(string userId);
    }
}
