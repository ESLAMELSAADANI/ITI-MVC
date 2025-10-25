using Demo.Repos;
using Microsoft.AspNetCore.Mvc;
using ModelsLayer;

namespace Demo.Controllers
{
    public class CourseController : Controller
    {
        IEntityRepo<Course> courseRepo;

        public CourseController(IEntityRepo<Course> _courseRepo)
        {
            courseRepo = _courseRepo;
        }

        public IActionResult Index()
        {
            var model = courseRepo.GetAll();
            return View(model);
        }
        public IActionResult Details(int? id)
        {
            if (id == null)
                return BadRequest();
            var course = courseRepo.Details(id.Value);
            if (course == null)
                return NotFound();
            return View(course);
        }
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return NotFound();
            var course = courseRepo.Get(id.Value);
            if (course == null)
                return NotFound();
            return View(course);
        }
        [HttpPost]
        public IActionResult Edit(Course course)
        {
            if (ModelState.IsValid)
            {
                courseRepo.Update(course);
                courseRepo.Save();
                return RedirectToAction("index");
            }
            var ccourse = courseRepo.Get(course.Id);
            return View(ccourse);
        }
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
                return BadRequest();
            var course = courseRepo.Get(id.Value);
            if (course == null)
                return NotFound();
            courseRepo.Delete(id.Value);
            courseRepo.Save();
            return RedirectToAction("index");
        }
        [HttpPost]
        public IActionResult Delete(Course course)
        {
            courseRepo.Delete(course.Id);
            courseRepo.Save();
            return RedirectToAction("index");
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Add(Course course)
        {
            if (ModelState.IsValid)
            {
                courseRepo.Insert(course);
                courseRepo.Save();
                return RedirectToAction("index");
            }
            return View();
        }
    }
}
