using Demo.DAL;
using Microsoft.EntityFrameworkCore;
using ModelsLayer;

namespace Demo.Repos
{
    public interface IGetStudentCourse
    {
        public StudentCourse Get(int stdId, int crsId);
        public void Delete(StudentCourse sc);
    }
    public class StudentCourseRepo : IEntityRepo<StudentCourse>, IGetStudentCourse
    {
        ITIDbContext dbContext;
        public StudentCourseRepo(ITIDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
        public void Delete(StudentCourse sc)
        {
            dbContext.StudentCourses.Remove(sc);
        }
        public StudentCourse Details(int id)
        {
            throw new NotImplementedException();
        }
        public StudentCourse Get(int id)
        {
            throw new NotImplementedException();
        }
        public StudentCourse Get(int stdID, int crsID)
        {
            return dbContext.StudentCourses.SingleOrDefault(sc => sc.StudentId == stdID && sc.CourseId == crsID);
        }
        public List<StudentCourse> GetAll()
        {
            throw new NotImplementedException();
        }
        public StudentCourse Insert(StudentCourse entity)
        {
            var studentCourse = dbContext.StudentCourses.SingleOrDefault(sc => sc.StudentId == entity.StudentId && sc.CourseId == entity.CourseId);
            dbContext.StudentCourses.Add(entity);
            return studentCourse;
        }
        public int Save()
        {
            return dbContext.SaveChanges();
        }
        public StudentCourse Update(StudentCourse entity)
        {
            throw new NotImplementedException();
        }
    }
}
