using Demo.DAL;
using Demo.Repos;
using Microsoft.EntityFrameworkCore;
using ModelsLayer.Models;

namespace Demo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<IEntityRepo<Department>, DepartmentRepo>();
            builder.Services.AddScoped<IEntityRepo<Student>, StudentRepo>();
            builder.Services.AddScoped<IEmailExist, StudentRepo>();
            builder.Services.AddScoped<IIdExist, DepartmentRepo>();
           
            //===== Use Default Constructor Of ITIDbContext When Make object from it and use OnConfiguring() of it which define the connectionString =======
            //builder.Services.AddScoped<ITIDbContext, ITIDbContext>();
            //builder.Services.AddScoped<ITIDbContext>();//Also Can Write This if the name of key same as value.

            //===== Use Parametrized Constructor Of ITIDbContext [ITIDbContext(DbContextOptions options)]When Make object from it and define the connection string and type of DB provider through [options] parameter=======
            //===== Life time of this service is scoped[Default] =======
            builder.Services.AddDbContext<ITIDbContext>(options =>
            {
                //options.UseSqlServer("Data Source=.;Initial Catalog=ITIMVC;Integrated Security=True;Trust Server Certificate=True");
                options.UseSqlServer(builder.Configuration.GetConnectionString("ITIMVC_Conn"));
            }/*,ServiceLifetime.Singleton*/);
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=student}/{action=index}/{id:int?}");//Start With ControllerName
                //pattern: "{action=index}/{controller=student}/{id:int?}");//Start With ActionName
                //pattern: "ITI/{controller=student}/{action=index}/{id:int?}");//Start URL with static value

            app.Run();
        }
    }
}
