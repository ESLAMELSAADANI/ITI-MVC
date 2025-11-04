using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ModelsLayer;
using System.Threading.Tasks;

namespace Demo.Controllers
{
    public class RoleController : Controller
    {
        RoleManager<ApplicationRole> roleManager;

        public RoleController(RoleManager<ApplicationRole> _roleManager)
        {
            roleManager = _roleManager;
        }

        public IActionResult Index()
        {
            var model = roleManager.Roles.ToList();
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
                return BadRequest();
            var model = await roleManager.FindByIdAsync(id);
            if (model == null)
                return NotFound();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(ApplicationRole role)
        {
            var roleExist = await roleManager.FindByNameAsync(role.Name);
            if (roleExist != null && roleExist.Id != role.Id)
                ModelState.AddModelError("Name", "Role Already Exists!");

            if (ModelState.IsValid)
            {
                //roleRepo.Update(role);//Conflict In Tracking
                var existingRole = await roleManager.FindByIdAsync(role.Id);
                existingRole.Name = role.Name;
                var result = await roleManager.UpdateAsync(existingRole);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(existingRole);
                }
                return RedirectToAction("index");
            }
            var model = await roleManager.FindByIdAsync(role.Id);
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Add()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Add(ApplicationRole model)
        {
            var ExistRole = await roleManager.FindByNameAsync(model.Name);
            if (ExistRole != null)
                ModelState.AddModelError("Name", "Role Already Exists!");
            if (ModelState.IsValid)
            {
                var result = await roleManager.CreateAsync(model);
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
            return View();
        }
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
                return BadRequest();
            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
                return NotFound();
            var res =  await roleManager.DeleteAsync(role);
            if (!res.Succeeded)
            {
                foreach (var error in res.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                    return View("Index");
            }
            return RedirectToAction("index");
        }
        public async Task<IActionResult> RoleExist([FromQuery(Name = "Name")] string roleName, [FromQuery(Name = "id")] string id)
        {
            var roleExist = await roleManager.FindByNameAsync(roleName);
            if (roleExist == null)
                return Json(true);
            if (roleExist.Id == id)
                return Json(true);
            return Json(false);
        }
    }
}
