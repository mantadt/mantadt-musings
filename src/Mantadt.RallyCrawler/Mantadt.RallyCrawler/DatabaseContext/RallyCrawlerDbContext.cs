using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Pearson.RallyCrawler.Models;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Pearson.RallyCrawler
{
    public class RallyCrawlerDbContext : DbContext
    {
        public RallyCrawlerDbContext() : base("name=dbConnection") { }
        
        public DbSet<Models.Task> Tasks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamMapping> TeamMappings { get; set; }
        public DbSet<Workspace> Workspaces { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
