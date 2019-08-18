using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using MySql.Data.MySqlClient;

using Olivia.Web.Models;
using Olivia.Web.Models.Identity;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Razor;
using Olivia.Web.Models.Validation;
using Olivia.Web.Models.Data;

namespace Olivia.Web
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
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            var builder = new MySqlConnectionStringBuilder()
            {
                Server = Configuration["Olivia:Host"],
                Port = UInt32.Parse(Configuration["Olivia:Port"]),
                Database = Configuration["Olivia:Database"],
                UserID = Configuration["Olivia:User"],
                Password = Configuration["Olivia:Password"],
            };

            services.AddDbContext<OliviaContext>(opt =>
                opt.UseMySQL(builder.ConnectionString));

            services.AddIdentityCore<User>(o =>
            {
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequireUppercase = false;
            })
                .AddDefaultTokenProviders()
                .AddSignInManager();

            services.AddTransient<IUserStore<User>, UserStore>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
                options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            }).AddCookie(IdentityConstants.ApplicationScheme);

            services.ConfigureApplicationCookie(opt =>
            {
                opt.ExpireTimeSpan = TimeSpan.FromDays(14);
                opt.SlidingExpiration = true;
            });

            services.Configure<SecurityStampValidatorOptions>(o =>
            {
                o.ValidationInterval = TimeSpan.FromMinutes(1);
            });

            services.AddLocalization(o =>
            {
                o.ResourcesPath = "Resources";
            });

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddViewLocalization(o =>
                {
                    o.ResourcesPath = "Resources";
                })
                .AddViewLocalization(LanguageViewLocationExpanderFormat.SubFolder)
                .AddDataAnnotationsLocalization(options => options.DataAnnotationLocalizerProvider = (t, f) => f.Create(typeof(SharedResources)));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, SignInManager<User> users)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                //var r = users.UserManager.CreateAsync(new User { Username = "Sheemin" }, "abcd1234");
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            var cultures = new[] {
                new CultureInfo("es-CL")
            };

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("es-CL"),
                SupportedCultures = cultures,
                SupportedUICultures = cultures
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
            });
        }
    }
}
