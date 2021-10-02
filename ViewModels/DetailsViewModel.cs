using Forum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.ViewModels
{
    public class DetailsViewModel
    {
        public Theme Theme { get; set; }
        public List<Response> Responses { get; set; }
    }
}
