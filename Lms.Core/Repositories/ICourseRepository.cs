using Lms.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Core.Repositories
{
    public interface ICourseRepository
    {
        Task<IEnumerable<Course>> GetAllCourses(bool includeModules);
        Task<Course> GetCourse(int? Id, bool includeModules);
        Task<bool> SaveAsync();
        Task AddAsync<T>(T added);
        bool Any(int? Id);
        void Remove(Course removed);
    }
}
