
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;    // add this
using CollectorsHub.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMemoryCache();
builder.Services.AddSession();
builder.Services.AddControllersWithViews().AddNewtonsoftJson();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication();
builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddTransient<ICollectorsHubUnitOfWork, CollectorsHubUnitOfWork>();
builder.Services.AddTransient(typeof(IRepository<>), typeof(Repository<>));

var connectionString = builder.Configuration.GetConnectionString("CollectorsHub");
builder.Services.AddDbContext<CollectorsHubContext>(options =>
                    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<User, IdentityRole>(options => {
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
}).AddEntityFrameworkStores<CollectorsHubContext>()
  .AddDefaultTokenProviders();


var app = builder.Build();
app.UseDeveloperExceptionPage();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();   // add this
app.UseAuthorization();    // add this
app.UseSession();
#pragma warning disable ASP0014
app.UseEndpoints(endpoints =>
{
endpoints.MapAreaControllerRoute(
    name: "admin",
    areaName: "Admin",
    pattern: "Admin/{controller}/{action}"
    );
    endpoints.MapAreaControllerRoute(
    name: "admin",
    areaName: "Admin",
    pattern: "Admin/{controller}/{action}/{id}"
    );
endpoints.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
    );

    endpoints.MapControllerRoute(
    name:"ControllerToAction",
    pattern:"{controller}/{action}"
    );

    endpoints.MapControllerRoute(
    name: "filterUserName",
    pattern: "{controller=User}/{action=List}/filter/{filterUserName}/{filterCollectionTag}"
    );
    endpoints.MapControllerRoute(
    name: "filterByItemName",
    pattern: "{controller=Collection}/{action=List}/{id}/filter/{filter}"
    );
    endpoints.MapControllerRoute(
    name: "InvalidAddOrEdit",
    pattern: "{controller=Item}/{action=AddEdit}/{ItemId}/{CollectionId}"
    );
}
);
//This method creates the fake users, admin, and injects seed data into the database.
CollectorsHubContext.CreateAdminUserAndTestUsers(builder.Services.BuildServiceProvider()).Wait();
app.Run();
