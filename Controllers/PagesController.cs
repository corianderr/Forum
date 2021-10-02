using Forum.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Controllers
{
    public class PagesController : Controller
    {
        private readonly UserManager<User> _userManager;
        private ForumContext _context;
        public PagesController(UserManager<User> userManager, ForumContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Index()
        {
            IQueryable<Theme> themes = _context.Themes.Include(t => t.Creator);
            themes = themes.OrderByDescending(t => t.CreationDate);
            return View(themes.ToList());
        }
        [HttpGet]
        public IActionResult CreateTheme()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateTheme(Theme theme)
        {
            var user = _context.Users.Include(u => u.ProfilePic).FirstOrDefault(u => u.Id == _userManager.GetUserId(User));
            theme.CreatorId = user.Id;
            theme.Creator = user;
            theme.CreationDate = DateTime.Now;
            _context.Themes.Add(theme);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Details(int id)
        {
            var theme = _context.Themes.Include(t => t.Creator).FirstOrDefault(t => t.Id == id);
            return View(theme);
        }
        public IActionResult Search(string keyWord)
        {
            List<Theme> themes = _context.Themes.Include(p => p.Creator).Where(u =>
                u.Name.Contains(keyWord)).ToList();
            return View("Index", themes);
        }
    }
}
