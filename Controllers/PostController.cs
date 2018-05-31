using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogWebApi.Logging;
using BlogWebApi.Models;
using BlogWebApi.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BlogWebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly ILogger<PostController> _logger = null;

        public PostController(IPostRepository postRepository, ILogger<PostController> logger)
        {
            _postRepository = postRepository;
            _logger = logger;
        }

        [HttpPost]
        public IActionResult Create([FromBody] Post post)
        {
            _logger.LogInformation(LoggingEvents.InsertItem, "Add New Post");

            if (post == null)
            {
                _logger.LogWarning(LoggingEvents.InsertItem, "Add New Post");

                return BadRequest();
            }

            _postRepository.SavePost(post);
            return CreatedAtRoute("GetPost", new { id = post.Id }, post);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            _logger.LogInformation(LoggingEvents.DeleteItem, "Delete Post", "Id ", id);

            var post = _postRepository.Posts.FirstOrDefault(p => p.Id == id);
            if (post == null)
            {
                _logger.LogWarning(LoggingEvents.DeleteItem, "Delete Post", "Not Found Id ", id);

                return NotFound();
            }

            _postRepository.DeletePost(id);
            return new NoContentResult();
        }

        [HttpPut("{id}/ChangeMetadata"), ActionName("ChangeMetadata")]
        public IActionResult ChangeMetadata(long id, [FromBody] string metadata)
        {
            _logger.LogInformation(LoggingEvents.UpdateItem, "Update Metadata of Post ", id);

            var post = _postRepository.Posts.FirstOrDefault(p => p.Id == id);

            if (post == null)
            {
                _logger.LogWarning(LoggingEvents.UpdateItem, "Update Metadata of Post", id, "Post not found");
                return NotFound();
            }

            post.Metadata = metadata;

            _postRepository.SavePost(post);
            return new NoContentResult();
        }

        [HttpPut("{id}/ChangeBody"), ActionName("ChangeBody")]
        public IActionResult ChangeBody(long id, [FromBody] string body)
        {
            _logger.LogInformation(LoggingEvents.UpdateItem, "Update Body of Post ", id);

            var post = _postRepository.Posts.FirstOrDefault(p => p.Id == id);

            if (post == null)
            {
                _logger.LogWarning(LoggingEvents.UpdateItem, "Update Body of Post", id, "Post not found");
                return NotFound();
            }

            post.Body = body;

            _postRepository.SavePost(post);
            return new NoContentResult();
        }

        [HttpPut("{id}/ChangeTemplate")]
        public IActionResult ChangeTemplate(long id, [FromBody] Template template)
        {
            _logger.LogInformation(LoggingEvents.UpdateItem, "Update Template of Post ", id);

            var post = _postRepository.Posts.FirstOrDefault(p => p.Id == id);

            if (post == null || template == null)
            {
                _logger.LogWarning(LoggingEvents.UpdateItem, "Update Template of Post", id, "Post not found");
                return NotFound();
            }


            post.TemplateID = template.Id;

            _postRepository.SavePost(post);
            return new NoContentResult();
        }

        [HttpGet("{id}/GetBody")]
        public IActionResult GetBody(long id)
        {
            _logger.LogInformation(LoggingEvents.GetItem, "Getting Post Body By Id", id);

            var post = _postRepository.Posts.FirstOrDefault(p => p.Id == id);
            if (post == null)
            {
                _logger.LogWarning(LoggingEvents.GetItemNotFound, "Getting Post Body By Id", id, "Not Found");

                return NotFound();
            }

            return new ObjectResult(post.Body);
        }

        [HttpGet("{id}/GetTemplate")]
        public IActionResult GetTemplate(long id)
        {
            _logger.LogInformation(LoggingEvents.GetItem, "Getting Post Template By Id", id);

            var post = _postRepository.Posts.FirstOrDefault(p => p.Id == id);
            if (post == null)
            {
                _logger.LogWarning(LoggingEvents.GetItemNotFound, "Getting Post Template By Id", id, "Not Found");
                return NotFound();
            }

            return new ObjectResult(post.Template);
        }

        [HttpGet("{id}/GetComments")]
        public IActionResult GetComments(long id)
        {
            _logger.LogInformation(LoggingEvents.GetItem, "Getting Post Comments By Id", id);

            var post = _postRepository.Posts.FirstOrDefault(p => p.Id == id);
            if (post == null)
            {
                _logger.LogWarning(LoggingEvents.GetItemNotFound, "Getting Post Comments By Id", id, "Not Found");

                return NotFound();
            }

            return new ObjectResult(post.Comments);
        }

        [HttpGet("{id}/GetAttachments")]
        public IActionResult GetAttachments(long id)
        {
            _logger.LogInformation(LoggingEvents.GetItem, "Getting Post Attachments By Id", id);

            var post = _postRepository.Posts.FirstOrDefault(p => p.Id == id);
            if (post == null)
            {
                _logger.LogWarning(LoggingEvents.GetItemNotFound, "Getting Post Attachments By Id", id, "Not Found");

                return NotFound();
            }

            return new ObjectResult(post.Attachments);
        }

        [HttpGet]
        public IEnumerable<Post> GetAll()
        {
            _logger.LogInformation(LoggingEvents.ListItems, "Getting All Posts");

            return _postRepository.Posts;
        }

        [HttpGet("{id}", Name = "GetPost")]
        public IActionResult GetById(long id)
        {
            _logger.LogInformation(LoggingEvents.GetItem, "Getting Post By Id", id);

            var post = _postRepository.Posts.FirstOrDefault(p => p.Id == id);
            if (post == null)
            {
                _logger.LogWarning(LoggingEvents.GetItemNotFound, "Getting Post By Id", id, "Not Found");

                return NotFound();
            }

            return new ObjectResult(post);
        }
    }
}