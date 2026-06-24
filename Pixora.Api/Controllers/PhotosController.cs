using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pixora.Api.Requests;
using Pixora.BL.DTOs;
using Pixora.BL.Models;
using Pixora.BL.Services.Photos;
using System.Security.Claims;

namespace Pixora.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PhotosController : ControllerBase
    {
        private readonly IPhotoService _photoService;
        private readonly IMapper _mapper;

        public PhotosController(IPhotoService photoService, IMapper mapper)
        {
            _photoService = photoService;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAll()
        {
            var photos = _photoService.GetAll();
            return Ok(photos.Select(p => _mapper.Map<PhotoDto>(p)));
        }

        [HttpGet("latest")]
        [AllowAnonymous]
        public IActionResult GetLatest([FromQuery] int count = 10)
        {
            var photos = _photoService.GetLatest(count);
            return Ok(photos.Select(p => _mapper.Map<PhotoDto>(p)));
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult GetById(int id)
        {
            var photo = _photoService.GetById(id);

            if (photo == null)
            {
                return NotFound("Photo not found.");
            }

            return Ok(_mapper.Map<PhotoDto>(photo));
        }

        [HttpPost("search")]
        [AllowAnonymous]
        public IActionResult Search(PhotoSearchDto dto)
        {
            var photos = _photoService.Search(dto);
            return Ok(photos.Select(p => _mapper.Map<PhotoDto>(p)));
        }

        [HttpPost("upload")]
        [Authorize]
        public async Task<IActionResult> Upload([FromForm] UploadPhotoRequest request)
        {
            if (request.File == null || request.File.Length == 0)
            {
                return BadRequest("Image is required.");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized();
            }
            try
            {
                var photoId = await _photoService.UploadPhotoAsync(request.ToDto(userId));

                return Ok(new { PhotoId = photoId });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }

        }

        [HttpPut("{id}/metadata")]
        [Authorize]
        public IActionResult EditMetadata(int id, EditPhotoMetadataRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized();
            }

            var dto = new EditPhotoMetadataDto
            {
                PhotoId = id,
                UserId = userId,
                Description = request.Description,
                Hashtags = request.Hashtags
            };

            try
            {
                _photoService.EditMetadata(dto, isAdmin:false);

                return Ok("Photo metadata updated.");
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized();
            }

            var isAdmin = User.IsInRole("Admin");

            try
            {
                await _photoService.DeleteAsync(id, userId, isAdmin);
                return Ok("Photo deleted.");
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("{id}/download")]
        [Authorize]
        public async Task<IActionResult> Download(int id)
        {
            var file = await _photoService.DownloadAsync(id);

            return File(file.Stream, file.ContentType, file.FileName);
        }
    }
}