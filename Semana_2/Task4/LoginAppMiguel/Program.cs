using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using LoginAppMiguel.Data;
using LoginAppMiguel.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
// For Entity Framework aand injection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("LoginConnection"),
        new MySqlServerVersion(new Version(8, 0, 21))
    ));
// For Identity
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 1;
    options.Password.RequiredUniqueChars = 1;

    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = false;
}).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
});
var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
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
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();