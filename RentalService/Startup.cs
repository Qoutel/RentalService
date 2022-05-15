using RentalService.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Autofac;
using RentalService.Managers;
using RentalService.Interface;
using RentalService.Chat;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace RentalService
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
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<IdentityContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<IdentityContext>();

            services.AddSignalR();
            
            services.AddControllersWithViews();
        }
        public void ConfigureContainer(ContainerBuilder builder)
        {
            // Register your services
            builder.RegisterType<DbManager>().As<IDbManager>();
            builder.RegisterType<DbManagerADONET>().As<IDbManagerADONET>();
        }
        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                name: "chat",
                pattern: "{controller=Account}/{action=Chat}");

                endpoints.MapHub<ChatHub>("/chat");
            });

        }
    }
}