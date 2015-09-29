using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pearson.RallyCrawler.Models
{
    public class TeamMapping
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int TeamId { get; set; }
        public bool IsActive { get; set; } 
        public bool IsReviewer { get; set; }
        
        public virtual List<User> Users { get; set; }       
    }
}
