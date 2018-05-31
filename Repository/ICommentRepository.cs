using BlogWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogWebApi.Repository
{
    public interface ICommentRepository
    {
        IEnumerable<Comment> Comments { get; }
        void SaveComment(Comment comment);
        Comment DeleteComment(long commentId);
    } 
}
