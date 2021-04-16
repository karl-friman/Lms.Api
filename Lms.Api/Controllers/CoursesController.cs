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
    public class CoursesController : ControllerBase
    {
        //private readonly LmsApiContext db;
        private readonly IUnitOfWork uow;
        private readonly IMapper mapper;

        public CoursesController(LmsApiContext db, IUnitOfWork uow, IMapper mapper)//(LmsApiContext db, IUnitOfWork uow, IMapper mapper)
        {
            //this.db = db;
            this.uow = uow;
            this.mapper = mapper;
        }

        // GET: api/Courses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourse(bool includeModules = false)
        {
            var courses = await uow.CourseRepository.GetAllCourses(includeModules);
            var courseModulesDto = mapper.Map<IEnumerable<CourseModulesDto>>(courses);
            return Ok(courseModulesDto);
        }

        // GET: api/Courses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourse(int? id, bool includeModules = false)
        {
            if (id is null) return BadRequest();

            var course = await uow.CourseRepository.GetCourse(id, includeModules);

            if (course == null) return NotFound();

            var courseModulesDto = mapper.Map<CourseModulesDto>(course);

            return Ok(courseModulesDto);
        }

        // PUT: api/Courses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse(int id, Course course)
        {
            if (id != course.Id)
            {
                return BadRequest();
            }

            //db.Entry(course).State = EntityState.Modified;

            try
            {
                await uow.CourseRepository.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
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

        // POST: api/Courses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Course>> PostCourse(Course course)
        {
            //_context.Course.Add(course);
            await uow.CourseRepository.SaveAsync();
            //await _context.SaveChangesAsync();

            return CreatedAtAction("GetCourse", new { id = course.Id }, course);
        }

        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int? id)
        {
            var course = await uow.CourseRepository.GetCourse(id, includeModules: false);//await _context.Course.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            uow.CourseRepository.Remove(course);
            await uow.CourseRepository.SaveAsync();

            return NoContent();

        }

        private bool CourseExists(int id)
        {
            return uow.CourseRepository.Any(id);
        }

        [HttpPatch]
        public async Task<ActionResult<CourseDto>> PatchCourse(int? courseId, JsonPatchDocument<CourseDto> patchDocument)
        {
            //Kolla om kursen med courseId finns, returnera NotFound om den inte finns
            if (courseId is null) return BadRequest();

            var course = await uow.CourseRepository.GetCourse(courseId, includeModules: false);

            if (course == null) return NotFound();
            //Ta fram kursen mha UoW

            var model = mapper.Map<CourseDto>(course);
            patchDocument.ApplyTo(model, ModelState);

            //Försök validera modellen
            mapper.Map(model, course);
            if (await uow.CourseRepository.SaveAsync())
            {
                return Ok(mapper.Map<CourseDto>(course));
            }
            else
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
