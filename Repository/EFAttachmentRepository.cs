using BlogWebApi.DataAccess;
using BlogWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogWebApi.Repository
{
    public class EFAttachmentRepository : IAttachmentRepository
    {
        private readonly ApplicationDbContext _context;

        public EFAttachmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Attachment> Attachments => _context.Attachments;

        public void SaveAttachment(Attachment attachment)
        {
            if(attachment.Id == 0)
            {
                _context.Attachments.Add(attachment);
            }
            else
            {
                Attachment dbEntry = _context.Attachments.FirstOrDefault(a => a.Id == attachment.Id);
                if(dbEntry != null)
                {
                    dbEntry.Name = attachment.Name;
                    dbEntry.Link = attachment.Link;
                    dbEntry.PostID = attachment.PostID;
                    dbEntry.Post = attachment.Post;
                }
            }

            _context.SaveChanges();
        }

        public Attachment DeleteAttachment(long attachmentId)
        {
            Attachment dbEntry = _context.Attachments.FirstOrDefault(a => a.Id == attachmentId);
            if(dbEntry != null)
            {
                _context.Attachments.Remove(dbEntry);
                _context.SaveChanges();
            }

            return dbEntry;
        }
    }
}
