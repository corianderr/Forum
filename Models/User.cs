using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Models
{
    public class User : IdentityUser
    {
        public int ProfilePicId { get; set; }
        public AvatarFile ProfilePic { get; set; }
    }
}
