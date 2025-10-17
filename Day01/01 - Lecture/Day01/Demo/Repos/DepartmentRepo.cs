using Demo.DAL;
using Microsoft.EntityFrameworkCore;
using ModelsLayer.Models;

namespace Demo.Repos
{
    
    public class DepartmentRepo : IEntityRepo<Department>
    {
        ITIDbContext dbContext = new ITIDbContext();
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
            //dbContext.Remove(dept);
        }

        public Department Details(int id)
        {
            var dept = dbContext.Department.SingleOrDefault(d => d.DeptId == id);
            return dept;
        }

        public void Dispose()
        {
            dbContext.Dispose();
        }

        public Department Get(int id)
        {
            return dbContext.Department.SingleOrDefault(d => d.DeptId == id);
        }

        public List<Department> GetAll()
        {
            return dbContext.Department.Where(d => d.IsActive == true).ToList();//Just Returned Active Depts Which Contain Students.
        }

        public Department Insert(Department department)
        {

            //dbContext.Add(department);
            dbContext.Department.Add(department);
            return department;
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
