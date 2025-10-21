using ModelsLayer.Models;

namespace Demo.Repos
{
    public class StudentMock : IEntityRepo<Student>
    {
        List<Student> students = new List<Student>()
        {
            new Student(){Id = 10,Name = "Eslam Elsaadany",Email = "eslam@gmail.com",Age = 23,DeptNo=500,Password = "123456"},
            new Student(){Id = 10,Name = "Eslam Elsaadany",Email = "eslam@gmail.com",Age = 23,DeptNo=500,Password = "123456"},
            new Student(){Id = 10,Name = "Eslam Elsaadany",Email = "eslam@gmail.com",Age = 23,DeptNo=500,Password = "123456"},
            new Student(){Id = 10,Name = "Eslam Elsaadany",Email = "eslam@gmail.com",Age = 23,DeptNo=500,Password = "123456"},
            new Student(){Id = 10,Name = "Eslam Elsaadany",Email = "eslam@gmail.com",Age = 23,DeptNo=500,Password = "123456"},
            new Student(){Id = 10,Name = "Eslam Elsaadany",Email = "eslam@gmail.com",Age = 23,DeptNo=500,Password = "123456"},
        };
        public void Delete(int id)
        {
            var student = students.SingleOrDefault(s => s.Id == id);
            students.Remove(student);
        }

        public Student Details(int id)
        {
            var student = students.SingleOrDefault(s => s.Id == id);
            return student;
        }

        public Student Get(int id)
        {
            var student = students.SingleOrDefault(s => s.Id == id);
            return student;
        }

        public List<Student> GetAll()
        {
            return students;
        }

        public Student Insert(Student student)
        {
            students.Add(student);
            return student;
        }

        public int Save()
        {
            throw new NotImplementedException();
        }

        public Student Update(Student department)
        {
            throw new NotImplementedException();
        }
    }
}
