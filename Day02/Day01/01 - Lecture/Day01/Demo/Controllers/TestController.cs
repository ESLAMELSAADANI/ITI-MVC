using Demo.Models;
using Demo.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers
{
    public class TestController : Controller
    {
        //Domain:Port/Test/Display
        public string Display()
        {
            return $"Hello From Display() Inside TestController!";
        }
        //Domain:Port/Test/ShowAddForm
        public ViewResult ShowAddForm()
        {
            return View();
        }
        //Domain:Port/Test/Add?x={value}&y={value}
        public int Add(int? x, int y)
        {
            return (x ?? 0) + y;
        }
        //Domain:Port/Test/Details
        public ViewResult Details()
        {
            int x = 150;
            string name = "Eslam";
            List<string> names = new List<string>() { "ali", "eslam", "sara", "fatma" };
            Student student = new Student() { Id = 10, Name = "Eslam", Age = 23 };
            List<Student> students = new List<Student>()
            {
                new Student(){ Id = 10, Name = "Ahmed", Age = 23 },
                new Student(){ Id = 20, Name = "Hanaa", Age = 25 },
                new Student(){ Id = 30, Name = "Sama", Age = 27 },
                new Student(){ Id = 40, Name = "Ashraf", Age = 14 },
                new Student(){ Id = 50, Name = "Randa", Age = 15 },
                new Student(){ Id = 60, Name = "Heba", Age = 33 },
            };

            //Use ViewData Collection (Key Value Pair) To send data from Controller to view
            //The Type of returned data is object.
            //ViewData["fname"] = name;
            //ViewData["x"] = x;
            //ViewData["student"] = student;
            //ViewData["students"] = students;

            //Use ViewBag Collection (Key Value Pair) To send data from Controller to view
            //The Type of returned data is dynamic => know type in runtime.
            //ViewBag.instructor = "Ahmed";
            //ViewBag.fname = name;
            //ViewBag.student = student;
            //ViewBag.students = students;

            //ViewBag is wrapper on ViewData
            //Mean That anything added in ViewData will add in ViewBag automatic and viceversa
            //
            //ViewData["Course"] = "Python";
            //@ViewBag.Course -> Python
            //
            //ViewBag.Instructor = "Eslam";
            //ViewData["instructor"] -> Eslam
            //
            //And when read data using ViewBag it will read it from ViewData as of type "dynamic"
            //Using Model
            HomoginuousTypesVM typesVM = new HomoginuousTypesVM()
            {
                name = name,
                names = names,
                student = student,
                students = students,
                x = x
            };
            return View(typesVM);
        }
    }
}
