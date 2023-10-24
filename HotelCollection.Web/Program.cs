using System.IO;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using DinkToPdf;
using DinkToPdf.Contracts;
using HotelCollection;
using HotelCollection.Data;
using HotelCollection.Data.Entity;
using HotelCollection.Infrastructure.Interface;
using HotelCollection.Infrastructure.Models;
using HotelCollection.Infrastructure.Services;
using HotelCollection.Web.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddAppServices(builder.Configuration);

builder.Services.AddIdentityCore<ApplicationUser>()
    .AddRoles<Role>()
    .AddUserManager<UserManager<ApplicationUser>>()
    .AddRoleManager<RoleManager<Role>>()
    .AddSignInManager<SignInManager<ApplicationUser>>()
    .AddEntityFrameworkStores<HotelCollectionContext>();
          







var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
#region Cookie
app.UseCookiePolicy(new CookiePolicyOptions
{
   // HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always,
    MinimumSameSitePolicy = SameSiteMode.None
});
#endregion

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UpdateDatabase();
app.Seeder();
app.UseAuthentication();
//app.UseSession();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseCookiePolicy();
           
// CreateUserRoles(services).Wait();

app.Run();