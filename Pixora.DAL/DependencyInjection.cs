using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pixora.DAL.Config;
using Pixora.DAL.Models;
using Pixora.DAL.Repositories.Impl;
using Pixora.DAL.Repositories.Interfaces;
namespace Pixora.DAL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDalServices(this IServiceCollection services)
        {
            services.AddDbContext<PixoraContext>(options => options.UseNpgsql(DatabaseConfig.ConnectionString));

            services.AddScoped<IRepositoryProvider, RepositoryProvider>();

            services.AddScoped<IPhotoRepository, PhotoRepository>();
            services.AddScoped<IHashtagRepository, HashtagRepository>();
            services.AddScoped<IPhotoHashtagRepository, PhotoHashtagRepository>();
            services.AddScoped<IUserActionLogRepository, UserActionLogRepository>();
            services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();

            return services;
        }
    }
}
