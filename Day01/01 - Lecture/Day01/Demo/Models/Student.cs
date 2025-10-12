using System.ComponentModel.DataAnnotations.Schema;

namespace Demo.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }

        [ForeignKey("Department")]
        public int DeptNo { get; set; }
        public Department Department { get; set; }

        public override string ToString()
        {
            return $"Id: {Id} - Name: {Name} - Age: {Age}";
        }
    }
}
