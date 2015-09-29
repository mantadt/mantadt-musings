using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pearson.RallyCrawler.Models
{
    public class Task
    {
        public int Id { get; set; }
        public int TeamId { get; set; }
        public int SprintId { get; set; }
        public int StoryId { get; set; } 
        public int DefectId { get; set; }
        public int TaskId { get; set; } 
        public double SPlanned { get; set; } 
        public double TPlanned { get; set; }  
        public double TActual { get; set; }
        public double TRemaining { get; set; }
        public DateTime TDate { get; set; }
        public string status { get; set; }
    }
}
