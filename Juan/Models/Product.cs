using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Juan.Models
{
    public class Product:BaseEntity
    {
        [StringLength(255), Required]
        public string Title { get; set; }
        [Column(TypeName = "money")]
        public double DiscountPrice { get; set; }
        [Column(TypeName = "money")]
        public double Price { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        public int Count { get; set; }
        public string MainImage { get; set; }
        public bool Availability { get; set; }
        [NotMapped]
        public List<int> TagIds { get; set; } = new List<int>();
        [NotMapped]
        public List<int> ColorIds { get; set; } = new List<int>();
        [NotMapped]
        public List<int> SizeIds { get; set; } = new List<int>();

        [NotMapped]
        public IFormFile[] ProductImagesFile { get; set; }
        [NotMapped]
        public IFormFile MainImageFile { get; set; }
        public Nullable<int> BrandId { get; set; }
        public Brand Brand { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public Category Category { get; set; }
        
        public IEnumerable<ProductTag> ProductTags { get; set; }
        public IEnumerable<ProductColor> ProductColors { get; set; }
        public IEnumerable<ProductSize> productSizes { get; set; }
        public List<ProductImage> ProductImages { get; set; }
        public IEnumerable<Review> Reviews { get; set; }
    }
}
