using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogWebApi.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public string Text { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime UpdatedDateTime { get; set; }
        
        public int PostID { get; set; }
        public Post Post { get; set; }

        public int UserID { get; set; }
        //public User User { get; set; }
    }
}
