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

namespace FinalProjectSPC.Controllers
{
    public class AttendancesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AttendancesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var all = await _context.Attendances
                .Include(a => a.Student)
                .Include(a => a.Instructor)
                .Include(a => a.Course)
                .OrderBy(a => a.Date).ToListAsync();

            return View(all);
        }

        [HttpGet]
        public async Task<IActionResult> IndexOnlyStudents()
        {
            return View(await _context.Attendances
                .Include(a => a.Student)
                .Where(a => a.Instructor == null)
                .OrderBy(a => a.Date).ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> IndexOnlyInstructors()
        {
            return View(await _context.Attendances
                .Include(a => a.Instructor)
                .Where(a => a.Student == null)
                .OrderBy(a => a.Date).ToListAsync());
        }

        [HttpGet]
        public IActionResult Login()
        {
            var dto = new LoginDto();

            return View(dto);
        }

        [HttpGet]
        public async Task<IActionResult> Find([Bind("UserId,CourseId,IsStudent")] LoginDto dto)
        {
            if (dto.IsStudent)
            {
                var studentCourses = _context.StudentCourses
                    .Include(sc => sc.Course)
                    .Where(sc => sc.Student.GetId().Equals(dto.UserId)).ToList();

                if (!studentCourses.Any())
                    return RedirectToAction(nameof(Login));


                var studentLoginDto = new LoginDto
                {
                    UserId = dto.UserId,
                    IsStudent = dto.IsStudent,
                    Courses = new List<Course>(),
                    Name = _context.Students
                        .SingleOrDefault(s => s.GetId().Equals(dto.UserId)).Name
                };

                for (int sc = 0; sc < studentCourses.Count; sc++)
                    studentLoginDto.Courses.Add(studentCourses[sc].Course);

                return View(studentLoginDto);
            }


            var instructorCourses = _context.InstructorCourses
                .Include(sc => sc.Course)
                .Where(sc => sc.Instructor.GetId().Equals(dto.UserId)).ToList();

            if (!instructorCourses.Any())
                return RedirectToAction(nameof(Login));

            var InstructorLoginDto = new LoginDto
            {
                UserId = dto.UserId,
                IsStudent = dto.IsStudent,
                Courses = new List<Course>(),
                Name = _context.Instructors
                    .SingleOrDefault(i => i.GetId().Equals(dto.UserId)).Name
            };

            for (int ic = 0; ic < instructorCourses.Count; ic++)
                InstructorLoginDto.Courses.Add(instructorCourses[ic].Course);

            return View(InstructorLoginDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,CourseId,IsStudent")] LoginDto dto)
        {
            if (!dto.CourseId.Equals(null))
            {
                if (dto.IsStudent)
                {
                    var StudentAttendance = new Attendance
                    {
                        Id = Guid.NewGuid(),
                        Date = DateTime.Now,
                        Course = await _context.Courses
                        .SingleOrDefaultAsync(c => c.Id.Equals(dto.CourseId)),
                        Student = await _context.Students
                        .SingleOrDefaultAsync(s => s.GetId().Equals(dto.UserId))
                    };

                    _context.Add(StudentAttendance);

                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }

                var InstructorAttendance = new Attendance
                {
                    Id = Guid.NewGuid(),
                    Date = DateTime.Now,
                    Course = await _context.Courses
                    .SingleOrDefaultAsync(c => c.Id.Equals(dto.CourseId)),
                    Instructor = await _context.Instructors
                    .SingleOrDefaultAsync(i => i.GetId().Equals(dto.UserId))
                };

                _context.Add(InstructorAttendance);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(dto);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendance = await _context.Attendances
                .FirstOrDefaultAsync(m => m.Id == id);
            if (attendance == null)
            {
                return NotFound();
            }

            return View(attendance);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var attendance = await _context.Attendances.FindAsync(id);
            _context.Attendances.Remove(attendance);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AttendanceExists(Guid id)
        {
            return _context.Attendances.Any(e => e.Id == id);
        }
    }
}
