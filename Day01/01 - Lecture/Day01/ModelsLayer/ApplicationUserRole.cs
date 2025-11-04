using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsLayer
{
    public class ApplicationUserRole:IdentityUserRole<string>
    {
        [Required]
        public override string UserId { get => base.UserId; set => base.UserId = value; }
        [ Required]
        public override string RoleId { get => base.RoleId; set => base.RoleId = value; }

        //[ForeignKey("User"), Required]
        //public int UserId { get; set; }
        //[ForeignKey("Role"), Required]
        //public int RoleId { get; set; }

        //public User User { get; set; }
        //public Role Role { get; set; }
    }
}
