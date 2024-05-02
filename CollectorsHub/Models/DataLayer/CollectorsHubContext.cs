using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;

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
            CollectorsHubUnitOfWork data = new CollectorsHubUnitOfWork(serviceProvider.GetRequiredService<CollectorsHubContext>());

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
                    FirstName="Admin",
                    LastName="Admin",
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
                    FirstName="user",
                    LastName="A"
                };

                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, roleUser);
                    User fakeUser = data.Users.Get(new QueryOptions<User> {
                        Where = (fake => fake.UserName == username)
                    }) ;
                    Collection collection = new Collection()
                    {
                        Name= "Old Machines",
                        Tag= "Machine",
                        UserId=fakeUser.Id
                    };
                    data.Collections.Insert(collection);
                    data.Save();
                    collection = data.Collections.Get(new QueryOptions<Collection>
                    {
                        Where = (col => col.Name == "Old Machines"&&col.Tag=="Machine")
                    }) ;
                    Item item = new Item()
                    {
                        Name = "Typewriter",
                        Description= "An old typewriter",
                        image=ImageConverter.imageToByteArray("images/typeWriter.png"),
                        CollectionId=collection.CollectionId
                    };
                    data.Items.Insert(item);
                    data.Save();
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
                    FirstName = "user",
                    LastName = "B"
                };

                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, roleUser);
                    await userManager.AddToRoleAsync(user, roleUser);
                    User fakeUser = data.Users.Get(new QueryOptions<User>
                    {
                        Where = (fake => fake.UserName == username)
                    });
                    Collection collection = new Collection()
                    {
                        Name = "Fossils",
                        Tag = "Fossils",
                        UserId = fakeUser.Id
                    };
                    data.Collections.Insert(collection);
                    data.Save();
                    collection = data.Collections.Get(new QueryOptions<Collection>
                    {
                        Where = (col => col.Name == "Fossils" && col.Tag == "Fossils")
                    });
                    Item item = new Item()
                    {
                        Name = "Amber",
                        Description = "An old tree fossil",
                        image = ImageConverter.imageToByteArray("images/amber.png"),
                        CollectionId = collection.CollectionId
                    };
                    data.Items.Insert(item);
                    data.Save();
                }
            }

            username = "user3";
            password = "userC1";
            if (await userManager.FindByNameAsync(username) == null)
            {
                User user = new User
                {
                    UserName = username,
                    FirstName = "user",
                    LastName = "C"
                };

                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, roleUser);
                    await userManager.AddToRoleAsync(user, roleUser);
                    User fakeUser = data.Users.Get(new QueryOptions<User>
                    {
                        Where = (fake => fake.UserName == username)
                    });
                    Collection collection = new Collection()
                    {
                        Name = "Ancient History",
                        Tag = "Relic",
                        UserId = fakeUser.Id
                    };
                    data.Collections.Insert(collection);
                    data.Save();
                    collection = data.Collections.Get(new QueryOptions<Collection>
                    {
                        Where = (col => col.Name == "Ancient History" && col.Tag == "Relic")
                    });
                   
               
                    Item item = new Item()
                    {
                        Name = "Ancient Pot ",
                        Description = "A clay pot from 100bc.",
                        image = ImageConverter.imageToByteArray("images/oldPot.png"),
                        CollectionId = collection.CollectionId
                    };
                    data.Items.Insert(item);
                    data.Save();
                }
            }


        }
    }
}
