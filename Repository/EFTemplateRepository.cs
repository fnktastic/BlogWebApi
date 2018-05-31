using BlogWebApi.DataAccess;
using BlogWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogWebApi.Repository
{
    public class EFTemplateRepository : ITemplateRepository
    {
        private readonly ApplicationDbContext _context;

        public EFTemplateRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Template> Templates => _context.Templates;

        public void SaveTemplate(Template template)
        {
            if(template.Id == 0)
            {
                _context.Templates.Add(template);
            }
            else
            {
                Template dbEntry = _context.Templates.FirstOrDefault(t => t.Id == template.Id);
                if(dbEntry != null)
                {
                    dbEntry.Name = template.Name;
                    dbEntry.Body = template.Body;
                }
            }

            _context.SaveChanges();
        }

        public Template DeleteTemplate(long templateId)
        {
            Template dbEntry = _context.Templates.FirstOrDefault(t => t.Id == templateId);
            if(dbEntry != null)
            {
                _context.Templates.Remove(dbEntry);
                _context.SaveChanges();
            }

            return dbEntry;
        }

    }
}
