using Microsoft.AspNetCore.Mvc;
using Task.Model;
using Task.Services;

namespace Task.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogpostController : ControllerBase
    {
        public BlogpostController(BlogpostService blogpostService)
        {
            _blogpostService = blogpostService;
        }
        private readonly BlogpostService _blogpostService;
        // GET: api/Blogpost

        //Http caching
        [HttpGet]
        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 300)]
        public IActionResult GetAll()
        {
            var blogpost = _blogpostService.GetAll();
            if (blogpost == null)
                return NotFound("Bad request");
            else
                return Ok(blogpost);
        }

        // GET: api/Blogpost/5
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var blogpost = _blogpostService.GetBlogpostById(id);
            if (blogpost == null)
                return NotFound("Bad request");
            else
                return Ok(blogpost);
        }

        [HttpPost]
        public IActionResult Add([FromBody] Blogpost value)
        {
            if (!ModelState.IsValid)
                return Content($"write the post");
            var blogpost = _blogpostService.AddNewBlogpost(value);
            if (blogpost == null)
                return NotFound("Bad Request");
            else
                return Ok("New blogpost is added");
        }

        [HttpPut]
        public IActionResult UpdateBlogpost([FromBody] Blogpost value)
        {
            var data = _blogpostService.UpdateBlogpost(value);
            if (data.blogpost == null)
                return BadRequest("400");
            else
                return Ok(data);

        }

        [HttpDelete("{id}")]
        public IActionResult DeletebyId(int id)
        {
            var data = _blogpostService.DeleteBlogpost(id);
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
