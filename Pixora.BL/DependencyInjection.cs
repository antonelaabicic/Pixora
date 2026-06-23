using Microsoft.Extensions.DependencyInjection;
using Pixora.BL.Mapping;
using Pixora.BL.Services.Auth;
using Pixora.BL.Services.Hashtags;
using Pixora.BL.Services.ImageProcessing;
using Pixora.BL.Services.Logs;
using Pixora.BL.Services.Photos;
using Pixora.BL.Services.Plans;
using Pixora.BL.Services.Storage;

namespace Pixora.BL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBlServices(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            services.AddScoped<IPlanPolicy, FreePlanPolicy>();
            services.AddScoped<IPlanPolicy, ProPlanPolicy>();
            services.AddScoped<IPlanPolicy, GoldPlanPolicy>();
            services.AddScoped<PlanPolicyResolver>();

            services.AddScoped<IImageProcessingPipelineFactory, ImageProcessingPipelineFactory>();
            services.AddScoped<IImageProcessor, ImageProcessor>();

            services.AddScoped<SupabaseStorageClient>();
            services.AddScoped<IImageStorageService, SupabaseStorageAdapter>();

            services.AddScoped<IAuthFacade, AuthFacade>();
            services.AddScoped<IHashtagService, HashtagService>();
            services.AddScoped<IUserActionLogService, UserActionLogService>();
            services.AddScoped<IPhotoService, PhotoService>();

            services.AddScoped<IPlanService, PlanService>();
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
