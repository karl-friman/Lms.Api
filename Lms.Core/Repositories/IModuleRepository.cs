using Lms.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Core.Repositories
{
    public interface IModuleRepository
    {
        Task<IEnumerable<Module>> GetAllModules();
        Task<Module> GetModule(int? Id);
        Task<Module> GetModule(string title);
        Task<bool> SaveAsync();
        Task AddAsync<T>(T added);
        bool Any(int? Id);
        void Remove(Module removed);
    }
}
