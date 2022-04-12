﻿using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static BookFace.Data.DataConstants.ApplicationUser;


namespace BookFace.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(FirstNameMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(LastNameMaxLength)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(ProfileImagePathMaxLength)]
        public string ProfileImagePath { get; set; }

        [Required]
        public IEnumerable<Friendship> Friendships { get; set; }

        public IEnumerable<Post> Posts { get; set; }

        public IEnumerable<Post> Likes { get; set; }
    }
}