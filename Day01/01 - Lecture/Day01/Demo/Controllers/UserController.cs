using Demo.Repos;
using Demo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelsLayer;
using ModelsLayer.Models;
using System.Threading.Tasks;

namespace Demo.Controllers
{
    public class UserController : Controller
    {
        UserRepoExtra userRepoExtra;
        UserManager<ApplicationUser> userManager;
        RoleManager<ApplicationRole> roleManager;
        IStudentRepoExtra studentRepoExtra;
        IEntityRepo<Student> studentRepo;

        public UserController(UserRepoExtra _userRepoExtra, UserManager<ApplicationUser> _userManager, RoleManager<ApplicationRole> _roleManager, IStudentRepoExtra _studentRepoExtra, IEntityRepo<Student> _studentRepo)
        {
            userRepoExtra = _userRepoExtra;
            userManager = _userManager;
            roleManager = _roleManager;
            studentRepoExtra = _studentRepoExtra;
            studentRepo = _studentRepo;
        }

        public IActionResult Index()
        {
            var model = userManager.Users.ToList();
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var model = await userManager.FindByIdAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
                return BadRequest();
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            var userRoles = await userManager.GetRolesAsync(user);
            var model = new UserRoleVM()
            {
                User = user,
                RolesToDelete = userRoles.ToList()
            };

            return View(model);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(ApplicationUser model)
        {
            var user = await userManager.FindByIdAsync(model.Id);
            if (user == null)
                return NotFound();

            // Check for duplicate email
            var existingUser = await userManager.FindByEmailAsync(model.Email);
            if (existingUser != null && existingUser.Id != model.Id)
                ModelState.AddModelError("Email", "Email already exists!");

            if (ModelState.IsValid)
            {
                // Update properties
                user.Age = model.Age;
                user.Email = model.Email;
                user.UserName = model.UserName;
                user.NormalizedEmail = model.Email.ToUpper();
                user.NormalizedUserName = model.UserName.ToUpper();

                // Re-hash password if changed
                if (!string.IsNullOrEmpty(model.PasswordHash))
                {
                    var hasher = new PasswordHasher<ApplicationUser>();
                    user.PasswordHash = hasher.HashPassword(user, model.PasswordHash);
                }

                // Update user
                var result = await userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                    return View(model);
                }

                // Sync with Student if exists
                var student = await studentRepoExtra.GetStudentByUserIdAsync(user.Id);
                if (student != null)
                {
                    student.Age = user.Age;
                    student.Email = user.Email;
                    student.Password = user.PasswordHash;
                    student.Name = user.UserName;

                    studentRepo.Update(student);
                    studentRepo.Save();
                }

                return RedirectToAction("Index");
            }

            return View(model);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
                return BadRequest();
            var model = await userManager.FindByIdAsync(id);
            if (model == null)
                return NotFound();
            var student = await studentRepoExtra.GetStudentByEmailAsync(model.Email);
            if (student != null)
                studentRepo.Delete(student.Id);
            var result = await userManager.DeleteAsync(model);
            if (result.Succeeded)
                return RedirectToAction("index");
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View("index");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Add()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Add(ApplicationUser model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user != null)
                ModelState.AddModelError("Email", "Email Exists!");
            if (ModelState.IsValid)
            {
                var result = await userManager.CreateAsync(model, model.PasswordHash);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(model, "User");
                    return RedirectToAction("index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }
            return View();
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ViewRoles(string id)
        {
            if (id == null)
                return BadRequest();
            //var user = await userExtraRepo.GetUserByIdWithRolesAsync(id.Value);
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();
            var roles = roleManager.Roles.Select(r => r.Name).ToList();
            var userRoles = await userManager.GetRolesAsync(user);
            var otherRoles = roles.Except(userRoles);
            UserRoleVM userRoleVM = new UserRoleVM()
            {
                User = user,
                RolesToDelete = userRoles.ToList(),
                RolesToAdd = otherRoles.ToList()

            };
            return View(userRoleVM);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteSelectedRoles(UserRoleVM model)
        {
            var user = await userManager.FindByIdAsync(model.User.Id);
            if (user == null)
                return NotFound();
            if (model.RolesToDeleteNames != null && model.RolesToDeleteNames.Count > 0)
            {
                foreach (var roleName in model.RolesToDeleteNames)
                {
                    var result = await userManager.RemoveFromRoleAsync(user, roleName);
                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                            ModelState.AddModelError("", error.Description);
                    }
                }
                return RedirectToAction("ViewRoles", new { id = user.Id });
            }
            ModelState.AddModelError("", "Select Roles To Delete FromThis User!");
            var roles = roleManager.Roles.Select(r => r.Name).ToList();
            var rolesToDelete = await userManager.GetRolesAsync(user);
            var rolesToAdd = roles.Except(rolesToDelete);

            UserRoleVM updatedModel = new UserRoleVM()
            {
                User = user,
                RolesToDelete = rolesToDelete.ToList(),
                RolesToAdd = rolesToAdd.ToList()
            };
            return View("ViewRoles", updatedModel);

        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddSelectedRoles(UserRoleVM model)
        {
            var user = await userManager.FindByIdAsync(model.User.Id);
            if (user == null)
                return NotFound();
            if (model.RolesToAddNames != null && model.RolesToAddNames.Count > 0)
            {
                foreach (var roleName in model.RolesToAddNames)
                {
                    if (roleName == "Student")
                    {
                        Student std = new Student()
                        {
                            Name = user.UserName,
                            Age = user.Age,
                            Email = user.Email,
                            DeptNo = 100,
                            Password = user.PasswordHash,
                            UserId = user.Id
                        };
                        studentRepo.Insert(std);
                        studentRepo.Save();
                    }

                    var result = await userManager.AddToRoleAsync(user, roleName);
                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                            ModelState.AddModelError("", error.Description);
                    }
                }
                return RedirectToAction("ViewRoles", new { id = user.Id });
            }
            ModelState.AddModelError("", "Select Roles To Add FromThis User!");
            var roles = roleManager.Roles.Select(r => r.Name).ToList();
            var rolesToDelete = await userManager.GetRolesAsync(user);
            var rolesToAdd = roles.Except(rolesToDelete);

            UserRoleVM updatedModel = new UserRoleVM()
            {
                User = user,
                RolesToDelete = rolesToDelete.ToList(),
                RolesToAdd = rolesToAdd.ToList()
            };
            return View("ViewRoles", updatedModel);

        }

        public async Task<IActionResult> EmailExist(string email, string id)
        {
            var emailExist = await userRepoExtra.IsEmailExistAsync(email, id);
            return Json(!emailExist);
        }
    }
}
