using ModelsLayer.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace ModelsLayer.Models
{
    public class Student
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please Enter Your Name!")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name Must be less than 50 chars and grater than 3 chars")]//Max Validation mapped to DB, min validation not mapped
        public string Name { get; set; }
        [Range(10, 50, ErrorMessage = "Age must be between 10 and 50 years.")]//just validation in App, Not mapped to DB
        public int Age { get; set; }
        [RegularExpression("[a-zA-Z0-9_]+@[a-zA-Z]+.[a-zA-Z]{2,4}", ErrorMessage = "Enter Valid Email!")]//Validation Not Applied To DB - Validation For Application
        [Remote("EmailExist", "Student",AdditionalFields ="Student.Id", ErrorMessage = "This Email Exist In DB!"), Required(ErrorMessage = "Email Is Required!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required!"), MinLength(6, ErrorMessage = "Password Can't be less than 6")]
        public string Password { get; set; }
        [NotMapped, Compare("Password", ErrorMessage = "Not Match!")]
        public string ConfirmPassword { get; set; }
        [ForeignKey("Department")]
        [Required(ErrorMessage = "Please select a department!")]
        public int DeptNo { get; set; }
        public Department? Department { get; set; }
        public List<StudentCourse> StudentCourses { get; set; } = new List<StudentCourse>();

        [ForeignKey("User")]
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }
        public override string ToString()
        {
            return $"Id: {Id} - Name: {Name} - Age: {Age}";
        }
    }
}
