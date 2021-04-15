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
    public class ModuleRepository : IModuleRepository
    {
        private readonly LmsApiContext db;

        public ModuleRepository(LmsApiContext db)
        {
            this.db = db;
        }
        public async Task<IEnumerable<Module>> GetAllModules()
        {
            return await db.Module.ToListAsync();
        }
        public async Task<Module> GetModule(int? Id)
        {
            return await db.Module
                .FirstOrDefaultAsync(m => m.Id == Id);
        }

        public async Task<bool> SaveAsync()
        {
            return (await db.SaveChangesAsync()) >= 0;
        }
        public async Task AddAsync<Module>(Module module)
        {
            await db.AddAsync(module);
        }
        public void Remove(Module module)
        {
            db.Remove(module);
        }
        public bool Any(int? Id)
        {
            return db.Module.Any(m => m.Id == Id);
        }
    }
}
