using Pixora.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pixora.BL.DTOs
{
    public class UpdateUserDto
    {
        public string UserId { get; set; } = null!;
        public string? Email { get; set; }
        public PlanType? PlanType { get; set; }
    }
}
