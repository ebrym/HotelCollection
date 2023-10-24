// using System;
// using Autofac;
// using Autofac.Extensions.DependencyInjection;
// using AutoMapper;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Builder;
// using Microsoft.AspNetCore.Hosting;
// using Microsoft.AspNetCore.Http;
// using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using HotelCollection.Data;
// using System.IO;
// using HotelCollection.Infrastructure.LDAP;
// using HotelCollection.Data.Entity;
// using Novell.Directory.Ldap;
// using HotelCollection.Infrastructure.Models;
// using Microsoft.AspNetCore.Authentication.Cookies;
// using HotelCollection.Repository.AutofacModule;
// using DinkToPdf;
// using DinkToPdf.Contracts;
// using HotelCollection.Infrastructure.Services;
// using HotelCollection.Web.Helpers;
// using HotelCollection.Infrastructure.Interface;
// using HotelCollection.Web.Filters;
// using Microsoft.AspNetCore.CookiePolicy;
//
// namespace HotelCollection
// {
//     public class Startup
//     {
//         public IConfiguration Configuration { get; }
//         public IContainer ApplicationContainer { get; private set; }
//         public IHostingEnvironment HostingEnvironment { get; }
//         public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
//         {
//             Configuration = configuration;
//             HostingEnvironment = hostingEnvironment;
//         }
//
//
//         // This method gets called by the runtime. Use this method to add services to the container.
//         public IServiceProvider ConfigureServices(IServiceCollection services)
//         {
//             services.Configure<CookiePolicyOptions>(options =>
//             {
//                 // This lambda determines whether user consent for non-essential cookies is needed for a given request.
//                 options.CheckConsentNeeded = context => true;
//                 options.MinimumSameSitePolicy = SameSiteMode.None;
//             });
//            // services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
//            // services.AddMvc(options => options.EnableEndpointRouting = false);
//
//
//
//             services.AddDbContext<HotelCollectionContext>(options =>
//                 options.UseSqlServer(
//                     Configuration.GetConnectionString("DefaultConnection")));
//
//           
//             
//             services.AddIdentityCore<ApplicationUser>()
//                 .AddRoles<Role>()
//                 .AddUserManager<UserManager<ApplicationUser>>()
//                 .AddRoleManager<RoleManager<Role>>()
//                 .AddSignInManager<SignInManager<ApplicationUser>>()
//                 .AddEntityFrameworkStores<HotelCollectionContext>();
//           
//
//             // inrastructure service
//             services.AddScoped<IProjectService, ProjectService>();
//             services.AddTransient<IViewRenderService, ViewRenderService>();
//             services.AddSingleton<ISMTPService, SMTPService>();
//
//
//
//
//             services.AddAutoMapper(typeof(Startup).Assembly);
//             services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
//             //services.AddSingleton<ISMTPService, SMTPService>();
//             services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
//
//
//             services.AddMvc( options=>{
//                 options.EnableEndpointRouting = false;
//                 //options.Filters.Add(typeof(AuthenticateUser));
//                 options.Filters.Add(typeof(CustomExceptionFilterAttribute));
//                 options.Filters.Add(typeof(AuditFilter));
//
//             });
//
//             services.AddControllersWithViews();
//             
//             services.AddSession(options =>
//             {
//                 options.IdleTimeout = TimeSpan.FromMinutes(30);
//             });
//
//             // for PDF conversion
//             var wkHtmlToPdfPath = Path.Combine(HostingEnvironment.ContentRootPath, $"libwkhtmltox");
//
//             CustomAssemblyLoadContext ctext = new CustomAssemblyLoadContext();
//            // ctext.LoadUnmanagedLibrary(wkHtmlToPdfPath);
//             services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
//
//             services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//             .AddCookie();
//
//             var builder = new ContainerBuilder();
//             builder.Populate(services);
//
//             // builder.RegisterType<EmailSettings>().AsSelf().SingleInstance();
//             builder.RegisterModule<AutofacRepoModule>();
//
//             ApplicationContainer = builder.Build();
//             // Create the IServiceProvider based on the container.
//             return new AutofacServiceProvider(ApplicationContainer);
//
//
//
//         }
//
//         // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
//         public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime appLifetime, IServiceProvider services)
//         {
//             if (env.IsDevelopment())
//             {
//                 app.UseDeveloperExceptionPage();
//                // app.UseDatabaseErrorPage();
//                using (var serviceScope = app.ApplicationServices.CreateScope())
//                {
//                    var dbContext = serviceScope.ServiceProvider.GetRequiredService<HotelCollectionContext>();
//                    dbContext.Database.Migrate();
//                }
//             }
//             else
//             {
//                 app.UseExceptionHandler("/Home/Error");
//                 // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//                 app.UseHsts();
//             }
//             #region Cookie
//             app.UseCookiePolicy(new CookiePolicyOptions
//             {
//                 HttpOnly = HttpOnlyPolicy.Always,
//                 Secure = CookieSecurePolicy.Always,
//                 MinimumSameSitePolicy = SameSiteMode.None
//             });
//             #endregion
//
//             app.UseHttpsRedirection();
//             app.UseStaticFiles();
//
//             app.UseAuthentication();
//             app.UseSession();
//            
//             app.UseRouting();
//             app.UseMvc(routes =>
//             {
//                 routes.MapRoute(
//                     name: "default",
//                     template: "{controller=Home}/{action=Index}/{id?}");
//             
//             });
//             // app.MapControllerRoute(
//             //     name: "default",
//             //     pattern: "{controller=Home}/{action=Index}/{id?}");
//
//            app.UseCookiePolicy();
//            
//           // CreateUserRoles(services).Wait();
//             appLifetime.ApplicationStopped.Register(() => ApplicationContainer.Dispose());
//
//         }
//         
//         private async Task CreateUserRoles(IServiceProvider serviceProvider)
//         {
//             //var RoleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
//             var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
//
//             var user = new ApplicationUser() { FirstName = "Ibrahim",
//                 LastName = "Abdullahi",
//                 Email = "ibrodex@gmail.com", 
//                 UserName = "ibrodex" };
//             var result = await UserManager.CreateAsync(user, "Pa$$word123");
//
//             // var roleCheck = await RoleManager.RoleExistsAsync("Admin");
//             // if (!roleCheck)
//             // {
//             //     //create the roles and seed them to the database  
//             //    await RoleManager.CreateAsync(new Role(){Name = "Admin"});
//             // }
//             //  await UserManager.AddToRoleAsync(user, "Admin");
//         }
//        
//       
//     }
// }
