using BlogWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogWebApi.Repository
{
    public interface IPostRepository
    {
        IEnumerable<Post> Posts { get; }
        void SavePost(Post post);
        Post DeletePost(long postId);
    }
}
