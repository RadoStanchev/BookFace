using BookFace.Data;
using BookFace.Data.Models;
using BookFace.Hubs;
using BookFace.Infrastructure.Extensions;
using BookFace.Services.ApplicationUsers;
using BookFace.Services.Comment;
using BookFace.Services.Friend;
using BookFace.Services.Friendship;
using BookFace.Services.Home;
using BookFace.Services.Post;
using BookFace.Services.System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using static BookFace.Data.DataConstants.ApplicationUser;

namespace BookFace
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<ApplicationUser>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = PasswordMinLength;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

            services.ConfigureApplicationCookie(options =>
            {
                options.LogoutPath = $"/User/Logout";
            });

            services.AddAutoMapper(typeof(Startup));
            services.AddRazorPages();
            services.AddMemoryCache();
            services.AddSignalR();

            services.AddTransient<IApplicationUserService, ApplicationUserService>();
            services.AddTransient<ICommentService, CommentService>();
            services.AddTransient<IValidator, ClassValidator>();
            services.AddTransient<IPostService, PostService>();
            services.AddTransient<IFriendService, FriendService>();
            services.AddTransient<IFriendshipService, FriendshipService>();
            services.AddTransient<IHomeService, HomeService>();

            services.AddControllersWithViews(options =>
            {
                options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapDefaultAreaRoute();
                endpoints.MapRazorPages();
                endpoints.MapHub<ChatHub>("/chatHub");
                endpoints.MapHub<PostHub>("/postHub");
                endpoints.MapHub<FriendshipHub>("/friendshipHub");
            });
        }
    }
}
