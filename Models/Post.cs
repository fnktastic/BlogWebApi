using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BlogWebApi.Models
{
    public class Post
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Body { get; set; }
        public string Metadata { get; set; }

        public int UserID { get; set; }
        public User User { get; set; }

        public int? TemplateID { get; set; }
        public Template Template { get; set; }

        public int SiteID { get; set; }
        public Site Site { get; set; }

        public IEnumerable<Attachment> Attachments { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
    }
}
