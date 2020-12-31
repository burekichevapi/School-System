using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProjectSPC.Models
{
    public class StudentCourses
    {
        public Guid Id { get; set; }
        public Course Course { get; set; }
        public Student Student { get; set; }
    }
}
