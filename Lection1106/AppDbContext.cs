using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lection1106
{
    internal class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        protected string db = "";
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source = app.db");
        }
    }
}
