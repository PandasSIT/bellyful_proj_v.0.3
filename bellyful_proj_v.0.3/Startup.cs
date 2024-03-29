﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using bellyful_proj_v._0._3.Data;
using bellyful_proj_v._0._3.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace bellyful_proj_v._0._3
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddDbContext<bellyful_v03Context>(options => { options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")); });//配置EF 使用SQL Server

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            var dataProtectionProviderType = typeof(DataProtectorTokenProvider<ApplicationUser>);//No IUserTwoFactorTokenProvider<TUser> 
            var phoneNumberProviderType = typeof(PhoneNumberTokenProvider<ApplicationUser>);     //named 'Default' is registered.
            var emailTokenProviderType = typeof(EmailTokenProvider<ApplicationUser>);            //  error
            services.AddIdentity<ApplicationUser,IdentityRole>(  //注入 Role Identity   ,把Default去掉
                    options =>
                    {
                        options.Password.RequireNonAlphanumeric = false;
                        options.Password.RequireLowercase = false;
                        options.Password.RequireUppercase = false;
                        options.Password.RequiredLength = 3;

                    })
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddTokenProvider(TokenOptions.DefaultProvider, dataProtectionProviderType)       //No IUserTwoFactorTokenProvider<TUser>
                .AddTokenProvider(TokenOptions.DefaultEmailProvider, emailTokenProviderType)      //named 'Default' is registered.
                .AddTokenProvider(TokenOptions.DefaultPhoneProvider, phoneNumberProviderType);    //  error

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

          //  app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
