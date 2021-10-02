using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Models
{
    public class ForumContext : IdentityDbContext<User>
    {
        public DbSet<AvatarFile> Avatars { get; set; }
        public DbSet<Theme> Themes { get; set; }
        public DbSet<Response> Responses { get; set; }
        public ForumContext(DbContextOptions<ForumContext> options) : base(options) { }
    }
}
