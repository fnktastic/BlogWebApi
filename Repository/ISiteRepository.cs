using BlogWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogWebApi.Repository
{
    public interface ISiteRepository
    {
        IEnumerable<Site> Sites { get; }
        void SaveSite(Site site);
        Site DeleteSite(long siteId);
    }
}
