using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EnhancedEbookWebApp.Data;
using EnhancedEbookWebApp.Filters;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;

namespace EnhancedEbookWebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            #region snippet_AddMvc
            services.AddMvc()
                .AddRazorPagesOptions(options =>
                    {
                        options.Conventions
                            .AddPageApplicationModelConvention("/StreamedSingleFileUploadDb",
                                model =>
                                {
                                    model.Filters.Add(
                                        new GenerateAntiforgeryTokenCookieAttribute());
                                    model.Filters.Add(
                                        new DisableFormValueModelBindingAttribute());
                                });
                        options.Conventions
                            .AddPageApplicationModelConvention("/StreamedSingleFileUploadPhysical",
                                model =>
                                {
                                    model.Filters.Add(
                                        new GenerateAntiforgeryTokenCookieAttribute());
                                    model.Filters.Add(
                                        new DisableFormValueModelBindingAttribute());
                                });
                    })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            #endregion

           // Use SQL Database if in Azure, otherwise, use SQLite
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
                services.AddDbContext<AppDbContext>(options =>
                       options.UseSqlServer(Configuration.GetConnectionString("EnhancedEbookDbConnection")));
            else
                services.AddDbContext<AppDbContext>(options =>
                        options.UseSqlite("Data Source=localdatabase.db"));

            // Automatically perform database migration
            services.BuildServiceProvider().GetService<AppDbContext>().Database.Migrate();

            // Add identity services to app for login/authentication/authorization managemement through entity framework core
            services.AddIdentity<IdentityUser, IdentityRole>()
                    .AddEntityFrameworkStores<AppDbContext>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            //middleware for authenticating users
            app.UseAuthentication();
            app.UseMvcWithDefaultRoute();
        }
    }
}
