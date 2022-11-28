using HouseRentingSystem.Data;
using HouseRentingSystem.Data.Entities;
using HouseRentingSystem.Infrastructure;
using HouseRentingSystem.Services.Agents;
using HouseRentingSystem.Services.Houses;
using HouseRentingSystem.Services.Statistics;
using HouseRentingSystem.Services.Users;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<HouseRentingDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<User>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
    })
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<HouseRentingDbContext>();

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();//Anti CSRF attacks
});
builder.Services.AddTransient<IHouseService, HouseService>();
builder.Services.AddTransient<IAgentService, AgentService>();
builder.Services.AddTransient<IStatisticsService, StatisticsService>();
builder.Services.AddTransient<IUserService, UserService>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error/500");
    app.UseStatusCodePagesWithRedirects("/Home/Error?statusCode={0}");
    app.UseHsts();
}

app.SeedAdmin();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "House Details",
        pattern: "/Houses/Details/{id}/{information}",
        defaults: new { Controller = "Houses", Action = "Details" }
        );

    app.MapDefaultControllerRoute();
    app.MapRazorPages();
});

app.Run();
