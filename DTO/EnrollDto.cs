using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProjectSPC.DTO
{
    public class EnrollDto
    {
        public Guid Id { get; set; }
        public IList<CourseDto> CoursesOffered { get; set; }

    }
}
