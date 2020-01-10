using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Task.Entities;
using Task.Services;

namespace Task.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public AuthController(ProfileService profileService)
        {
            _profileService = profileService;
        }
        private readonly ProfileService _profileService;

        [HttpPost]
        public IActionResult Auth([FromBody]User value)
        {
            var user = _profileService.Authorization(value);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }
    }
}
