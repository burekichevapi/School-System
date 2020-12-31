using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProjectSPC.Models
{
    public class InstructorCourses
    {
        public Guid Id { get; set; }
        public Instructor Instructor { get; set; }
        public Course Course { get; set; }
    }
}
