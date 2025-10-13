using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo.Models
{
    [PrimaryKey("DeptId")]
    public class Department
    {
        public int DeptId { get; set; }
        public string DeptName { get; set; }
        public int Capacity { get; set; }
        public List<Student> Students { get; set; } = new List<Student>();
        public override string ToString()
        {
            return $"DeptId= {DeptId} - DeptName= {DeptName} - Capacity= {Capacity}";
        }
    }
}
