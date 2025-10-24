using Demo.DAL;
using Microsoft.EntityFrameworkCore;
using ModelsLayer.Models;

namespace Demo.Repos
{
    public interface IEmailExist
    {
        bool IsEmailExist(string email);
    }
    public class StudentRepo : IEntityRepo<Student>,IEmailExist
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
        public Student Insert(Student student)
        {
            dbContext.Students.Add(student);//Add In App Memory
            return student;
        }
        public bool IsEmailExist(string email)
        {
            return dbContext.Students.Any(s => s.Email == email);
        }
        public int Save()
        {
            return dbContext.SaveChanges();
        }
        public Student Update(Student student)
        {
            dbContext.Students.Update(student);
            return student;
        }
        //public void EditStudentsDept(int deptId)
        //{
        //    foreach (var student in db.Students)
        //    {
        //        student.DeptNo = randomDept.DeptId;
        //        dbContext.Students.Update(student);
        //    }
        //}
    }
}
