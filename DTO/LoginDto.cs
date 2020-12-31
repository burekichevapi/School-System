using FinalProjectSPC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProjectSPC.DTO
{
    public class LoginDto
    {
        public string UserId { get; set; }
        public Guid? CourseId { get; set; }
        public string Name { get; set; }
        public bool IsStudent { get; set; }
        public IList<Course> Courses { get; set; }
    }
}
