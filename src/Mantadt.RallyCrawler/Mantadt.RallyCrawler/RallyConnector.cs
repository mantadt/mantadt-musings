using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pearson.RallyCrawler
{
    public static class RallyConnector
    {
        public static RallyCrawlerDbContext _rcDB;

        static RallyConnector()
        {
            _rcDB = new RallyCrawlerDbContext();
        }
    }
}
