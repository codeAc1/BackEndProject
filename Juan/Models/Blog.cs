using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Juan.Models
{
    public class Blog:BaseEntity
    {
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public string MainDes { get; set; }
        public string Desc { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public int CategoryId { get; set; }
        
        public Category Category { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }

        [NotMapped]
        public List<int> TagIds { get; set; } = new List<int>();

        public IEnumerable<BlogTag> BlogTags { get; set; }
        public IEnumerable<Review> Reviews { get; set; }

    }
}
