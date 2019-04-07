using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuctionApp.Data;
using AuctionApp.Entities;
using AuctionApp.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuctionApp
{
    public class Startup
    {
        private IConfiguration _configuration;

        public Startup(IConfiguration conguration)
        {
            _configuration = conguration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>(cfg => cfg.User.RequireUniqueEmail = true).AddEntityFrameworkStores<AuctionDbContext>();
            services.AddDbContext<AuctionDbContext>(options => options.UseSqlServer(_configuration.GetConnectionString("AuctionDB")));
            services.AddScoped<UnitOfWork>();
            services.AddTransient<Seeder>();
            services.AddMvc();
            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, Seeder seeder)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                seeder.Seed().Wait();
            }

            app.UseStaticFiles();
            app.UseNodeModules(env.ContentRootPath);
            app.UseAuthentication();
            app.UseSignalR(routes => routes.MapHub<AuctionHub>("/auctionhub"));
            app.UseMvc(ConfigureRouting);
        }

        private void ConfigureRouting(IRouteBuilder obj)
        {
            obj.MapRoute("Default","{controller=Home}/{action=Index}");
        }
    }
}
