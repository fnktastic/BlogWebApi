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
    public class AttachmentController : Controller
    {
        private readonly IAttachmentRepository _attachmentRepository;
        private readonly ILogger<AttachmentController> _logger = null;

        public AttachmentController(IAttachmentRepository attachmentRepository, ILogger<AttachmentController> logger)
        {
            _attachmentRepository = attachmentRepository;
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Attachment> GetAll()
        {
            _logger.LogInformation(LoggingEvents.ListItems, "Getting All Attachments");

            return _attachmentRepository.Attachments;
        }

        [HttpGet("{id}", Name = "GeAttachment")]
        public IActionResult GetById(long id)
        {
            _logger.LogInformation(LoggingEvents.GetItem, "Getting Attachment By Id", id);

            var item = _attachmentRepository.Attachments.FirstOrDefault(a => a.Id == id);
            if(item == null)
            {
                _logger.LogInformation(LoggingEvents.GetItem, "Getting Attachment By Id", id, "Not Found");
                return NotFound();
            }

            return new ObjectResult(item);
        }

        [HttpPost("{postId}")]
        public IActionResult Create(int postId, [FromBody] Attachment attachment)
        {
            _logger.LogInformation(LoggingEvents.InsertItem, "Insert New Attachment");

            if (attachment == null)
            {
                _logger.LogWarning(LoggingEvents.InsertItem, "Insert New Attachment Error");
                return BadRequest();
            }

            attachment.PostID = postId;

            _attachmentRepository.SaveAttachment(attachment);
            return CreatedAtRoute("GeAttachment", new { id = attachment.Id }, attachment);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] Attachment attachment)
        {
            _logger.LogInformation(LoggingEvents.UpdateItem, "Update Attachment", "Id ", id);
            if (attachment == null || attachment.Id != id)
            {
                _logger.LogInformation(LoggingEvents.UpdateItem, "Update Attachment", "Id", id);
                return BadRequest();
            }

            var item = _attachmentRepository.Attachments.FirstOrDefault(a => a.Id == id);
            if(item == null)
            {
                _logger.LogInformation(LoggingEvents.UpdateItem, "Update Attachment", "Not Found Id", id);
                return NotFound();
            }

            item.Link = attachment.Link;
            item.Name = attachment.Name;
            item.PostID = attachment.PostID;

            _attachmentRepository.SaveAttachment(item);
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            _logger.LogInformation(LoggingEvents.DeleteItem, "Delete Attachment", "Id ", id);

            var attachment = _attachmentRepository.Attachments.FirstOrDefault(a => a.Id == id);
            if(attachment == null)
            {
                _logger.LogInformation(LoggingEvents.DeleteItem, "Delete Attachment", "Not Found Id ", id);
                return NotFound();
            }
            _attachmentRepository.DeleteAttachment(id);
            return new NoContentResult();
        }
    }
}