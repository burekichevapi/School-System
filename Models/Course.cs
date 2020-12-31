using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace FinalProjectSPC.Models
{
    public class Course
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime? MeetingTime { get; set; }
        public DateTime? EndingTime { get; set; }
        public string MeetingDays { get; set; }
    }
}