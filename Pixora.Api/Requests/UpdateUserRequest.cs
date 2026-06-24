using Pixora.DAL.Models;

namespace Pixora.Api.Requests
{
    public class UpdateUserRequest
    {
        public string? Email { get; set; }
        public PlanType? PlanType { get; set; }
    }
}
