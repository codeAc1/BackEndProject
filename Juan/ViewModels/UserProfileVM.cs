using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Juan.ViewModels
{
    public class UserProfileVM
    {
        public UserUpdateVM Member { get; set; }
        public List<Models.Order> Orders { get; set; }
    }
}
