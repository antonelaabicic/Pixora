using Pixora.DAL.Models;

namespace Pixora.BL.Services.Plans
{
    public class PlanPolicyResolver
    {
        private readonly IEnumerable<IPlanPolicy> _policies;
        public PlanPolicyResolver(IEnumerable<IPlanPolicy> policies)
        {
            _policies = policies;
        }

        public IPlanPolicy Resolve(PlanType planType)
        {
            return _policies.FirstOrDefault(p => p.PlanType == planType)
                ?? throw new InvalidOperationException($"No policy registered for plan {planType}.");
        }
    }
}
