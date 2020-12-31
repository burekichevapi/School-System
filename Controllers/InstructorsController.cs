using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinalProjectSPC.Data;
using FinalProjectSPC.Models;
using FinalProjectSPC.DTO;
using AutoMapper;

namespace FinalProjectSPC.Controllers
{
    public class InstructorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InstructorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Instructors.ToListAsync());
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
                return NotFound();

            var instructor = await _context.Instructors
                .FirstOrDefaultAsync(m => m.Id == id);

            if (instructor == null)
                return NotFound();

            var dto = new InstructorDto
            {
                Id = instructor.Id,
                Name = instructor.Name,
                Courses = new List<Course>()
            };

            foreach (var instructorCourses in _context.InstructorCourses
                .Include(ic => ic.Course)
                .Where(ic => ic.Instructor.Id.Equals(id)))
            {
                dto.Courses.Add(instructorCourses.Course);
            }

            return View(dto);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Instructor instructor)
        {
            if (ModelState.IsValid)
            {
                instructor.Id = Guid.NewGuid();
                _context.Add(instructor);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(instructor);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
                return NotFound();

            var instructor = await _context.Instructors.FindAsync(id);

            if (instructor == null)
                return NotFound();

            return View(instructor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name")] Instructor instructor)
        {
            if (id != instructor.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(instructor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InstructorExists(instructor.Id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(instructor);
        }

        [HttpGet]
        public IActionResult AssignCourse(Guid id)
        {
            var dto = new EnrollDto { Id = id, CoursesOffered = new List<CourseDto>() };

            foreach (var course in _context.Courses)
                dto.CoursesOffered.Add(Mapper.Map<Course, CourseDto>(course));

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignCourse(EnrollDto dto)
        {
            foreach (var course in dto.CoursesOffered)
                if (course.IsSelected)
                {
                    _context.InstructorCourses.Add(new InstructorCourses
                    {
                        Course = await _context.Courses.SingleOrDefaultAsync(c => c.Id.Equals(course.Id)),
                        Instructor = await _context.Instructors.SingleOrDefaultAsync(i => i.Id.Equals(dto.Id))
                    });

                    await _context.SaveChangesAsync();
                }

            return RedirectToAction(nameof(Details), new { id = dto.Id });
        }

        [HttpGet]
        public async Task<IActionResult> RemoveFromCourse(Guid? instructorId, Guid? courseId)
        {
            var instructorCourse = await _context.InstructorCourses
                .Include(ic => ic.Course)
                .Include(ic => ic.Instructor)
                .SingleOrDefaultAsync(ic => ic.Instructor.Id.Equals(instructorId)
                && ic.Course.Id.Equals(courseId));

            return View(instructorCourse);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFromCourse(Guid? id)
        {
            var instructorCourse = await _context.InstructorCourses
                .SingleOrDefaultAsync(ic => ic.Id.Equals(id));

            _context.Remove(instructorCourse);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool InstructorExists(Guid id) =>
            _context.Instructors.Any(e => e.Id == id);
    }
}
