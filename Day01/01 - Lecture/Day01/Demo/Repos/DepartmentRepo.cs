using Demo.DAL;
using Microsoft.EntityFrameworkCore;
using ModelsLayer.Models;

namespace Demo.Repos
{
    public interface IDepartmentRepoExtra
    {
        bool IsIdExist(int id);
        Task<Department> GetFirstDeptAsync();
    }

    public class DepartmentRepo : IEntityRepo<Department>, IDepartmentRepoExtra
    {
        //====== Dependency Injection =======
        ITIDbContext dbContext;

        public DepartmentRepo(ITIDbContext _dbContext)//Constructor Injection [DIC Will Inject The Object Here]
        {
            dbContext = _dbContext;
        }
        
        public void Delete(int id)
        {
            var dept = dbContext.Department.Include(d => d.Students).SingleOrDefault(d => d.DeptId == id);
            if (dept.Students.Count == 0)
                dbContext.Department.Remove(dept);//Hard Delete From DB
            //Soft Delete
            else
            {
                dept.IsActive = false;//Soft Delete, Just Marked In DB As InActive
                Update(dept);
                var studentsInDept = dbContext.Students.Where(s => s.DeptNo == dept.DeptId);
                Save();
                var deptsActive = dbContext.Department.Where(d => d.IsActive != false);
                if (deptsActive.Count() == 0)
                    dbContext.Students.ExecuteDelete();
                else
                {
                    var randomDept = deptsActive.First();
                    foreach (var std in studentsInDept)
                    {
                        std.DeptNo = randomDept.DeptId;
                        dbContext.Students.Update(std);
                    }
                }

            }
        }
        public Department Details(int id)
        {
            var dept = dbContext.Department.SingleOrDefault(d => d.DeptId == id);
            return dept;
        }
        public Department Get(int id)
        {
            return dbContext.Department.Include(d => d.Courses).Include(d => d.Students).ThenInclude(d => d.StudentCourses).SingleOrDefault(d => d.DeptId == id);
        }
        public List<Department> GetAll()
        {
            return dbContext.Department.Where(d => d.IsActive == true).ToList();//Just Returned Active Depts Which Contain Students.
        }

        public Task<Department> GetFirstDeptAsync()
        {
            throw new NotImplementedException();
        }

        public Department Insert(Department department)
        {
            dbContext.Department.Add(department);
            return department;
        }
        public bool IsIdExist(int id)
        {
            return dbContext.Department.Any(d => d.DeptId == id);
        }
        public int Save()
        {
            return dbContext.SaveChanges();
        }
        public Department Update(Department department)
        {
            dbContext.Department.Update(department);
            return department;
        }
    }
}
