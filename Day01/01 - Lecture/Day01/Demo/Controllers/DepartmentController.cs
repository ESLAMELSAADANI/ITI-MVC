using Demo.DAL;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers
{
    public class DepartmentController : Controller
    {
        ITIDbContext dbContext = new ITIDbContext();
        public IActionResult Index()
        {
            var model = dbContext.Department.ToList();
            return View(model);
        }
    }
}
