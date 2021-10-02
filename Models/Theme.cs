using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Models
{
    public class Theme
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Enter theme's name")]
        public string Name { get; set; }
        public string CreatorId { get; set; }
        public User Creator { get; set; }
        public DateTime CreationDate { get; set; }
        public int ResponsesCount { get; set; }
    }
}
