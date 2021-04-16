using Lms.Core.Entities;
using Lms.Core.Repositories;
using Lms.Data.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Data.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly LmsApiContext db;

        public CourseRepository(LmsApiContext db)
        {
            this.db = db;
        }
        public async Task<IEnumerable<Course>> GetAllCourses(bool includeModules) 
        {
            if (includeModules)
                return await db.Course.Include(m => m.Modules).ToListAsync();
            else
                return await db.Course.ToListAsync();
        }
        public async Task<Course> GetCourse(int? Id, bool includeModules)
        {
            if (includeModules)
                return await db.Course.Include(m => m.Modules).FirstOrDefaultAsync(m => m.Id == Id);
            else
                return await db.Course
                .FirstOrDefaultAsync(m => m.Id == Id);
        }
        public bool Any(int? Id)
        {
            return db.Course.Any(m => m.Id == Id);
        }
        public async Task<bool> SaveAsync()
        {
            return (await db.SaveChangesAsync()) >= 0;
        }
        public async Task AddAsync<Course>(Course course)
        {
            await db.AddAsync(course);
        }

        public void Remove(Course course)
        {
            db.Remove(course);
        }
    }
}
