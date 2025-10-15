using Demo.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Demo.ViewModels
{
    public class StudentDepartment
    {
        public Student Student { get; set; }
        public SelectList DepartmentsSelectList { get; set; }
    }
}
