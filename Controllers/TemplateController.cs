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
    public class TemplateController : Controller
    {
        private readonly ITemplateRepository _templateRepository;
        private readonly ILogger<TemplateController> _logger = null;

        public TemplateController(ITemplateRepository templateRepository, ILogger<TemplateController> logger)
        {
            _templateRepository = templateRepository;
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Template> GetAll()
        {
            _logger.LogInformation(LoggingEvents.ListItems, "Getting All Templates");

            return _templateRepository.Templates;
        }

        [HttpGet("{id}", Name = "GetTemplate")]
        public IActionResult GetById(long id)
        {
            _logger.LogInformation(LoggingEvents.GetItem, "Getting Template By Id ", id);

            var item = _templateRepository.Templates.FirstOrDefault(t => t.Id == id);
            if (item == null)
            {
                _logger.LogWarning(LoggingEvents.GetItemNotFound, "Getting Template By Id Not Found ", id);

                return NotFound();
            }

            return new ObjectResult(item);
        }

        [HttpGet("{id}/GetBody")]
        public IActionResult GetBody(long id)
        {
            _logger.LogInformation(LoggingEvents.GetItem, "Getting Template By Id ", id);

            var item = _templateRepository.Templates.FirstOrDefault(t => t.Id == id);
            if (item == null)
            {
                _logger.LogWarning(LoggingEvents.GetItemNotFound, "Getting Template By Id Not Found ", id);

                return NotFound();
            }

            return new ObjectResult(item.Body);
        }

        [HttpGet("{id}/GetName")]
        public IActionResult GetName(long id)
        {
            _logger.LogInformation(LoggingEvents.GetItem, "Getting Template By Id ", id);

            var item = _templateRepository.Templates.FirstOrDefault(t => t.Id == id);
            if (item == null)
            {
                _logger.LogWarning(LoggingEvents.GetItemNotFound, "Getting Template By Id Not Found ", id);

                return NotFound();
            }

            return new ObjectResult(item.Name);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            _logger.LogInformation(LoggingEvents.DeleteItem, "Delete Tenplate", id);

            var item = _templateRepository.Templates.FirstOrDefault(t => t.Id == id);
            if (item == null)
            {
                _logger.LogWarning(LoggingEvents.GetItemNotFound, "Delete Template Not Found", id);

                return NotFound();
            }

            _templateRepository.DeleteTemplate(id);
            return new NoContentResult();
        }

        [HttpPost]
        public IActionResult Create([FromBody] Template template)
        {
            _logger.LogInformation(LoggingEvents.InsertItem, "Add New Template");

            if (template == null)
            {
                _logger.LogWarning(LoggingEvents.InsertItem, "New Template is NULL");

                return BadRequest();
            }

            _templateRepository.SaveTemplate(template);
            return CreatedAtRoute("GetTemplate", new { id = template.Id }, template);
        }

        [HttpPut("{id}/ChangeBody")]
        public IActionResult ChangeBody(long id, [FromBody] string body)
        {
            _logger.LogInformation(LoggingEvents.UpdateItem, "Update Template Body ", id);

            var template = _templateRepository.Templates.FirstOrDefault(t => t.Id == id);
            if(template == null)
            {
                _logger.LogWarning(LoggingEvents.UpdateItemNotFound, "Update Template Body Not Found ", id);
                return NotFound();
            }

            template.Body = body;
            _templateRepository.SaveTemplate(template);
            return new NoContentResult();
        }

        [HttpPut("{id}/ChangeName")]
        public IActionResult ChangeName(long id, [FromBody] string name)
        {
            _logger.LogInformation(LoggingEvents.UpdateItem, "Update Template Body ", id);

            var template = _templateRepository.Templates.FirstOrDefault(t => t.Id == id);
            if (template == null)
            {
                _logger.LogWarning(LoggingEvents.UpdateItemNotFound, "Update Template Body Not Found ", id);
                return NotFound();
            }

            template.Name = name;
            _templateRepository.SaveTemplate(template);
            return new NoContentResult();
        }


    }
}