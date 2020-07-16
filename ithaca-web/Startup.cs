// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.TokenCacheProviders.InMemory;
using TodoListClient.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web.UI;
using Microsoft.IdentityModel.Logging;
using myConstants = TodoListClient.Infrastructure.Constants;
using System.Threading.Tasks;
using Microsoft.Identity.Web.Resource;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace TodoListClient
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
            services.AddDistributedMemoryCache();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
                // Handling SameSite cookie according to https://docs.microsoft.com/en-us/aspnet/core/security/samesite?view=aspnetcore-3.1
                options.HandleSameSiteCookieCompatibility();
            });

            services.AddOptions();
            //  This is the syntax for the v0.2 preview Microsoft.Identity Web.  If upgrading, then you would need to refactor the code below hooking into RedirectToIdentityProvider to fit this new structure.
            //services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
            //    .AddMicrosoftWebApp(Configuration, "AzureAdB2C")
            //    .AddMicrosoftWebAppCallsWebApi(Configuration, new string[] { Configuration["TodoList:TodoListScope"] }, configSectionName: "AzureAdB2C")
            //    .AddInMemoryTokenCaches();

            services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                .AddSignIn("AzureAdB2C", Configuration, options =>
                { 
                    Configuration.Bind("AzureAdB2C", options);
                    options.Events = new OpenIdConnectEvents
                    {
                        // handle the  redirection
                        OnRedirectToIdentityProvider = (context) =>
                        {
                            foreach (var item in context.Properties.Parameters)
                            {
                                if (context.ProtocolMessage.Parameters.ContainsKey(item.Key))
                                {
                                    context.ProtocolMessage.Parameters[item.Key] = item.Value.ToString();
                                }
                                else
                                {
                                    context.ProtocolMessage.Parameters.Add(item.Key, item.Value.ToString());
                                }
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

          

            // Add APIs - can be removed...
            services.AddWebAppCallsProtectedWebApi(Configuration, new string[] { Configuration["TodoList:TodoListScope"] }, configSectionName: "AzureAdB2C")
                   .AddInMemoryTokenCaches();
            services.AddTodoListService(Configuration);
            // end can be removed

            services.AddControllersWithViews(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            }).AddMicrosoftIdentityUI();

            services.AddRazorPages();

            //Configuring appsettings section AzureAdB2C, into IOptions
            services.AddOptions();
            services.Configure<OpenIdConnectOptions>(Configuration.GetSection("AzureAdB2C"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                
                    Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;
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
            app.UseCookiePolicy();

            app.UseRouting();

            
            app.UseAuthentication();
            
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}