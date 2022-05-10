using Juan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Juan.ViewModels
{
    public class BlogListVM
    {
        public List<Category> Categories { get; set; }
        public List<Tag> Tags { get; set; }
        public List<Blog> Blogs { get; set; }
        public List<int> FilterTagIds { get; set; }

    }
}
