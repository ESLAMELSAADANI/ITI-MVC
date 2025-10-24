using System.ComponentModel.DataAnnotations;

namespace Demo.ViewModels
{
    public class StudentDegreeVM
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        [Range(0, 100, ErrorMessage = "Degree must be between 0 and 100.")]
        public int? Degree { get; set; }
    }
}
