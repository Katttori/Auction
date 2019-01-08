using DAL.Entities;
using DAL.Identity.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using System.Linq;

namespace DAL
{
    public class AuctionContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Lot> Lots { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> UsersProfiles { get; set; }

        static AuctionContext()
        {
            Database.SetInitializer(new DbInitializer());
        }
        public AuctionContext(string connectionString) : base(connectionString) {}
                
    }

    public class DbInitializer : CreateDatabaseIfNotExists<AuctionContext>
    {
        protected override void Seed(AuctionContext context)
        {
            var roleStore = new RoleStore<ApplicationRole>(context);
            var roleManager = new RoleManager<ApplicationRole>(roleStore);
            var adminRole = new ApplicationRole { Name = "Admin" };
            var userRole = new ApplicationRole { Name = "User" };
            var moderatorRole = new ApplicationRole { Name = "Moderator" };

            roleManager.Create(adminRole);
            roleManager.Create(userRole);
            roleManager.Create(moderatorRole);

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);
            var adminUser = new ApplicationUser { UserName = "Admin" , Email = "admin@gmail.com"};
            var userUser = new ApplicationUser { UserName = "User", Email = "user@gmail.com" };
            var moderatorUser = new ApplicationUser { UserName = "Moderator", Email = "moderator@gmail.com" };

            userManager.Create(adminUser, "1234admin");
            userManager.AddToRole(adminUser.Id, "Admin");

            userManager.Create(userUser, "1234user");
            userManager.AddToRole(userUser.Id, "User");

            userManager.Create(moderatorUser, "1234moderator");
            userManager.AddToRole(moderatorUser.Id, "Moderator");

            context.UsersProfiles.Add(new User { Id = adminUser.Id, Name = "Admin" });
            context.UsersProfiles.Add(new User { Id = userUser.Id, Name = "User" });
            context.UsersProfiles.Add(new User { Id = moderatorUser.Id, Name = "Moderator" });
            
            context.Categories.Add(new Category { Id = 1, Name = "Other" });
            context.Categories.Add(new Category { Id = 2, Name = "Laptops" });
            context.Categories.Add(new Category { Id = 3, Name = "Monitors" });
            context.Categories.Add(new Category { Id = 4, Name = "CPUs" });
            context.Categories.Add(new Category { Id = 5, Name = "Video cards" });
        }
    }
    
}