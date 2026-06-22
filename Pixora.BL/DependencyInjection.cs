using Microsoft.Extensions.DependencyInjection;
using Pixora.BL.Services.Auth;
using Pixora.BL.Services.ImageProcessing;
using Pixora.BL.Services.Plans;

namespace Pixora.BL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBlServices(this IServiceCollection services)
        {
            services.AddScoped<IPlanPolicy, FreePlanPolicy>();
            services.AddScoped<IPlanPolicy, ProPlanPolicy>();
            services.AddScoped<IPlanPolicy, GoldPlanPolicy>();

            services.AddScoped<PlanPolicyResolver>();

            services.AddScoped<IImageProcessingPipelineFactory, ImageProcessingPipelineFactory>();
            services.AddScoped<IImageProcessor, ImageProcessor>();

            services.AddScoped<IAuthFacade, AuthFacade>();

            return services;
        }
    }
}
