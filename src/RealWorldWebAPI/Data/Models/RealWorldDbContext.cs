using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RealWorldWebAPI.Data.Models
{
    public class RealWorldDbContext : DbContext
    {

        private static bool _created;

        public RealWorldDbContext(DbContextOptions<RealWorldDbContext> contextOptions) : 
            base(contextOptions)
        {
            if (_created) return;

            Database.Migrate();
            _created = true;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=.\\App_Data\\RealWorld.db");

        public DbSet<Article> Articles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}
