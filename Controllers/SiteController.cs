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
    public class SiteController : Controller
    {
        private readonly ISiteRepository _siteRepository;
        private readonly ILogger<SiteController> _logger = null;

        public SiteController(ISiteRepository siteRepository, ILogger<SiteController> logger)
        {
            _siteRepository = siteRepository;
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Site> GetAll()
        {
            _logger.LogInformation(LoggingEvents.ListItems, "Getting All Sites");

            return _siteRepository.Sites;
        }

        [HttpGet("{id}", Name = "GetSite")]
        public IActionResult GetById(long id)
        {
            _logger.LogInformation(LoggingEvents.GetItem, "Getting Site By Id", id);

            var item = _siteRepository.Sites.FirstOrDefault(s => s.Id == id);
            if (item == null)
            {
                _logger.LogInformation(LoggingEvents.GetItem, "Getting Site By Id", id, "Not Found");
                return NotFound();
            }

            return new ObjectResult(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Site site)
        {
            _logger.LogInformation(LoggingEvents.InsertItem, "Insert New Site");

            if (site == null)
            {
                _logger.LogWarning(LoggingEvents.InsertItem, "Insert New Site Error");
                return BadRequest();
            }

            _siteRepository.SaveSite(site);
            return CreatedAtRoute("GetSite", new { id = site.Id }, site);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] Site site)
        {
            _logger.LogInformation(LoggingEvents.UpdateItem, "Update Site", "Id ", id);

            if (site == null || site.Id != id)
            {
                _logger.LogInformation(LoggingEvents.UpdateItem, "Update Site", "Id", id);
                return BadRequest();
            }

            var item = _siteRepository.Sites.FirstOrDefault(s => s.Id == id);
            if(item == null)
            {
                _logger.LogInformation(LoggingEvents.UpdateItem, "Update Site", "Not Found Id", id);
                return NotFound();
            }

            item.Name = site.Name;
            item.Posts = site.Posts;

            _siteRepository.SaveSite(item);
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            _logger.LogInformation(LoggingEvents.DeleteItem, "Delete Site", "Id ", id);

            var site = _siteRepository.Sites.FirstOrDefault(s => s.Id == id);
            if(site == null)
            {
                _logger.LogInformation(LoggingEvents.DeleteItem, "Delete Site", "Not Found Id ", id);
                return NotFound();
            }

            _siteRepository.DeleteSite(id);
            return new NoContentResult();
        }
    }
}