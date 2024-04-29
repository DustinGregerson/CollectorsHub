using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace CollectorsHub.Models
{
    public class CollectorsHubContext : IdentityDbContext<User>
    {
        public DbSet<Item> items { get; set; }
        public DbSet<Collection> collections { get; set; }

        public CollectorsHubContext(DbContextOptions<CollectorsHubContext> options)
        : base(options)
        {}
        //Defining the navigation properties of the models this is done automatically by ef core with the right naming
        //conventions, however it is best practice to define them.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasKey(User =>User.Id);

            modelBuilder.Entity<Collection>()
            .HasKey(col => col.CollectionId);

            modelBuilder.Entity<Collection>()
            .HasOne(col => col.User)
            .WithMany(user => user.Collection)
            .HasForeignKey(col => new {col.UserId});

            modelBuilder.Entity<Item>().HasKey(itm => itm.itemId);
            modelBuilder.Entity<Item>()
            .HasOne(itm => itm.Collection)
            .WithMany(col => col.Items)
            .HasForeignKey(col => new {col.CollectionId});
        }



        public static async Task CreateAdminUserAndTestUsers(IServiceProvider serviceProvider)
        {
            UserManager<User> userManager =
                serviceProvider.GetRequiredService<UserManager<User>>();
            RoleManager<IdentityRole> roleManager =
                serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string username = "admin";
            string password = "Sesame";
            string roleAdmin = "Admin";
            string roleUser = "user";
            //if role doesn't exist, create it
            if (await roleManager.FindByNameAsync(roleAdmin) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(roleAdmin));
                await roleManager.CreateAsync(new IdentityRole(roleUser));
            }

            //if the username doesn't exist, create it and add the role

            if (await userManager.FindByNameAsync(username) == null)
            {
                User user = new User
                {
                    UserName = username,
                };

                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, roleAdmin);
                }
            }

            username = "user1";
            password = "userA1";
            if (await userManager.FindByNameAsync(username) == null)
            {

                User user = new User
                {
                    UserName = username,
                };

                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, roleUser);
                }
                else
                {
                    Console.WriteLine("<______________________________________________________________________");
                    Console.WriteLine(result);
                    Console.WriteLine("<______________________________________________________________________");
                }
            }

            username = "user2";
            password = "userB1";
            if (await userManager.FindByNameAsync(username) == null)
            {
                User user = new User
                {
                    UserName = username,
                };

                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, roleUser);
                }
            }

            username = "user3";
            password = "userC1";
            if (await userManager.FindByNameAsync(username) == null)
            {
                User user = new User
                {
                    UserName = username,
                };

                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, roleUser);
                }
            }


        }
    }
}
