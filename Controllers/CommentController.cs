using Microsoft.AspNetCore.Mvc;
using Task.Model;
using Task.Services;

namespace Task.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        public CommentController(CommentService commentService)
        {
            _commentService = commentService;
        }
        private readonly CommentService _commentService;
        
        [HttpGet]
        public IActionResult GetAll()
        {
            var comment = _commentService.GetAll();
            if (comment == null)
                return NotFound("Bad request");
            else
                return Ok(comment);
        }


        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var comment = _commentService.GetCommentById(id);
            if (comment == null)
                return NotFound("Bad request");
            else
                return Ok(comment);
        }

        [HttpPost]
        public IActionResult Add([FromBody] Comment value)
        {
            var comment = _commentService.AddNewComment(value);
            if (comment == null)
                return NotFound("Bad Request");
            else
                return Ok("New comment is added");
        }

        [HttpPut]
        public IActionResult UpdateBlogpost([FromBody] Comment value)
        {
            var data = _commentService.UpdateComment(value);
            if (data.comment == null)
                return BadRequest("400");
            else
                return Ok(data);

        }

        [HttpDelete("{id}")]
        public IActionResult DeletebyId(int id)
        {
            var data = _commentService.DeleteComment(id);
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
