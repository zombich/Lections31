using Lection1017.Models;
using Microsoft.EntityFrameworkCore;

namespace Lection1017.Contexts
{
    public partial class StudentsDbContext : DbContext
    {
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Student> Students { get; set; }

        //public StudentsDbContext()
        //{ 
        
        //}

        //public StudentsDbContext(DbContextOptions<StudentsDbContext> options): base(options)
        //{ 
        
        //}
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer("Data Source=mssql;Initial Catalog=ispp3104;Persist Security Info=True;User ID=ispp3104;Password=3104;Encrypt=True;Trust Server Certificate=True");
            optionsBuilder.UseSqlite("DataSource = students.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Group>().HasData(
                new Group { Id = 1, Title = "ИСПП-31"},
                new Group { Id = 2, Title = "ИСПП-34"},
                new Group { Id = 3, Title = "ИСПП-45"});
            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
