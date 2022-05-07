using Juan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Juan.ViewModels
{
    public class HomeVM
    {
        public IEnumerable <Slider> Sliders { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Blog> Blogs { get; set; }
        public IEnumerable<Brand> Brands { get; set; }

    }
}
