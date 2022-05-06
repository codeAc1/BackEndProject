using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Juan.Models
{
    public class Color:BaseEntity
    {
        [StringLength(255), Required]
        public string Name { get; set; }
        public IEnumerable<ProductColor> ProductColors { get; set; }
        
    }
}
