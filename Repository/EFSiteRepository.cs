using BlogWebApi.DataAccess;
using BlogWebApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogWebApi.Repository
{
    public class EFSiteRepository : ISiteRepository
    {
        private readonly ApplicationDbContext _context;

        public EFSiteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Site> Sites => _context.Sites
            .Include(s => s.Posts)
                .ThenInclude(a => a.Attachments)
            .Include(s => s.Posts)
                .ThenInclude(c => c.Comments)
            .Include(s => s.Posts)
                .ThenInclude(u => u.User)
            .Include(s => s.Posts)
                .ThenInclude(t => t.Template)
            .ToList();

        public void SaveSite(Site site)
        {
            if (site.Id == 0)
            {
                _context.Sites.Add(site);
            }
            else
            {
                Site dbEntry = _context.Sites.FirstOrDefault(s => s.Id == site.Id);
                if (dbEntry != null)
                {
                    dbEntry.Name = site.Name;

                    dbEntry.Posts = site.Posts;
                }
            }

            _context.SaveChanges();
        }

        public Site DeleteSite(long siteId)
        {
            Site dbEntry = _context.Sites.FirstOrDefault(s => s.Id == siteId);
            if(dbEntry != null)
            {
                _context.Sites.Remove(dbEntry);
                _context.SaveChanges();
            }

            return dbEntry;
        }

    }
}
