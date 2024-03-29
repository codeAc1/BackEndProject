﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Juan.Models
{
    public class AppUser: IdentityUser
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string SurName { get; set; }
        public bool IsDeleted { get; set; }
        public string Image { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        public bool isAdmin { get; set; }
        public string Address { get; set; }
        [StringLength(255)]
        public string Country { get; set; }
        [StringLength(255)]
        public string City { get; set; }
        [StringLength(255)]
        public string State { get; set; }
        [StringLength(255)]
        public string ZipCode { get; set; }
    }
}
