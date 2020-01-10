using Microsoft.AspNetCore.Mvc;
using Task.Model;
using Task.Services;

namespace Task.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        public ProfileController(ProfileService profileService)
        {
            _profileService = profileService;

        }
        private readonly ProfileService _profileService;

        //https://localhost:5001/api/profile/users
        [HttpGet("users")]
        public IActionResult GetAllU()
        {
            var user = _profileService.GetAllUsers();
            if (user == null)
                return NotFound("Bad request");
            else
                return Ok(user);
        }

        //https://localhost:5001/api/profile/user:1
        [HttpGet("user:{id}")]
        public IActionResult GetUById(int id)
        {
            var user = _profileService.GetUserById(id);
            if (user == null)
                return NotFound("Bad request");
            else
                return Ok(user);
        }
        [HttpGet]
        public IActionResult GetAllP()
        {
            var profile = _profileService.GetAllProfiles();
            if (profile == null)
                return NotFound("Bad request");
            else
                return Ok(profile);
        }

        
        [HttpGet("{id}")]
        public IActionResult GetPById(int id)
        {
            var profile = _profileService.GetProfileById(id);
            if (profile == null)
                return NotFound("Bad request");
            else
                return Ok(profile);
        }

        [HttpPost]
        public IActionResult Add([FromBody] Profile value)
        {
            if (!ModelState.IsValid)
                return Content($"write all info");
            var profile = _profileService.AddNewProfile(value);
            if (profile == null)
                return NotFound("Bad Request");
            else
                return Ok("New profile is added");
        }

        [HttpPut]
        public IActionResult UpdateBlogpost([FromBody] Profile value)
        {
            var data = _profileService.UpdateProfile(value);
            if (data.profile == null)
                return BadRequest("400");
            else
                return Ok(data);

        }

        [HttpDelete("{id}")]
        public IActionResult DeletebyId(int id)
        {
            var data = _profileService.DeleteProfile(id);
            if (data.exception != null)
            {
                return BadRequest(data.exception.Message);
            }
            else
            {
                return Ok(data.result);
            }
        }
    }
}
