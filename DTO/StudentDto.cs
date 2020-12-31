using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FinalProjectSPC.DTO
{
    public class StudentDto
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public IList<CourseDto> Courses { get; set; }
    }
}
