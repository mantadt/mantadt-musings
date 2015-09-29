using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pearson.RallyCrawler
{
    public static class RallyAPIConnector
    {
        public static RallyCrawlerDbContext _rcDB;

        static RallyAPIConnector()
        {
            _rcDB = new RallyCrawlerDbContext();
        }
    }
}
