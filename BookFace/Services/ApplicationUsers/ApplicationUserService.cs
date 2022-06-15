using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BookFace.Data;
using BookFace.Data.Models;
using BookFace.Infrastructure.Extensions;
using BookFace.Models.User;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static BookFace.WebConstants;

namespace BookFace.Services.ApplicationUsers
{
    public class ApplicationUserService : IApplicationUserService
    {
        private readonly ApplicationDbContext data;

        private readonly UserManager<ApplicationUser> userManager;

        private readonly IConfigurationProvider mapper; 

        public ApplicationUserService(ApplicationDbContext data, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            this.data = data;
            this.userManager = userManager;
            this.mapper = mapper.ConfigurationProvider;
        }

        public string AdminId()
        {
            return data.ApplicationUsers.FirstOrDefaultAsync(au => bool.Parse(userManager.IsInRoleAsync(au, "Admin").ToString())).GetAwaiter().GetResult().Id;
        }

        public bool Edit(string userId, string username, string email, string firstName, string lastName)
        {
            var user = Task.Run( async () => await userManager.FindByIdAsync(userId)).GetAwaiter().GetResult();

            if(user == null)
            {
                return false;
            }

            user.UserName = username;
            user.Email = email;
            user.FirstName = firstName;
            user.LastName = lastName;

            Task.Run(async () => await userManager.UpdateAsync(user)).GetAwaiter().GetResult();

            return true;
        }

        public IEnumerable<string> GetProfileImagePaths()
        {
            return Directory
                .GetFiles(Directory.GetCurrentDirectory() + wwwrootPath + imagesPath + iconsPath, "*.*", SearchOption.AllDirectories)
                .Select(s => s.Replace(Directory.GetCurrentDirectory(), string.Empty))
                .Select(s => s.Replace(wwwrootPath, string.Empty))
                .Select(s => s.Replace("\\", "/"))
                .ToList();
        }

        public bool IsEmailUnique(string email)
        {
            return data.ApplicationUsers.All(au => au.Email != email);
        }

        public bool IsUsernameUnique(string username)
        {
            return data.ApplicationUsers.All(au => au.UserName != username);
        }

        public HomeOwnerModel Owner(string creatorId)
        {
            var user = data.ApplicationUsers.FirstOrDefault(x => x.Id == creatorId);
            return Owner(user);
        }

        public HomeOwnerModel Owner(ApplicationUser user)
        {
            return new HomeOwnerModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                ProfileImagePath = user.ProfileImagePath,
            };
        }

        public string OwnerOfEmail(string email)
        {
            var user = data.ApplicationUsers.FirstOrDefault(au => au.UserName == email);

            if (user == null)
            {
                return null;
            }

            return user.Id;
        }

        public string OwnerOfUsername(string username)
        {
            var user = data.ApplicationUsers.FirstOrDefault(au => au.UserName == username);

            if (user == null)
            {
                return null;
            }

            return user.Id;
        }

        public ProfileServiceModel Profile(string userId)
        {
            var user = data.ApplicationUsers.FirstOrDefault(au => au.Id == userId);

            return new ProfileServiceModel
            {
                Id = user.Id,
                Username = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                ProfileImagePath = user.ProfileImagePath
            };
        }

        public UserServiceModel User(string userId)
        {
            var user = data.ApplicationUsers.FirstOrDefault(au => au.Id == userId);

            return new UserServiceModel
            {
                Id = user.Id,
                Username = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ProfileImagePath = user.ProfileImagePath,
            };
        }

        public string UserName(string userId)
        {
            return data.ApplicationUsers.FirstOrDefault(au => au.Id == userId).UserName;
        }
    }
}
