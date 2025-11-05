using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ModelsLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLayer
{
    public class ApplicationUser:IdentityUser
    {
        [Required, StringLength(60)]
        public override string? UserName { get => base.UserName; set => base.UserName = value; }
        [Required, EmailAddress, RegularExpression(@"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$", ErrorMessage = "Please enter a valid email address.")]
        [Remote("EmailExist", "user", AdditionalFields = "Id", ErrorMessage = "Email Exists!")]
        public override string? Email { get => base.Email; set => base.Email = value; }
        [Required, Range(10, 60)]
        public int Age { get; set; }
        [Required(ErrorMessage = "The Password Field Is Required.")]
        public override string? PasswordHash { get => base.PasswordHash; set => base.PasswordHash = value; }
        [NotMapped, Compare("PasswordHash", ErrorMessage = "Don't Match!")]
        public string ConfirmPassword { get; set; }

        public Student? Student { get; set; }
    }
}
