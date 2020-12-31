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
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Students.ToListAsync());
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
                return NotFound();

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.Id == id);

            if (student == null)
                return NotFound();

            var studentDto = Mapper.Map<Student, StudentDto>(student);
            studentDto.Courses = new List<CourseDto>();

            foreach(var studentCourse in _context.StudentCourses
                .Where(sc => sc.Student.Id.Equals(id))
                .Include(sc => sc.Course))
            {
                studentDto.Courses.Add(Mapper.Map<Course, CourseDto>(studentCourse.Course));
            }

            return View(studentDto);
        }

        public IActionResult Create()
        {
            var dto = new StudentDto() { Courses = new List<CourseDto>() };

            foreach (Course course in _context.Courses)
                dto.Courses.Add(Mapper.Map<Course, CourseDto>(course));

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudentDto dto)
        {
            if (String.IsNullOrWhiteSpace(dto.Name))
                return View(dto);

            foreach (var course in dto.Courses)
                if (course.IsSelected)
                {
                    var c = await _context.Courses
                        .SingleOrDefaultAsync(cu => cu.Id.Equals(course.Id));

                    _context.StudentCourses.Add(new StudentCourses
                    {
                        Id = Guid.NewGuid(),
                        Course = c,
                        Student = new Student
                        {
                            Name = dto.Name,
                            Id = Guid.NewGuid()
                        }
                });
                }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
                return NotFound();

            var student = await _context.Students
                .SingleOrDefaultAsync(s => s.Id.Equals(id));

            var studentDto = Mapper.Map<Student, StudentDto>(student);

            if (student == null)
                return NotFound();

            studentDto.Courses = new List<CourseDto>();

            foreach (var studentCourse in _context.StudentCourses
                .Where(sc => sc.Student.Id.Equals(id))
                .Include(sc => sc.Course))
                studentDto.Courses.Add(Mapper.Map<Course, CourseDto>(studentCourse.Course));

            return View(studentDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name")] Student student)
        {
            if (id != student.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(student);
        }

        [HttpGet]
        public IActionResult Enroll(Guid id)
        {
            var dto = new EnrollDto
            {
                CoursesOffered = new List<CourseDto>(),
                Id = id
            };

            foreach (var course in _context.Courses)
            {
                var courseInstructor = _context.InstructorCourses
                    .Include(ic => ic.Course)
                    .Include(ic => ic.Instructor)
                    .SingleOrDefault(ic => ic.Course.Id.ToString().Equals(course.Id.ToString()));

                var courseDto = Mapper.Map<Course, CourseDto>(course);

                if (courseInstructor != null)
                {
                    courseDto.InstructorName = courseInstructor.Instructor.Name;

                    dto.CoursesOffered.Add(courseDto);
                }
            }

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Enroll(EnrollDto dto)
        {
            if (!dto.CoursesOffered.Any(c => c.IsSelected))
                return View(dto);

            var student = await _context.Students
                .SingleOrDefaultAsync(s => s.Id.Equals(dto.Id));

            foreach (var course in dto.CoursesOffered)
                if (course.IsSelected)
                    _context.StudentCourses.Add(new StudentCourses
                    {
                        Student = student,
                        Course = await _context.Courses
                        .SingleOrDefaultAsync(c => c.Id.Equals(course.Id))
                    });

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = student.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Drop(Guid? studentId, Guid? courseId)
        {
            var studentCourse = await _context.StudentCourses
                .Include(sc => sc.Course)
                .Include(sc => sc.Student)
                .SingleOrDefaultAsync(sc => sc.Student.Id.Equals(studentId)
                && sc.Course.Id.Equals(courseId));

            return View(studentCourse);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Drop(Guid? id)
        {
            var studentCourse = await _context.StudentCourses
                .SingleOrDefaultAsync(sc => sc.Id.Equals(id));

            _context.Remove(studentCourse);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(Guid id) =>
            _context.Students.Any(e => e.Id == id);
    }
}
