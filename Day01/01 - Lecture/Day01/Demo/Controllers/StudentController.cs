using Demo.DAL;
using ModelsLayer.Models;
using Demo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Buffers;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Demo.Repos;

namespace Demo.Controllers
{
    public class StudentController : Controller
    {
        //ITIDbContext dbContext = new ITIDbContext();
        //===== Repository Design Pattern ========
        //IEntityRepo<Student> studentRepo = new StudentRepo();
        //IEntityRepo<Department> departmentRepo = new DepartmentRepo();
        //IEmailExist studentEmailExist = new StudentRepo();

        //===== Dependency Injection ========
        IEntityRepo<Student> studentRepo;
        IEntityRepo<Department> departmentRepo;
        IEmailExist studentEmailExist;

        public StudentController(IEntityRepo<Student> _studentRepo, IEntityRepo<Department> _departmentRepo, IEmailExist _studentEmailExist)
        {
            studentRepo = _studentRepo;
            departmentRepo = _departmentRepo;
            studentEmailExist = _studentEmailExist;
        }

        public IActionResult Index()
        {
            //int x = int.Parse("ssss");//Simulate there are exception to test Development and production Environment.
            //var students = dbContext.Students.Include(s => s.Department).ToList();//Load Related Data Through Navigational Property(EagerLoading) => Load Data Of Department also To Use It Inside View
            var students = studentRepo.GetAll();
            //studentRepo.Dispose();
            return View(students);
        }
        public IActionResult Details(int? id)
        {
            if (id == null)
                return BadRequest();
            var student = studentRepo.Details(id.Value);
            if (student == null)
            {
                //studentRepo.Dispose();
                return NotFound();
            }
            //studentRepo.Dispose();
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
            //departmentRepo.Dispose();
            return View(studentDepartment);
        }
        [HttpPost]
        public IActionResult Add(StudentDepartment studentDepartment)
        {
            //dbContext.Students.Add(student);
            //dbContext.SaveChanges();
            //return RedirectToAction("index");

            //==== Repository Design Pattern =========
            //==== Validation On student properties values ====
            //if (studentEmailExist.IsEmailExist(studentDepartment.Student.Email))
            //    ModelState.AddModelError("Email", "This email is already in use.");
            if (studentEmailExist.IsEmailExist(studentDepartment.Student.Email))
                ModelState.AddModelError("Student.Email", "This email is already in use.");

            if (ModelState.IsValid)
            {
                var hasher = new PasswordHasher<Student>();

                // Hash the password before saving it
                studentDepartment.Student.Password = hasher.HashPassword(studentDepartment.Student, studentDepartment.Student.Password);

                studentRepo.Insert(studentDepartment.Student);
                //studentRepo.Update(student);
                //studentRepo.Delete(student.Id);
                studentRepo.Save();
                //studentRepo.Dispose();
                return RedirectToAction("index");
            }

            studentDepartment.Departments = departmentRepo.GetAll();
            //departmentRepo.Dispose();
            return View(studentDepartment);

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
                //studentRepo.Dispose();
                return NotFound();
            }
            var depts = departmentRepo.GetAll();
            StudentDepartment studentDepartment = new StudentDepartment()
            {
                Student = student,
                Departments = depts
            };
            //departmentRepo.Dispose();
            return View(studentDepartment);
        }
        [HttpPost]
        public IActionResult Edit(Student student)
        {
            //dbContext.Students.Update(student);
            //dbContext.SaveChanges();
            //return RedirectToAction("index");

            //===== Repository Design Pattern =======
            if (studentEmailExist.IsEmailExist(student.Email))
                ModelState.AddModelError("Student.Email", "This email is already in use.");
            if (ModelState.IsValid)
            {
                var hasher = new PasswordHasher<Student>();
                // Hash the password before saving it
                student.Password = hasher.HashPassword(student, student.Password);

                studentRepo.Update(student);
                studentRepo.Save();
                //studentRepo.Dispose();
                return RedirectToAction("index");
            }
            var depts = departmentRepo.GetAll();
            StudentDepartment model = new StudentDepartment()
            {
                Student = student,
                Departments = depts

            };
            return View(model);
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
                //studentRepo.Dispose();
                return NotFound();
            }
            //dbContext.Students.Remove(student);
            //dbContext.SaveChanges();
            //return RedirectToAction("index");

            //====== Repository Pattern ======

            studentRepo.Delete(id.Value);
            studentRepo.Save();
            //studentRepo.Dispose();
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
            //studentRepo.Dispose();
            return RedirectToAction("index");
        }
        public IActionResult EmailExist([FromQuery(Name = "Student.Email")] string Student_Email)
        {
            bool isExist = studentEmailExist.IsEmailExist(Student_Email);
            if (isExist == true)
                return Json(false);
            else
                return Json(true);
        }
    }
}
