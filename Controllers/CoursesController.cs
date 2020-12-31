using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinalProjectSPC.Data;
using FinalProjectSPC.Models;
using FinalProjectSPC.DTO;
using AutoMapper;

namespace FinalProjectSPC.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CoursesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Courses.ToListAsync());
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
                return NotFound();

            var course = await _context.Courses
                .FirstOrDefaultAsync(m => m.Id == id);

            if (course == null)
                return NotFound();

            return View(course);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Location,MeetingTime,EndingTime,MeetingDays")] CourseDto course)
        {
            if (ModelState.IsValid)
            {
                course.Id = Guid.NewGuid();
                _context.Add(Mapper.Map<CourseDto, Course>(course));
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(course);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
                return NotFound();

            var course = await _context.Courses.FindAsync(id);

            if (course == null)
                return NotFound();

            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Description,Location,MeetingTime,EndingTime,MeetingDays")] Course course)
        {
            if (id != course.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.Id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(course);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
                return NotFound();

            var course = await _context.Courses
                .FirstOrDefaultAsync(m => m.Id == id);

            if (course == null)
                return NotFound();

            return View(course);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var course = await _context.Courses.FindAsync(id);

            var StudentCourse = await _context.StudentCourses
                .FirstOrDefaultAsync(m => m.Course.Id == id);

            var InstructorCourse = await _context.InstructorCourses
                .FirstOrDefaultAsync(m => m.Course.Id == id);

            if(StudentCourse != null)
                _context.Remove(StudentCourse);

            if(InstructorCourse != null)
                _context.Remove(InstructorCourse);

            _context.Remove(course);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(Guid id) => _context.Courses.Any(e => e.Id == id);

    }
}
