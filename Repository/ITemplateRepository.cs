using BlogWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogWebApi.Repository
{
    public interface ITemplateRepository
    {
        IEnumerable<Template> Templates { get; }
        void SaveTemplate(Template template);
        Template DeleteTemplate(long templateId);
    }
}
