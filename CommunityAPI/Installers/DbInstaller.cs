using CommunityAPI.Data;
using CommunityAPI.Domain;
using CommunityAPI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CommunityAPI.Installers
{
    public class DbInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ModelContext>(options =>
                   options.UseSqlite(
                       configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<User>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ModelContext>();

            services.AddScoped<ITechnologyService, TechnologyService>();
            services.AddScoped<ITechnologyScoreService, TechnologyScoreService>();
            services.AddScoped<IUriService, UriService>();
            services.AddScoped<IUserService, UserService>();
        }
    }
}
