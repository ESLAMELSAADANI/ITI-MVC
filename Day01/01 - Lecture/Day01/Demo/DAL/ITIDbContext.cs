using ModelsLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace Demo.DAL
{
    public class ITIDbContext : DbContext
    {
        //protected ITIDbContext()
        //{
        //}

        public ITIDbContext(DbContextOptions options) : base(options)
        {
            
        }


        public DbSet<Student> Students { get; set; }
        public DbSet<Department> Department { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=ITIMVC;Integrated Security=True;Trust Server Certificate=True");
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>(d =>
            {
                d.HasData(
                    new Department() { DeptId = 100, DeptName = "CS", Capacity = 50 },
                    new Department() { DeptId = 200, DeptName = "Cyber", Capacity = 25 },
                    new Department() { DeptId = 300, DeptName = "Java", Capacity = 30 },
                    new Department() { DeptId = 400, DeptName = "Cross", Capacity = 45 }
                    );
            });
        }
    }
}
