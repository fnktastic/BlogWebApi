using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogWebApi.Enums;
using BlogWebApi.Logging;
using BlogWebApi.Models;
using BlogWebApi.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BlogWebApi.ViewModels;

namespace BlogWebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly ILogger<UserController> _logger = null;

        public UserController(IUserRepository userRepository, ICommentRepository commentRepository, ILogger<UserController> logger)
        {
            _userRepository = userRepository;
            _commentRepository = commentRepository;
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<User> GetAll()
        {
            _logger.LogInformation(LoggingEvents.ListItems, "Getting All Users");

            return _userRepository.Users;
        }

        [HttpGet("{id}", Name = "GetUser")]
        public IActionResult GetById(long id)
        {
            _logger.LogInformation(LoggingEvents.GetItem, "Getting User By Id ", id);

            var item = _userRepository.Users.FirstOrDefault(u => u.Id == id);
            if (item == null)
            {
                _logger.LogWarning(LoggingEvents.GetItemNotFound, "Getting User By Id Not Found ", id);

                return NotFound();
            }

            return new ObjectResult(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody] User item)
        {
            _logger.LogInformation(LoggingEvents.InsertItem, "Add New User");

            if (item == null)
            {
                _logger.LogWarning(LoggingEvents.InsertItem, "New User is NULL");

                return BadRequest();
            }

            _userRepository.SaveUser(item);
            return CreatedAtRoute("GetUser", new { id = item.Id }, item);
        }

        [HttpPut("{id}/ChangeInitiales")]
        public IActionResult ChangeInitiales(long id, [FromBody] UserInitialsViewModel userInitials)
        {
            _logger.LogInformation(LoggingEvents.UpdateItem, "Update User Initiales ", id);

            var user = _userRepository.Users.FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                _logger.LogWarning(LoggingEvents.UpdateItemNotFound, "Update Item Not Found ", id);

                return NotFound();
            }

            user.FirstName = userInitials.FirstName;
            user.MiddleName = userInitials.MiddleName;
            user.LastName = userInitials.LastName;

            _userRepository.SaveUser(user);

            return new NoContentResult();

        }

        [HttpPut("{id}/ChangeRole")]
        public IActionResult ChangeRole(long id, [FromBody] RolesEnum role)
        {
            _logger.LogInformation(LoggingEvents.UpdateItem, "Update User Role ", id);

            var user = _userRepository.Users.FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                _logger.LogWarning(LoggingEvents.UpdateItemNotFound, "Update Item Not Found ", id);

                return NotFound();
            }

            user.Role = role;

            _userRepository.SaveUser(user);

            return new NoContentResult();
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] User item)
        {
            _logger.LogInformation(LoggingEvents.UpdateItem, "Update User ", id);

            if (item == null || item.Id != id)
            {
                _logger.LogWarning(LoggingEvents.UpdateItemNotFound, "Updating User Bad Request", id);

                return BadRequest();
            }

            var user = _userRepository.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                _logger.LogWarning(LoggingEvents.UpdateItemNotFound, "Updating User Not Found", id);

                return NotFound();
            }

            user.FirstName = item.FirstName;
            user.MiddleName = item.MiddleName;
            user.LastName = item.LastName;
            user.Username = item.Username;
            user.Password = item.Password;
            user.Email = item.Email;
            user.Phone = item.Phone;
            user.CreatedDate = item.CreatedDate;
            user.LastLoginDate = item.LastLoginDate;
            user.ModifyingDate = item.ModifyingDate;
            user.Role = item.Role;

            _userRepository.SaveUser(user);

            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            _logger.LogInformation(LoggingEvents.DeleteItem, "Delete User", id);

            var user = _userRepository.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                _logger.LogWarning(LoggingEvents.GetItemNotFound, "Delete User Not Found", id);

                return NotFound();
            }


            _userRepository.DeleteUser(id);

            return new NoContentResult();
        }

        [HttpGet("{id}/GetInitials")]
        public IActionResult GetInitials(long id)
        {
            _logger.LogInformation(LoggingEvents.GetItem, "Getting User Initials ", id);

            var user = _userRepository.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                _logger.LogWarning(LoggingEvents.GetItemNotFound, "Getting Initials User Not Found ", id);

                return NotFound();
            }

            string userInitials = $"{user.FirstName} {user.MiddleName} {user.LastName}";

            return new ObjectResult(userInitials);
        }

        [HttpGet("{id}/GetRole")]
        public IActionResult GetRole(long id)
        {
            _logger.LogInformation(LoggingEvents.GetItem, "Getting User Role ", id);

            var user = _userRepository.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                _logger.LogWarning(LoggingEvents.GetItemNotFound, "Getting User Role Not Found ", id);

                return NotFound();
            }

            string userRole = Enum.GetName(typeof(RolesEnum), user.Role);

            return new ObjectResult(userRole);
        }

        [HttpDelete("{id}/DeleteComments")]
        public IActionResult DeleteComments(long id)
        {
            _logger.LogInformation(LoggingEvents.DeleteItem, "Delete Comment", "Id ", id);

            var user = _userRepository.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                _logger.LogInformation(LoggingEvents.DeleteItem, "Delete Comment", "Not Found Id ", id);
                return NotFound();
            }

            var userComments = _commentRepository
                .Comments
                .Where(c => c.UserID == id)
                .ToList();

            foreach (var comment in userComments)
                _commentRepository.DeleteComment(comment.Id);

            return new NoContentResult();


        }

        [HttpGet("{id}/GetComments")]
        public IActionResult GetComments(long id)
        {
            _logger.LogInformation(LoggingEvents.GetItem, "Getting User Comments ", id);

            var user = _userRepository.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                _logger.LogWarning(LoggingEvents.GetItemNotFound, "Getting User Not Found ", id);

                return NotFound();
            }

            var userComments = _commentRepository
                .Comments
                .Where(c => c.UserID == id)
                .ToList();

            if (userComments == null || userComments.Count == 0)
            {
                _logger.LogWarning(LoggingEvents.GetItemNotFound, "Getting User Comments Not Found ", id);

                return NotFound();
            }

            return new ObjectResult(userComments);
        }

        [HttpGet("{id}/GetAttachments")]
        public IActionResult GetAttachments(long id)
        {
            _logger.LogInformation(LoggingEvents.GetItem, "Getting User Attachmnets ", id);

            var user = _userRepository.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                _logger.LogWarning(LoggingEvents.GetItemNotFound, "Getting User Not Found ", id);

                return NotFound();
            }

            var userAttachments = user.Posts
                .Where(p => p.UserID == user.Id)
                .Select(c => c.Attachments)
                .ToList();

            if (userAttachments == null || userAttachments.Count == 0)
            {
                _logger.LogWarning(LoggingEvents.GetItemNotFound, "Getting User GetAttachments Not Found ", id);

                return NotFound();
            }

            return new ObjectResult(userAttachments);
        }
    }
}