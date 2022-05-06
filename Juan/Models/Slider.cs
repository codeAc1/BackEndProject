using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Juan.Models
{
    public class Slider:BaseEntity
    {
        public string SlideSubtitle { get; set; }
        public string SlideTitle { get; set; }
        public string SlideDesc { get; set; }

        [StringLength(255)]
        public string ImageUrl { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }
    }
}
