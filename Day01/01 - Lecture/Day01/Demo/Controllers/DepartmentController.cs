using Demo.DAL;
using ModelsLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers
{
    //[Route("hamada/{action=index}/{id:int:max(500)?}")]
    public class DepartmentController : Controller
    {
        ITIDbContext dbContext = new ITIDbContext();
        //[Route("hamada")]
        public IActionResult Index()
        {
            var model = dbContext.Department.ToList();
            return View(model);
        }
        //Show The Form Of Add New Department
        [HttpGet] //ActionSelector say that this action work with Get Request Only
        public IActionResult Create()
        {
            return View();
        }
        //Receive Data from request and save data to database.
        [HttpPost]
        public IActionResult Create(Department dept)
        {
            try
            {
                dbContext.Department.Add(dept);
                dbContext.SaveChanges();
                return RedirectToAction("index");
            }
            catch (Exception ex)
            {

                return View("exception", ex);
            }
        }

        // department/details/500   =>  500 will binded to the passed parameter id [Route System Behavior]
        public IActionResult Details(int? id)
        {
            if (id == null)//if user not enter value for id => /department/details
            {
                return BadRequest();
            }
            var dept = dbContext.Department.SingleOrDefault(d => d.DeptId == id);
            if (dept == null)//if user enter id not match any dept in DB => /department/details/88888
                return NotFound();
            return View(dept);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            
            if (id == null)//if user not enter value for id => /department/edit
            {
                return BadRequest();
            }
            var dept = dbContext.Department.SingleOrDefault(d => d.DeptId == id);
            if (dept == null)//if user enter id not match any dept in DB => /department/edit/88888
                return NotFound();
            return View(dept);
        }
        [HttpPost]
        public IActionResult Edit(Department dept)
        {
            try
            {
                dbContext.Department.Update(dept);
                dbContext.SaveChanges();
                return RedirectToAction("index");
            }
            catch (Exception ex)
            {

                return View("exception", ex);
            }
        }
        
        public IActionResult Delete(int? id)
        {
            try
            {
                var dept = dbContext.Department.SingleOrDefault(d => d.DeptId == id);
                dbContext.Department.Remove(dept);
                dbContext.SaveChanges();
                return RedirectToAction("index");
            }
            catch (Exception ex)
            {

                return View("exception", ex);
            }
        }
        [HttpPost]
        public IActionResult Delete(Department dept)
        {
            try
            {
                //var dept = dbContext.Department.SingleOrDefault(d => d.DeptId == id);
                dbContext.Department.Remove(dept);
                dbContext.SaveChanges();
                return RedirectToAction("index");
            }
            catch (Exception ex)
            {

                return View("exception", ex);
            }
        }
    }
}
