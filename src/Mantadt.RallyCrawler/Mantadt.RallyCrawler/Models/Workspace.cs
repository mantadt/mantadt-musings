using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pearson.RallyCrawler.Models
{
    public class Workspace
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; } 
        
        public virtual List<Team> Teams { get; set; } 
    }
}
