using Demo.DAL;
using ModelsLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Demo.Repos;
using Demo.ViewModels;
using ModelsLayer;

namespace Demo.Controllers
{
    //[Route("hamada/{action=index}/{id:int:max(500)?}")]
    //[Route("hamada/{action=index}/{id:int:max(500)?}")]
    public class DepartmentController : Controller
    {
        //ITIDbContext dbContext = new ITIDbContext();
        //===== Repository Design Pattern =======
        //IEntityRepo<Department> departmentRepo = new DepartmentRepo();
        //IIdExist departmentExist = new DepartmentRepo();

        //===== Dependency Injection ======
        IEntityRepo<Department> departmentRepo;
        IEntityRepo<Course> courseRepo;
        IEntityRepo<StudentCourse> studentCourseRepo;
        IGetStudentCourse StudentCourseRepoGet;
        IIdExist departmentExist;
        public DepartmentController(IEntityRepo<Department> _departmentRepo, IIdExist _departmentExist, IEntityRepo<Course> _courseRepo, IEntityRepo<StudentCourse> _studentCourseRepo, IGetStudentCourse _studentCourseRepoGet)
        {
            departmentRepo = _departmentRepo;
            courseRepo = _courseRepo;
            departmentExist = _departmentExist;
            studentCourseRepo = _studentCourseRepo;
            StudentCourseRepoGet = _studentCourseRepoGet;
        }

        //[Route("hamada")]
        public IActionResult Index(/*[FromServices]IEntityRepo<Department> _departmentRepo*/)
        {
            //var model = dbContext.Department.ToList();
            var model = departmentRepo.GetAll();
            //departmentRepo.Dispose();
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
                //dbContext.Department.Add(dept);
                //dbContext.SaveChanges();
                //return RedirectToAction("index");

                departmentRepo.Insert(dept);
                departmentRepo.Save();
                //departmentRepo.Dispose();
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
            //var dept = dbContext.Department.SingleOrDefault(d => d.DeptId == id);
            var dept = departmentRepo.Get(id.Value);
            if (dept == null)//if user enter id not match any dept in DB => /department/details/88888
            {
                //departmentRepo.Dispose();
                return NotFound();
            }
            //departmentRepo.Dispose();
            return View(dept);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)//if user not enter value for id => /department/edit
            {
                return BadRequest();
            }
            //var dept = dbContext.Department.SingleOrDefault(d => d.DeptId == id);
            var dept = departmentRepo.Get(id.Value);
            if (dept == null)//if user enter id not match any dept in DB => /department/edit/88888
            {
                //departmentRepo.Dispose();
                return NotFound();
            }
            //departmentRepo.Dispose();
            return View(dept);
        }
        [HttpPost]
        public IActionResult Edit(Department dept)
        {
            try
            {
                //dbContext.Department.Update(dept);
                //dbContext.SaveChanges();
                //return RedirectToAction("index");


                departmentRepo.Update(dept);
                departmentRepo.Save();
                //departmentRepo.Dispose();
                return RedirectToAction("index");
            }
            catch (Exception ex)
            {

                return View("exception", ex);
            }
        }

        //For View Department Courses Details and can delete course
        public IActionResult DepartmentCourses(int? id)
        {
            var department = departmentRepo.Get(id.Value);
            return View(department);
        }
        [HttpPost]
        public IActionResult AddDepartmentCourse(DepartmentCoursesVM dept, int courseID)
        {
            var department = departmentRepo.Get(dept.Department.DeptId);
            var course = courseRepo.Get(courseID);
            department.Courses.Add(course);
            departmentRepo.Save();
            return RedirectToAction("DepartmentCourses", new { id = dept.Department.DeptId });
        }
        public IActionResult DeleteDepartmentCourse(Department dept, int courseID)
        {
            var department = departmentRepo.Get(dept.DeptId);
            var course = department.Courses.SingleOrDefault(c => c.Id == courseID);
            department.Courses.Remove(course);
            departmentRepo.Save();
            return RedirectToAction("DepartmentCourses", new { id = dept.DeptId });
        }
        public IActionResult DepartmentAddCourses(int id)
        {
            var department = departmentRepo.Get(id);
            var coursesInDepartment = department.Courses.ToList();
            var allCourses = courseRepo.GetAll();
            var otherCourses = allCourses.Except(coursesInDepartment).ToList();

            DepartmentCoursesVM model = new DepartmentCoursesVM()
            {
                Department = department,
                DepartmentCourses = coursesInDepartment,
                OtherCourses = otherCourses
            };

            return View(model);
        }
        [HttpGet]
        public IActionResult EditDepartmentCourses(int? id)
        {
            var department = departmentRepo.Get(id.Value);
            var departmentCourses = department.Courses.ToList();
            var allCourses = courseRepo.GetAll();
            var otherCourses = allCourses.Except(departmentCourses).ToList();

            DepartmentCoursesVM model = new DepartmentCoursesVM()
            {
                Department = department,
                DepartmentCourses = departmentCourses,
                OtherCourses = otherCourses
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult EditDepartmentCourses(DepartmentCoursesVM model)
        {
            var dept = departmentRepo.Get(model.Department.DeptId);
            if (dept == null)
                return NotFound();

            if (model.CoursesToDelete.Count == 0 && model.CoursesToAdd.Count == 0)
                return RedirectToAction("editDepartmentCourses", new { id = dept.DeptId });
            if (model.CoursesToDelete != null)
            {
                foreach (var id in model.CoursesToDelete)
                {
                    var course = dept.Courses.SingleOrDefault(c => c.Id == id);
                    dept.Courses.Remove(course);
                }
            }
            if (model.CoursesToAdd != null)
            {
                foreach (var id in model.CoursesToAdd)
                {
                    var course = courseRepo.Get(id);
                    dept.Courses.Add(course);
                }
            }
            departmentRepo.Save();
            return RedirectToAction("DepartmentCourses", new { id = dept.DeptId });
        }
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
                return BadRequest();
            try
            {
                //var dept = dbContext.Department.SingleOrDefault(d => d.DeptId == id);
                //dbContext.Department.Remove(dept);
                //dbContext.SaveChanges();
                //return RedirectToAction("index");


                departmentRepo.Delete(id.Value);
                departmentRepo.Save();
                //departmentRepo.Dispose();
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
                ////var dept = dbContext.Department.SingleOrDefault(d => d.DeptId == id);
                //dbContext.Department.Remove(dept);
                //dbContext.SaveChanges();
                //return RedirectToAction("index");

                departmentRepo.Delete(dept.DeptId);
                departmentRepo.Save();
                //departmentRepo.Dispose();
                return RedirectToAction("index");
            }
            catch (Exception ex)
            {

                return View("exception", ex);
            }
        }

        public IActionResult IdExist(int DeptId)
        {
            bool exist = departmentExist.IsIdExist(DeptId);
            return Json(!exist);
        }

        public IActionResult ViewCourseStudents(int crsId, int deptID)
        {
            var department = departmentRepo.Get(deptID);
            var course = courseRepo.Get(crsId);

            var departmentCourseStudents = department.Students.Where(s => s.StudentCourses.Any(sc => sc.CourseId == crsId)).ToList();
            var otherStudents = department.Students.Where(s => !s.StudentCourses.Any(sc => sc.StudentId == s.Id)).ToList();


            var studentDegrees = departmentCourseStudents.Select(s => new StudentDegreeVM
            {
                StudentId = s.Id,
                CourseId = crsId,
                Degree = s.StudentCourses.Single(sc => sc.CourseId == crsId).Degree
            }).ToList();
            var otherStudentsDegrees = otherStudents.Select(s => new StudentDegreeVM
            {
                StudentId = s.Id,
                CourseId = crsId,
                Degree = null
            }).ToList();
            DepartmentCourseStudentsVM model = new DepartmentCourseStudentsVM()
            {
                Course = course,
                Department = department,
                StudentCourseDegrees = studentDegrees,
                OtherStudents = otherStudentsDegrees

            };

            return View(model);
        }
        [HttpPost]
        public IActionResult UpdateDegree(DepartmentCourseStudentsVM model)
        {
            foreach (var item in model.StudentCourseDegrees)
            {
                var studentCourse = StudentCourseRepoGet.Get(item.StudentId, item.CourseId);
                if (studentCourse != null)
                    studentCourse.Degree = item.Degree;
            }
            studentCourseRepo.Save();
            return RedirectToAction("ViewCourseStudents", new { crsId = model.Course.Id, deptID = model.Department.DeptId });
        }
        public IActionResult EnrollSelectedStudents(DepartmentCourseStudentsVM model)
        {
            if (model.SelectedStudentIds != null && model.SelectedStudentIds.Any())
            {
                for(int i=0;i< model.SelectedStudentIds.Count;i++ )
                {
                    // Add a StudentCourse record for each selected student
                    studentCourseRepo.Insert(new StudentCourse
                    {
                        StudentId = model.SelectedStudentIds[i],
                        CourseId = model.Course.Id,
                        Degree = model.OtherStudents[i].Degree
                    });
                }
                studentCourseRepo.Save();
            }

            // Redirect back to the same page to see updates
            return RedirectToAction("ViewCourseStudents", new { crsId = model.Course.Id, deptID = model.Department.DeptId });
        }
        public IActionResult DeleteStudentCourse(int stdID, int crsID, int deptId)
        {
            var studentCourse = StudentCourseRepoGet.Get(stdID, crsID);
            StudentCourseRepoGet.Delete(studentCourse);
            studentCourseRepo.Save();
            return RedirectToAction("ViewCourseStudents", new { crsId = crsID, deptID = deptId });
        }
    }
}
