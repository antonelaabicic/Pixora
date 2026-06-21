using Microsoft.Extensions.DependencyInjection;

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

            return services;
        }
    }
}
