using Demo.DAL;
using ModelsLayer.Models;
using Demo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Buffers;
using Microsoft.EntityFrameworkCore;
using Demo.Repos;

namespace Demo.Controllers
{
    public class StudentController : Controller
    {
        //ITIDbContext dbContext = new ITIDbContext();
        IEntityRepo<Student> studentRepo = new StudentRepo();
        IEntityRepo<Department> departmentRepo = new DepartmentRepo();
        public IActionResult Index()
        {
            //var students = dbContext.Students.Include(s => s.Department).ToList();//Load Related Data Through Navigational Property(EagerLoading) => Load Data Of Department also To Use It Inside View
            var students = studentRepo.GetAll();
            studentRepo.Dispose();
            return View(students);
        }
        public IActionResult Details(int? id)
        {
            if (id == null)
                return BadRequest();
            var student = studentRepo.Details(id.Value);
            if (student == null)
            {
                studentRepo.Dispose();
                return NotFound();
            }
            studentRepo.Dispose();
            return View(student);
        }
        [HttpGet]
        public IActionResult Add()
        {
            //ViewBag.depts = dbContext.Department.ToList();
            //var depts = dbContext.Department.ToList();
            var depts = departmentRepo.GetAll();
            StudentDepartment studentDepartment = new StudentDepartment()
            {
                Student = new Student(),
                Departments = departmentRepo.GetAll()
            };
            departmentRepo.Dispose();
            return View(studentDepartment);
        }
        [HttpPost]
        public IActionResult Add(Student student)
        {
            //dbContext.Students.Add(student);
            //dbContext.SaveChanges();
            //return RedirectToAction("index");

            //==== Repository Design Pattern =========

            studentRepo.Insert(student);
            //studentRepo.Update(student);
            //studentRepo.Delete(student.Id);
            studentRepo.Save();
            studentRepo.Dispose();
            return RedirectToAction("index");
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return BadRequest();
            //var student = dbContext.Students.SingleOrDefault(s => s.Id == id);
            var student = studentRepo.Get(id.Value);
            if (student == null)
            {
                studentRepo.Dispose();
                return NotFound();
            }
            var depts = departmentRepo.GetAll();
            StudentDepartment studentDepartment = new StudentDepartment()
            {
                Student = student,
                Departments = departmentRepo.GetAll()
            };
            departmentRepo.Dispose();
            return View(studentDepartment);
        }
        [HttpPost]
        public IActionResult Edit(Student student)
        {
            //dbContext.Students.Update(student);
            //dbContext.SaveChanges();
            //return RedirectToAction("index");

            //===== Repository Design Pattern =======

            studentRepo.Update(student);
            studentRepo.Save();
            studentRepo.Dispose();
            return RedirectToAction("index");
        }
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
                return BadRequest();
            //var student = dbContext.Students.SingleOrDefault(s => s.Id == id);
            var student = studentRepo.Get(id.Value);
            if (student == null)
            {
                studentRepo.Dispose();
                return NotFound();
            }
            //dbContext.Students.Remove(student);
            //dbContext.SaveChanges();
            //return RedirectToAction("index");

            //====== Repository Pattern ======

            studentRepo.Delete(id.Value);
            studentRepo.Save();
            studentRepo.Dispose();
            return RedirectToAction("index");
        }
        [HttpPost]
        public IActionResult Delete(StudentDepartment stdDept)
        {
            //dbContext.Students.Remove(stdDept.Student);
            //dbContext.SaveChanges();
            //return RedirectToAction("index");

            //====== Repository Pattern ========
            studentRepo.Delete(stdDept.Student.Id);
            studentRepo.Save();
            studentRepo.Dispose();
            return RedirectToAction("index");
        }
    }
}
