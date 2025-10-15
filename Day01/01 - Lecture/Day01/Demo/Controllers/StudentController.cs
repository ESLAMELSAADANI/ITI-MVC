using Demo.DAL;
using Demo.Models;
using Demo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Buffers;
using Microsoft.EntityFrameworkCore;

namespace Demo.Controllers
{
    public class StudentController : Controller
    {
        ITIDbContext dbContext = new ITIDbContext();
        public IActionResult Index()
        {
            var students = dbContext.Students.Include(s => s.Department).ToList();//Load Related Data Through Navigational Property(EagerLoading) => Load Data Of Department also To Use It Inside View
            return View(students);
        }
        public IActionResult Details(int? id)
        {
            var student = dbContext.Students.Include(s => s.Department).SingleOrDefault(s => s.Id == id);
            if (student == null)
                return NotFound();
            return View(student);
        }
        [HttpGet]
        public IActionResult Add()
        {
            //ViewBag.depts = dbContext.Department.ToList();
            var depts = dbContext.Department.ToList();
            StudentDepartment studentDepartment = new StudentDepartment()
            {
                Student = new Student(),
                Departments = dbContext.Department.ToList()
            };

            return View(studentDepartment);
        }
        [HttpPost]
        public IActionResult Add(Student student)
        {
            dbContext.Students.Add(student);
            dbContext.SaveChanges();
            return RedirectToAction("index");
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return BadRequest();
            var student = dbContext.Students.SingleOrDefault(s => s.Id == id);
            if (student == null)
                return NotFound();
            var depts = dbContext.Department.ToList();
            StudentDepartment studentDepartment = new StudentDepartment()
            {
                Student = student,
                Departments = dbContext.Department.ToList()
            };
            return View(studentDepartment);
        }
        [HttpPost]
        public IActionResult Edit(Student student)
        {
            dbContext.Students.Update(student);
            dbContext.SaveChanges();
            return RedirectToAction("index");
        }
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
                return BadRequest();
            var student = dbContext.Students.SingleOrDefault(s => s.Id == id);
            if (student == null)
                return NotFound();
            dbContext.Students.Remove(student);
            dbContext.SaveChanges();
            return RedirectToAction("index");
        }
        [HttpPost]
        public IActionResult Delete(Student std)
        {
            dbContext.Students.Remove(std);
            dbContext.SaveChanges();
            return RedirectToAction("index");
        }
    }
}
