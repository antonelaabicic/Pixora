using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pixora.Api.Requests;
using Pixora.BL.DTOs;
using Pixora.BL.Services.Plans;
using Pixora.BL.Services.Users;
using System.Security.Claims;

namespace Pixora.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IPlanService _planService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IPlanService planService, IMapper mapper)
        {
            _userService = userService;
            _planService = planService;
            _mapper = mapper;
        }

        [HttpGet("me")]
        public IActionResult GetMe()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized();
            }

            var user = _userService.GetById(userId);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            return Ok(user);
        }

        [HttpGet("plan")]
        public IActionResult GetPlan()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized();
            }

            var plan = _planService.GetUserPlan(userId);

            return Ok(plan);
        }

        [HttpPost("plan")]
        public IActionResult ChangePlan(ChangePlanRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized();
            }

            var dto = new ChangePlanDto
            {
                UserId = userId,
                NewPlanType = request.NewPlanType
            };

            try
            {
                _planService.RequestPlanChange(dto);
                return Ok("Plan change requested. It will become active tomorrow.");
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
    }
}