using ModelsLayer.Models;
using Microsoft.EntityFrameworkCore;
using ModelsLayer;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Demo.DAL
{
    public class ITIDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, IdentityUserClaim<string>, ApplicationUserRole, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
    {

        public DbSet<Student> Students { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }
        //protected ITIDbContext()
        //{
        //}

        public ITIDbContext(DbContextOptions options) : base(options)
        {

        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=ITIMVC;Integrated Security=True;Trust Server Certificate=True");
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Course>(c =>
            {
                c.HasData(
                    new Course() { Id = 1, CrsName = "OS", CrsDuration = 120 },
                    new Course() { Id = 2, CrsName = "Network", CrsDuration = 100 },
                    new Course() { Id = 3, CrsName = "OOP", CrsDuration = 200 },
                    new Course() { Id = 4, CrsName = "LINQ", CrsDuration = 150 },
                    new Course() { Id = 5, CrsName = "DS", CrsDuration = 170 }
                    );
            });
            modelBuilder.Entity<Department>(d =>
            {
                d.HasData(
                    new Department() { DeptId = 100, DeptName = "CS", Capacity = 50 },
                    new Department() { DeptId = 200, DeptName = "Cyber", Capacity = 25 },
                    new Department() { DeptId = 300, DeptName = "Java", Capacity = 30 },
                    new Department() { DeptId = 400, DeptName = "Cross", Capacity = 45 }
                    );
            });
            //==== Seed Roles =====
            var adminRoleId = "f69a910e-11da-4b5e-a9bf-c18f189a7c18";
            var userRoleId = "d58d3875-d822-4283-8537-21c965172bf9";
            var instructorRoleId = "7add2ebe-4eec-4a32-a0de-701f06774bc9";
            var studentRoleId = "6f781404-8f89-4699-93ea-415a714f1d8c";
            var roles = new List<ApplicationRole>()
            {
                new ApplicationRole(){Id = adminRoleId,Name = "Admin",NormalizedName="Admin".ToUpper()},
                new ApplicationRole(){Id = studentRoleId,Name = "Student",NormalizedName="Student".ToUpper()},
                new ApplicationRole(){Id = userRoleId,Name = "User",NormalizedName="User".ToUpper()},
                new ApplicationRole(){Id = instructorRoleId,Name = "Instructor",NormalizedName="Instructor".ToUpper()}
            };
            modelBuilder.Entity<ApplicationRole>(r =>
            {
                r.HasData(roles);
            });

            //==== Seed user as Admin =====
            var useId = "a6797cb1-bac8-4c0e-9bf6-e29b6e74dfc2";
            ApplicationUser user = new ApplicationUser()
            {
                Id = useId,
                UserName = "Eslam Elsaadany",
                NormalizedUserName = "ESLAM ELSAADANY",
                Email = "eslam@gmail.com",
                NormalizedEmail = "eslam@gmail.com".ToUpper(),
                Age = 23,
                PasswordHash = "AQAAAAIAAYagAAAAEKHr+lr2tsvTe8ijmGJqPIUpruCb2HRoxXcYEnOvAtgcshi89rBpcwf6n6hx5kxpKQ==",//Admin@123
                SecurityStamp = "STATIC-SECURITY-STAMP",
                ConcurrencyStamp = "STATIC-CONCURRENCY-STAMP"

            };
            modelBuilder.Entity<ApplicationUser>(u =>
            {
                u.HasData(user);
            });

            modelBuilder.Entity<ApplicationUserRole>(ur =>
            {
                ur.HasData(
                    new ApplicationUserRole()
                    {
                        UserId = user.Id,
                        RoleId = adminRoleId
                    }
                    );
            });
        }
    }
}
