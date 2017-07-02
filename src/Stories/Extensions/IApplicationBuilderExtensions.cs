using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Stories.Data.DbContexts;
using Stories.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Stories.Extensions
{
    public static class IApplicationBuilderExtensions
    {
        public static void SeedData(this IApplicationBuilder app)
        {
            var context = app.ApplicationServices.GetService<StoriesDbContext>();
 
            if (!context.Roles.Any())
            {
                var roles = new List<Role>
                        {
                            new Role { Name = "Admin" },
                            new Role { Name = "Moderator" },
                            new Role { Name = "User" }
                        };

                context.Roles.AddRange(roles.ToArray());
                context.SaveChanges();
            }
        }
    }
}