using Internship.Data;
using Internship.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var myConnectionString = builder.Configuration.GetConnectionString("MyDBConnection");
builder.Services.AddDbContext<hseInternshipContext>(options =>
    options.UseSqlServer(myConnectionString));

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.MapControllerRoute(
    name: "documents",
    pattern: "Documents/Submit",
    defaults: new { controller = "Documents", action = "Submit" }
);
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "editApplication",
        pattern: "Applications/Edit/{id}",
        defaults: new { controller = "Applications", action = "Edit" });
});
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "DeleteApplication",
        pattern: "Home/DeleteApplication",
        defaults: new { controller = "Home", action = "DeleteApplication" });
}
);

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllerRoute(
            name: "Distribution",
            pattern: "/distribution",
            defaults: new { controller = "Home", action = "Distribution" });
    });
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllerRoute(
            name: "PerformDistribution",
            pattern: "/distribution",
            defaults: new { controller = "Home", action = "PerformDistribution" });
    });



    app.Run();
