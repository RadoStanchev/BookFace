using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using BookFace.Data;
using System.IO;
using Microsoft.Data.SqlClient;

namespace BookFace.Infrastructure
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json")
           .Build();


            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            //builder.MultipleActiveResultSets = true;

            return new ApplicationDbContext(builder.Options);
        }
    }
}
