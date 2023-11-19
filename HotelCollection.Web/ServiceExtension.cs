using System;
using System.Data;
using HotelCollection.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using DinkToPdf;
using DinkToPdf.Contracts;
using HotelCollection.Data.Entity;
using HotelCollection.Infrastructure.Interface;
using HotelCollection.Infrastructure.Models;
using HotelCollection.Infrastructure.Services;
using HotelCollection.Repository.AccountRepo;
using HotelCollection.Repository.ApprovalRepo;
using HotelCollection.Repository.EmailLogRepo;
using HotelCollection.Repository.Interface;
using HotelCollection.Repository.Repo;
using HotelCollection.Repository.Repo;
using HotelCollection.Repository.ReportRepo;
using HotelCollection.Repository.Repos;
using HotelCollection.Web.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using AccountManager = HotelCollection.Repository.AccountRepo.AccountManager;

namespace HotelCollection;

public static class ServiceExtension
{
    public static IContainer ApplicationContainer { get; private set; }
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<HotelCollectionContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly("HotelCollection.Data")));
        services.AddScoped<DbContext, HotelCollectionContext>();
        return services;
    }
    public static void AddAppServices(this IServiceCollection services, IConfiguration configuration)
        {
           // inrastructure service
           services.AddScoped<DbContext, HotelCollectionContext>();
           services.AddScoped<IProjectService, ProjectService>();
           services.AddTransient<IViewRenderService, ViewRenderService>();
           services.AddScoped<IRequisition, RequisitionRepository>(); 
           services.AddScoped<IEmailSentLog, EmailSentLogRepository>(); 
           services.AddScoped<IHotelCategoryRepository, HotelCategoryRepository>(); 
           services.AddScoped<IReportRepo, ReportRepository>(); 
           services.AddScoped<IAuditLogRepository, AuditLogRepository>(); 
           services.AddScoped<ISMTPService, SMTPService>();
           services.AddScoped<IApprovalRepository, ApprovalRepository>();
           services.AddScoped<IApprovalConfigRepository, ApprovalConfigRepository>();
           services.AddScoped<IAccountManager, AccountManager>();
           services.AddScoped<IAgentRepository, AgentRepository>();
           services.AddScoped<IHotelRepository, HotelRepository>();
           services.AddScoped<ILocalGovernmentAreaRepository, LocalGovernmentAreaRepository>();
           services.AddScoped<IHotelCategoryFeeRepository, HotelCategoryFeeRepository>();
           services.AddScoped<IPaymentTypeRepository, PaymentTypeRepository>();
        services.AddScoped<IPaymentSetupRepository, PaymentSetupRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<IPaymentTransaction, PaymentTransactionRepository>();
        services.AddAutoMapper(typeof(Program));
           services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
           services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
           
            // for PDF conversion
           var wkHtmlToPdfPath = Path.Combine(System.IO.Directory.GetCurrentDirectory(), $"libwkhtmltox");

           CustomAssemblyLoadContext ctext = new CustomAssemblyLoadContext();
            // ctext.LoadUnmanagedLibrary(wkHtmlToPdfPath);
           services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

           services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
               .AddCookie();


            services.AddTransient<IDbConnection>(db =>
            {
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                var connection = new SqlConnection(connectionString);
                return connection;
            });

            // Create the IServiceProvider based on the container.
            // return new AutofacServiceProvider(ApplicationContainer);
        }
    public static void UpdateDatabase(this IApplicationBuilder app)
    {
        using (var serviceScope = app.ApplicationServices.CreateScope())
        {
            var dbContext = serviceScope.ServiceProvider.GetRequiredService<HotelCollectionContext>();
            dbContext.Database.Migrate();
        }

       
    }
    
  public static void Seeder(this IApplicationBuilder app)
  {
      using (var scope = app.ApplicationServices.CreateScope())
      {
          var services = scope.ServiceProvider;

          try
          {
              var UserManager = services.GetRequiredService<UserManager<ApplicationUser>>();

              var user = new ApplicationUser() { FirstName = "Ibrahim",
                  LastName = "Abdullahi",
                  Email = "ibrodex@gmail.com", 
                  UserName = "ibrodex" };
               UserManager.CreateAsync(user, "Pa$$word123").Wait();
            
          }
          catch (Exception ex)
          {
              throw new Exception($"{ex.Message} \n {ex.InnerException} \n {ex.StackTrace}");
          }
      }
  } 
      
   
}