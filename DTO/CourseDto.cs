using FinalProjectSPC.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProjectSPC.DTO
{
    public class CourseDto
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public DateTime? MeetingTime { get; set; }
        [Required]
        public DateTime? EndingTime { get; set; }
        [Required]
        public string MeetingDays { get; set; }

        public bool IsSelected { get; set; }
        public string InstructorName { get; set; }
    }
}
