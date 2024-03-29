﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Juan.Models
{
    public class Tag:BaseEntity
    {
        [StringLength(255), Required]
        public string Name { get; set; }

        public IEnumerable<ProductTag> ProductTags { get; set; }
        public IEnumerable<BlogTag> BlogTags { get; set; }
    }
}
