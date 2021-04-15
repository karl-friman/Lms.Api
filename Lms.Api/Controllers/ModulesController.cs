using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lms.Data.Data;
using Lms.Core.Entities;
using Lms.Core.Repositories;
using AutoMapper;
using Lms.Core.Dto;
using Microsoft.AspNetCore.JsonPatch;

namespace Lms.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModulesController : ControllerBase
    {
        //private readonly LmsApiContext db;
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public ModulesController(LmsApiContext db, IUnitOfWork uow, IMapper mapper)//(LmsApiContext db, IUnitOfWork uow, IMapper mapper)
        {
            //this.db = db;
            this.uow = uow;
            this.mapper = mapper;
        }

        // GET: api/Modules
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ModuleDto>>> GetModule()
        {
            var modules = await uow.ModuleRepository.GetAllModules();
            var modulesDto = mapper.Map<IEnumerable<ModuleDto>>(modules);
            return Ok(modulesDto);
        }

        // GET: api/Modules/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Module>> GetModule(int? id)
        {
            if (id is null) return BadRequest();

            var module = await uow.ModuleRepository.GetModule(id);

            if (module == null) return NotFound();

            var moduleDto = mapper.Map<ModuleDto>(module);

            return Ok(moduleDto);
        }

        // PUT: api/Modules/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutModule(int id, Module module)
        {
            if (id != module.Id)
            {
                return BadRequest();
            }

            //db.Entry(module).State = EntityState.Modified;

            try
            {
                await uow.ModuleRepository.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ModuleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
            }

            return NoContent();
        }

        // POST: api/Modules
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Module>> PostModule(Module module)
        {
            //_context.Module.Add(module);
            await uow.ModuleRepository.SaveAsync();
            //await _context.SaveChangesAsync();

            return CreatedAtAction("GetModule", new { id = module.Id }, module);
        }

        // DELETE: api/Modules/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteModule(int? id)
        {
            var module = await uow.ModuleRepository.GetModule(id);//await _context.Module.FindAsync(id);
            if (module == null)
            {
                return NotFound();
            }

            uow.ModuleRepository.Remove(module);
            await uow.ModuleRepository.SaveAsync();

            return Ok(module);

        }

        private bool ModuleExists(int id)
        {
            return uow.ModuleRepository.Any(id);
        }

        [HttpPatch]
        public async Task<ActionResult<ModuleDto>> PatchModule(int? moduleId, JsonPatchDocument<ModuleDto> patchDocument)
        {
            //Kolla om kursen med moduleId finns, returnera NotFound om den inte finns
            if (moduleId is null) return BadRequest();

            var module = await uow.ModuleRepository.GetModule(moduleId);

            if (module == null) return NotFound();
            //Ta fram kursen mha UoW

            var model = mapper.Map<ModuleDto>(module);
            patchDocument.ApplyTo(model, ModelState);

            //Försök validera modellen
            mapper.Map(model, module);
            if (await uow.ModuleRepository.SaveAsync())
            {
                return Ok(mapper.Map<ModuleDto>(module));
            }
            else
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
