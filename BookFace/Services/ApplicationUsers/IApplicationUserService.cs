using BookFace.Data.Models;
using BookFace.Models.User;
using System.Collections.Generic;

namespace BookFace.Services.ApplicationUsers
{
    public interface IApplicationUserService
    {
        IEnumerable<string> GetProfileImagePaths();

        bool IsUsernameUnique(string username);

        bool IsEmailUnique(string email);

        string OwnerOfUsername(string username);

        string OwnerOfEmail(string email);

        string AdminId();

        string UserName(string userId);

        UserServiceModel User(string userId);

        ProfileServiceModel Profile(string userId);

        bool Edit(string userId, string username, string email, string firstName, string lastName);

        HomeOwnerModel Owner(string creatorId);

        HomeOwnerModel Owner(ApplicationUser user);
    }
}
