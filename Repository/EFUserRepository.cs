using BlogWebApi.DataAccess;
using BlogWebApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogWebApi.Repository
{
    public class EFUserRepository : IUserRepository
    {
        ApplicationDbContext _context;

        public EFUserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<User> Users => _context.Users
            .Include(u => u.Posts)
                .ThenInclude(c => c.Comments)
            .Include(p => p.Posts)
                .ThenInclude(a => a.Attachments)
            .ToList();

        public void SaveUser(User user)
        {
            if(user.Id == 0)
            {
                _context.Users.Add(user);
            }
            else
            {
                User dbEntry = _context.Users.FirstOrDefault(u => u.Id == user.Id);
                if(dbEntry != null)
                {
                    dbEntry.Username = user.Username;
                    dbEntry.Password = user.Password;
                    dbEntry.Email = user.Email;
                    dbEntry.Phone = user.Phone;
                    dbEntry.CreatedDate = user.CreatedDate;
                    dbEntry.ModifyingDate = user.ModifyingDate;
                    dbEntry.LastLoginDate = user.LastLoginDate;
                    dbEntry.FirstName = user.FirstName;
                    dbEntry.MiddleName = user.MiddleName;
                    dbEntry.LastName = user.LastName;
                    dbEntry.Role = user.Role;

                    //dbEntry.Posts = user.Posts;
                }
            }

            _context.SaveChanges();
        }

        public User DeleteUser(long userId)
        {
            User dbEntry = _context.Users.FirstOrDefault(u => u.Id == userId);
            if(dbEntry != null)
            {
                _context.Users.Remove(dbEntry);
                _context.SaveChanges();
            }

            return dbEntry;
        }
    }
}
