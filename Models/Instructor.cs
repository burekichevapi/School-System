using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProjectSPC.Models
{
    public class Instructor
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }

        public string GetId() =>
            Id.ToString().Substring(Id.ToString().Length - 6);
    }
}
