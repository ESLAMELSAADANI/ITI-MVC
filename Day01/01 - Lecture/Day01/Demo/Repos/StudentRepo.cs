using Demo.DAL;
using Microsoft.EntityFrameworkCore;
using ModelsLayer.Models;

namespace Demo.Repos
{
    public interface IStudentRepoExtra
    {
        bool IsEmailExist(string email, int id);
        Task<Student> GetStudentByUserIdAsync(string userId);
        Task<Student> GetStudentByEmailAsync(string email);
    }
    public class StudentRepo : IEntityRepo<Student>, IStudentRepoExtra
    {

        //ITIDbContext dbContext = new ITIDbContext();

        //====== Dependency Injection =======
        ITIDbContext dbContext;
        public StudentRepo(ITIDbContext _dbContext)////Constructor Injection [DIC Will Inject The Object Here]
        {
            dbContext = _dbContext;
        }

        public void Delete(int id)
        {
            //dbContext.Students.Where(s => s.Id == id).ExecuteDelete();
            var student = dbContext.Students.SingleOrDefault(s => s.Id == id);
            dbContext.Students.Remove(student);//Delete in App Memory
        }
        public Student Details(int id)
        {
            var student = dbContext.Students.Include(s => s.Department).SingleOrDefault(s => s.Id == id);
            return student;
        }
        //Not need to use it, bcz dependency injection automatically dispose object created after lifetime of it end
        //public void Dispose()
        //{
        //    dbContext.Dispose();
        //}
        public Student Get(int id)
        {
            return dbContext.Students.SingleOrDefault(s => s.Id == id);
        }
        public List<Student> GetAll()
        {
            return dbContext.Students.Include(s => s.Department).ToList();
        }

        public async Task<Student> GetStudentByEmailAsync(string email)
        {
            return await dbContext.Students.SingleOrDefaultAsync(s => s.Email == email);
        }

        public async Task<Student> GetStudentByUserIdAsync(string userId)
        {
            return await dbContext.Students.SingleOrDefaultAsync(s => s.UserId == userId);
        }

        public Student Insert(Student student)
        {
            dbContext.Students.Add(student);//Add In App Memory
            return student;
        }
        public bool IsEmailExist(string email, int id)
        {
            if (id == 0)
                return false;
            var existingStudent = dbContext.Students.SingleOrDefault(u => u.Email == email);
            if (existingStudent == null)
                return false;
            if (existingStudent.Id == id)
                return false;
            return true;
        }
        public int Save()
        {
            return dbContext.SaveChanges();
        }
        public Student Update(Student student)
        {
            var existing = dbContext.Students.FirstOrDefault(s => s.Id == student.Id);
            if (existing != null)
            {
                // update only the fields you allow editing
                existing.Name = student.Name;
                existing.Email = student.Email;
                existing.Age = student.Age;
                existing.Password = student.Password;
                existing.DeptNo = student.DeptNo;
                existing.Password = student.Password;
                existing.ConfirmPassword = student.ConfirmPassword;
            }
            return existing;
        }
    }
}
