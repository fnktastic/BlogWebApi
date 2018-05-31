using BlogWebApi.DataAccess;
using BlogWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogWebApi.Repository
{
    public class EFCommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _context;

        public EFCommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Comment> Comments => _context.Comments;

        public void SaveComment(Comment comment)
        {
            if(comment.Id == 0)
            {
                _context.Comments.Add(comment);
            }
            else
            {
                Comment dbEntry = _context.Comments.FirstOrDefault(c => c.Id == comment.Id);
                if(dbEntry != null)
                {
                    dbEntry.Text = comment.Text;
                    dbEntry.CreatedDateTime = comment.CreatedDateTime;
                    dbEntry.UpdatedDateTime = comment.UpdatedDateTime;
                    dbEntry.PostID = comment.PostID;
                    dbEntry.Post = comment.Post;
                    dbEntry.UserID = comment.UserID;
                }
            }

            _context.SaveChanges();
        }

        public Comment DeleteComment(long commentId)
        {
            Comment dbEntry = _context.Comments.FirstOrDefault(c => c.Id == commentId);
            if(dbEntry !=null)
            {
                _context.Comments.Remove(dbEntry);
                _context.SaveChanges();
            
            }

            return dbEntry;
        }
    }
}
