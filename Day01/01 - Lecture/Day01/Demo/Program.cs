namespace Demo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

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
