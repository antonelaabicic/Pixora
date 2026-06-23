using Pixora.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixora.BL.DTOs
{
    public class ChangePlanDto
    {
        public string UserId { get; set; } = null!;
        public PlanType NewPlanType { get; set; }
    }
}
