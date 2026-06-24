using Pixora.DAL.Models;

namespace Pixora.Api.Requests
{
    public class ChangePlanRequest
    {
        public PlanType NewPlanType { get; set; }
    }
}
