using BlogWebApi.DataAccess;
using BlogWebApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogWebApi.Repository
{
    public class EFPostRepository : IPostRepository
    {
        private readonly ApplicationDbContext _context;

        public EFPostRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Post> Posts => _context.Posts
            .Include(p => p.Attachments)
            .Include(p => p.Template)
            .Include(p => p.Comments)
            .Include(p => p.User)
            .ToList();

        public void SavePost(Post post)
        {
            if(post.Id == 0)
            {
                _context.Posts.Add(post);
            }
            else
            {
                Post dbEntry = _context.Posts.FirstOrDefault(p => p.Id == post.Id);
                if(dbEntry != null)
                {
                    dbEntry.Name = post.Name;
                    dbEntry.CreatedAt = post.CreatedAt;
                    dbEntry.Body = post.Body;
                    dbEntry.Metadata = post.Metadata;

                    dbEntry.UserID = post.UserID;
                    dbEntry.User = post.User;
                    dbEntry.TemplateID = post.TemplateID;
                    dbEntry.Template = post.Template;
                    dbEntry.SiteID = post.SiteID;
                    dbEntry.Site = post.Site;

                    dbEntry.Attachments = post.Attachments;
                    dbEntry.Comments = post.Comments;
                }
            }

            _context.SaveChanges();
        }

        public Post DeletePost(long postId)
        {
            Post dbEntry = _context.Posts.FirstOrDefault(p => p.Id == postId);
            if(dbEntry != null)
            {
                _context.Posts.Remove(dbEntry);
                _context.SaveChanges();
            }

            return dbEntry;
        }

    }
}
