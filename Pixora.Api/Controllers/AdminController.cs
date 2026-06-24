using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pixora.Api.Requests;
using Pixora.BL.DTOs;
using Pixora.BL.Services.Logs;
using Pixora.BL.Services.Photos;
using Pixora.BL.Services.Plans;
using Pixora.BL.Services.Users;
using System.Security.Claims;

namespace Pixora.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserActionLogService _logService;
        private readonly IPhotoService _photoService;
        private readonly IPlanService _planService;

        public AdminController(IUserService userService, IUserActionLogService logService, IPhotoService photoService,
            IPlanService planService)
        {
            _userService = userService;
            _logService = logService;
            _photoService = photoService;
            _planService = planService;
        }

        [HttpGet("users")]
        public IActionResult GetUsers()
        {
            return Ok(_userService.GetAll());
        }

        [HttpGet("users/{id}")]
        public IActionResult GetUser(string id)
        {
            var user = _userService.GetById(id);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            return Ok(user);
        }

        [HttpPut("users/{id}")]
        public IActionResult UpdateUser(string id, UpdateUserRequest request)
        {
            var dto = new UpdateUserDto
            {
                UserId = id,
                Email = request.Email,
                PlanType = request.PlanType
            };

            _userService.UpdateUser(dto);

            return Ok($"User {dto.Email} updated.");
        }

        [HttpGet("logs")]
        public IActionResult GetLogs()
        {
            return Ok(_logService.GetAll());
        }

        [HttpGet("logs/user/{userId}")]
        public IActionResult GetUserLogs(string userId)
        {
            return Ok(_logService.GetByUserId(userId));
        }

        [HttpGet("logs/latest")]
        public IActionResult GetLatestLogs([FromQuery] int count = 20)
        {
            return Ok(_logService.GetLatest(count));
        }

        [HttpDelete("photos/{id}")]
        public async Task<IActionResult> DeletePhoto(int id)
        {
            var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (adminId == null)
            {
                return Unauthorized();
            }

            await _photoService.DeleteAsync(id, adminId, isAdmin: true);

            return Ok("Photo deleted by admin.");
        }

        [HttpPut("users/{id}/plan")]
        public IActionResult ChangeUserPlan(string id, ChangePlanRequest request)
        {
            _planService.AdminChangeUserPlan(id, request.NewPlanType);

            return Ok("User plan changed by admin.");
        }

        [HttpPut("photos/{id}/metadata")]
        public IActionResult EditPhotoMetadata(int id, EditPhotoMetadataRequest request)
        {
            var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (adminId == null)
                return Unauthorized();

            var dto = new EditPhotoMetadataDto
            {
                PhotoId = id,
                UserId = adminId,
                Description = request.Description,
                Hashtags = request.Hashtags
            };

            _photoService.EditMetadata(dto, isAdmin: true);

            return Ok("Photo metadata updated by admin.");
        }
    }
}