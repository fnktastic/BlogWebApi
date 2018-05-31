using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogWebApi.Logging;
using BlogWebApi.Models;
using BlogWebApi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BlogWebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class CommentController : Controller
    {
        private readonly ICommentRepository _commentRepository;
        private readonly ILogger<CommentController> _logger = null;

        public CommentController(ICommentRepository commentRepository, ILogger<CommentController> logger)
        {
            _commentRepository = commentRepository;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Editor, User")]
        public IEnumerable<Comment> GetAll()
        {
            _logger.LogInformation(LoggingEvents.ListItems, "Getting All Comments");

            return _commentRepository.Comments;
        }

        [HttpGet("{id}", Name = "GetComment")]
        public IActionResult GetById(long id)
        {
            _logger.LogInformation(LoggingEvents.GetItem, "Getting Comment By Id", id);

            var item = _commentRepository.Comments.FirstOrDefault(c => c.Id == id);
            if (item == null)
            {
                _logger.LogInformation(LoggingEvents.GetItem, "Getting Comment By Id", id, "Not Found");
                return NotFound();
            }

            return new ObjectResult(item);
        }

        [HttpPost("{userId}")]
        public IActionResult Create(int userId, [FromBody] Comment comment)
        {
            _logger.LogInformation(LoggingEvents.InsertItem, "Insert New Comment");

            if(comment == null)
            {
                _logger.LogWarning(LoggingEvents.InsertItem, "Insert New Comment Error");
                return BadRequest();
            }

            comment.UserID = userId;

            _commentRepository.SaveComment(comment);
            return CreatedAtRoute("GetComment", new { id = comment.Id }, comment);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] Comment comment)
        {
            _logger.LogInformation(LoggingEvents.UpdateItem, "Update Comment", "Id ", id);

            if(comment == null || comment.Id != id)
            {
                _logger.LogInformation(LoggingEvents.UpdateItem, "Update Comment Bad Request", "Id", id);
                return BadRequest();
            }

            var item = _commentRepository.Comments.FirstOrDefault(c => c.Id == id);
            if(item == null)
            {
                _logger.LogInformation(LoggingEvents.UpdateItem, "Update Comment", "Not Found Id", id);
                return NotFound();
            }

            item.CreatedDateTime = comment.CreatedDateTime;
            item.UpdatedDateTime = comment.UpdatedDateTime;
            item.Text = comment.Text;
            item.PostID = comment.PostID;

            _commentRepository.SaveComment(item);
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            _logger.LogInformation(LoggingEvents.DeleteItem, "Delete Comment", "Id ", id);

            var comment = _commentRepository.Comments.FirstOrDefault(c => c.Id == id);
            if(comment == null)
            {
                _logger.LogInformation(LoggingEvents.DeleteItem, "Delete Comment", "Not Found Id ", id);
                return NotFound();
            }

            _commentRepository.DeleteComment(id);
            return new NoContentResult();
        }
    }
}