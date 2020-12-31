using FinalProjectSPC.Models;
using System;
using System.Collections.Generic;

namespace FinalProjectSPC.DTO
{
    public class InstructorDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IList<Course> Courses { get; set; }
    }
}
