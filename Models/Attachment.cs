using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogWebApi.Models
{
    public class Attachment
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Link { get; set; }

        public int PostID { get; set; }
        public Post Post { get; set; }
    }
}
