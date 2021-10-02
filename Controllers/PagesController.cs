using Forum.Models;
using Forum.ViewModels;
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
            var responses = _context.Responses.Include(r => r.User).Include(r => r.User.ProfilePic).Include(r => r.Theme).Where(r => r.ThemeId == id);
            responses = responses.OrderBy(r => r.CreationDate);
            DetailsViewModel dvm = new DetailsViewModel
            {
                Theme = theme,
                Responses = responses.ToList()
            };
            return View(dvm);
        }
        public IActionResult Search(string keyWord)
        {
            List<Theme> themes = _context.Themes.Include(p => p.Creator).Where(u =>
                u.Name.Contains(keyWord)).ToList();
            return View("Index", themes);
        }
        [HttpPost]
        public IActionResult CreateResponse(int themeId, string text)
        {
            var theme = _context.Themes.Include(t => t.Creator).FirstOrDefault(t => t.Id == themeId);
            var user = _context.Users.Include(u => u.ProfilePic).FirstOrDefault(u => u.Id == _userManager.GetUserId(User));
            Response response = new Response
            {
                UserId = user.Id,
                User = user,
                CreationDate = DateTime.Now,
                Text = text,
                ThemeId = theme.Id,
                Theme = theme
            };
            _context.Responses.Add(response);
            theme.ResponsesCount = theme.ResponsesCount + 1;
            _context.SaveChanges();
            var response2 = _context.Responses.Include(r => r.User).Include(r => r.User.ProfilePic).Include(r => r.Theme).FirstOrDefault(r => r.Text == text);
            return PartialView("PartialViews/ResponsePartialView", response2);
        }
        public IActionResult ResponseDisplay(string text)
        {
            var response = _context.Responses.Include(r => r.User).Include(r => r.User.ProfilePic).Include(r => r.Theme).FirstOrDefault(r => r.Text == text);
            return PartialView("PartialViews/ResponsePartialView", response);
        }
    }
}
