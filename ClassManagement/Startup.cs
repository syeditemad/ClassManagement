using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassManagement_ModelLibrary.Class_Model;
using ClassManagement.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using DinkToPdf.Contracts;
using DinkToPdf;
using System.IO;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Twitter;

namespace ClassManagement
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var lockoutOptions = new LockoutOptions()
            {
                AllowedForNewUsers = true,
                DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2),
                MaxFailedAccessAttempts = 3

            };

            var ConnectionString = Configuration.GetConnectionString("DevConnection");
            services.AddDbContext<InfrastructureDBContext>(options => options.UseSqlServer(ConnectionString));
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IEmailService, EmailService>();

            // context.LoadUnmanagedLibrary(Path.Combine(Directory.GetCurrentDirectory(), "libwkhtmltox.dll"));
            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
            services.AddControllersWithViews();

            services.AddIdentity<IdentityUser, IdentityRole>(option =>
            {
                option.Password.RequiredLength = 8;
                option.Password.RequiredUniqueChars = 3;
                option.Password.RequireNonAlphanumeric = true;
                option.Password.RequireUppercase = true;
                option.Password.RequireLowercase = false;
                option.Password.RequireDigit = false;
                option.SignIn.RequireConfirmedEmail = true;

                option.Lockout = lockoutOptions;
            })
             .AddEntityFrameworkStores<InfrastructureDBContext>()
             .AddDefaultTokenProviders();
            services.AddSession(option =>
            {
                option.IdleTimeout = TimeSpan.FromMinutes(30);
            });
            services.AddAuthentication().AddGoogle(opt =>
            {

                var Google_Authenticate = Configuration.GetSection("Authentication:Google");
                opt.ClientId = Google_Authenticate["ClientId"];
                opt.ClientSecret = Google_Authenticate["Secretkey"];
                opt.SignInScheme = IdentityConstants.ExternalScheme;
            })
                .AddFacebook(opt =>
                {
                     opt.AppId = "750668742868387";
                    opt.AppSecret = "b5c777badffc7f9bf5be323c60fcba55";
                });
            //services.AddMvc(options =>
            //{
            //    var policy = new AuthorizationPolicyBuilder()
            //                .RequireAuthenticatedUser()
            //                .Build();
            //    options.Filters.Add(new AuthorizeFilter(policy));

            //}).AddXmlSerializerFormatters();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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
            app.UseSession();   

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            
        }
    }

  
}
