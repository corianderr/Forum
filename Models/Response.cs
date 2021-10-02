using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Models
{
    public class Response
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public DateTime CreationDate { get; set; }
        [Required]
        public string Text { get; set; }
        public int ThemeId { get; set; }
        public Theme Theme { get; set; }
    }
}
