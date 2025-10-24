using Demo.DAL;
using ModelsLayer;

namespace Demo.Repos
{
    public class CourseRepo : IEntityRepo<Course>
    {
        ITIDbContext dbContext;

        public CourseRepo(ITIDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public void Delete(int id)
        {
            var course = dbContext.Courses.SingleOrDefault(c => c.Id == id);
            dbContext.Courses.Remove(course);
        }

        public Course Details(int id)
        {
            return dbContext.Courses.SingleOrDefault(c => c.Id == id);
        }

        public Course Get(int id)
        {
            return dbContext.Courses.SingleOrDefault(c => c.Id == id);
        }

        public List<Course> GetAll()
        {
            return dbContext.Courses.ToList();
        }

        public Course Insert(Course entity)
        {
            dbContext.Courses.Add(entity);
            return entity;
        }

        public int Save()
        {
            return dbContext.SaveChanges();
        }

        public Course Update(Course entity)
        {
            dbContext.Courses.Update(entity);
            return entity;
        }
    }
}
