using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProjectSPC.Models
{
    public class Attendance
    {
        public Guid Id { get; set; }
        public DateTime? Date { get; set; }
        public Course Course { get; set; }
        public Student Student { get; set; }
        public Instructor Instructor { get; set; }

    }
}
