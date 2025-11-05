using Demo.DAL;
using ModelsLayer.Models;
using Demo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Buffers;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Demo.Repos;
using Microsoft.AspNetCore.Authorization;
using ModelsLayer;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace Demo.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        //===== Dependency Injection ========
        IEntityRepo<Student> studentRepo;
        IEntityRepo<Department> departmentRepo;
        IStudentRepoExtra studentRepoExtra;
        UserManager<ApplicationUser> userManager;

        public StudentController(IEntityRepo<Student> _studentRepo, IEntityRepo<Department> _departmentRepo, IStudentRepoExtra _studentRepoExtra, UserManager<ApplicationUser> _userManager)
        {
            studentRepo = _studentRepo;
            departmentRepo = _departmentRepo;
            studentRepoExtra = _studentRepoExtra;
            userManager = _userManager;
        }
        public IActionResult Index()
        {
            var students = studentRepo.GetAll();
            return View(students);
        }
        public IActionResult Details(int? id)
        {
            if (id == null)
                return BadRequest();
            var student = studentRepo.Details(id.Value);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Add()
        {
            var depts = departmentRepo.GetAll();
            StudentDepartment studentDepartment = new StudentDepartment()
            {
                Student = new Student(),
                Departments = departmentRepo.GetAll()
            };
            return View(studentDepartment);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Add(StudentDepartment studentDepartment)
        {
            var existingUserUserName = await userManager.FindByNameAsync(studentDepartment.Student.Name);
            if (existingUserUserName != null)
            {
                ModelState.AddModelError("Student.Name", "UserName Exists!");
            }
            var existingUserEmail = await userManager.FindByEmailAsync(studentDepartment.Student.Email);
            if (existingUserEmail != null)
            {
                ModelState.AddModelError("Student.Email", "Email Exists!");
            }
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser()
                {
                    UserName = studentDepartment.Student.Name,
                    Age = studentDepartment.Student.Age,
                    Email = studentDepartment.Student.Email,

                };
                var createResult = await userManager.CreateAsync(user, studentDepartment.Student.Password);
                if (!createResult.Succeeded)
                {
                    foreach (var error in createResult.Errors)
                        ModelState.AddModelError("", error.Description);

                    var deptss = departmentRepo.GetAll();
                    studentDepartment.Departments = deptss.ToList();
                    return View(studentDepartment);
                }
                studentDepartment.Student.UserId = user.Id;

                studentRepo.Insert(studentDepartment.Student);
                studentRepo.Save();

                await userManager.AddToRolesAsync(user, new List<string>() { "User", "Student" });

                return RedirectToAction("index");
            }

            var depts = departmentRepo.GetAll();
            studentDepartment.Departments = depts.ToList();

            return View(studentDepartment);

        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return BadRequest();
            var student = studentRepo.Get(id.Value);
            if (student == null)
            {
                return NotFound();
            }
            var depts = departmentRepo.GetAll();
            StudentDepartment studentDepartment = new StudentDepartment()
            {
                Student = student,
                Departments = depts
            };
            return View(studentDepartment);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(Student student)
        {
            var existingStudent = studentRepo.Get(student.Id);
            //===== Repository Design Pattern =======
            if (existingStudent != null && existingStudent.Id != student.Id)
                ModelState.AddModelError("Student.Email", "This email is already in use.");
            if (ModelState.IsValid)
            {
                var hasher = new PasswordHasher<Student>();
                // Hash the password before saving it
                student.Password = hasher.HashPassword(student, student.Password);

                studentRepo.Update(student);
                studentRepo.Save();
                //Update Associated User
                var user = await userManager.FindByEmailAsync(student.Email);
                if (user != null)
                {
                    user.UserName = student.Name;
                    user.Email = student.Email;
                    user.Age = student.Age;
                    user.NormalizedEmail = student.Email.ToUpper();
                    user.NormalizedUserName = student.Name.ToUpper();
                    user.PasswordHash = student.Password;
                    user.ConfirmPassword = student.ConfirmPassword;
                    var result = await userManager.UpdateAsync(user);
                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                        return View();
                    }
                    return RedirectToAction("index");
                }
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
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return BadRequest();
            var student = studentRepo.Get(id.Value);
            if (student == null)
            {
                return NotFound();
            }

            //====== Repository Pattern ======
            var user = await userManager.FindByEmailAsync(student.Email);
            if (user != null)
                await userManager.RemoveFromRoleAsync(user, "Student");
            studentRepo.Delete(id.Value);
            studentRepo.Save();
            return RedirectToAction("index");
        }
        [AllowAnonymous]
        public IActionResult EmailExist([FromQuery(Name = "Student.Email")] string Student_Email, [FromQuery(Name = "Student.Id")] int id)
        {
            bool isExist = studentRepoExtra.IsEmailExist(Student_Email, id);
            if (isExist == true)
                return Json(false);
            else
                return Json(true);
        }
    }
}
