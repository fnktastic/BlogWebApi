using BlogWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogWebApi.Repository
{
    public interface IAttachmentRepository
    {
        IEnumerable<Attachment> Attachments { get; }
        void SaveAttachment(Attachment attachment);
        Attachment DeleteAttachment(long attachmentId);
    }
}
