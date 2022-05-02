using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Juan.Models
{
    public class Size:BaseEntity
    {
        [Required]
        public int Name { get; set; }
        public IEnumerable<ProductSize> ProductSizes { get; set; }
    }
}
