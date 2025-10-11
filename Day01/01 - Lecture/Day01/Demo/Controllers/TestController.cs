using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers
{
    public class TestController : Controller
    {
        public string Display()
        {
            return $"Hello From Display() Inside TestController!";
        }
        public ViewResult ShowAddForm()
        {
            return View();
        }
        public int Add(int? x, int y)
        {
            return (x ?? 0) + y;
        } 
    }
}
