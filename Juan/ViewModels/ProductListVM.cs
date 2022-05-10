using Juan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Juan.ViewModels
{
    public class ProductListVM
    {

        public List<Setting> Settings { get; set; }
        public List<Product> Products { get; set; }
        public List<Category> Categories { get; set; }
        public List<Color> Colors { get; set; }
        public List<Size> Sizes { get; set; }
        public List<int> FilterColorIds { get; set; }
        public List<int> FilterSizeIds { get; set; }
        public decimal MaxPrice { get; set; }
        public decimal MinPrice { get; set; }

    }
}
