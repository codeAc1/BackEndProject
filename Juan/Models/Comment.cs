using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Juan.Models
{
    public class Comment:BaseEntity
    {
        public string Message { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public int?  BlogId { get; set; }
        public Blog Blog { get; set; }
    }
}
