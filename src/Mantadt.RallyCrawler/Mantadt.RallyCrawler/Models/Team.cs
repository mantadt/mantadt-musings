using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pearson.RallyCrawler.Models
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int WorkSpaceId { get; set; } 

        public virtual List<TeamMapping> TeamMappings { get; set; }
        public virtual Workspace Workspace { get; set; }
    }
}
