using Forum.Models;
using Forum.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private IWebHostEnvironment _environment;
        private ForumContext _context;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IWebHostEnvironment environment, ForumContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _environment = environment;
            _context = context;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(List<IFormFile> files, RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                foreach (var file in files)
                {
                    var basePath = Path.Combine(Directory.GetCurrentDirectory() + "\\wwwroot" + "\\Avatars\\");
                    bool basePathExists = Directory.Exists(basePath);
                    if (!basePathExists) Directory.CreateDirectory(basePath);
                    var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                    var filePath = Path.Combine(basePath, file.FileName);
                    var extension = Path.GetExtension(file.FileName);
                    if (!System.IO.File.Exists(filePath))
                    {
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                        User user = new User
                        {
                            Email = model.Email,
                            UserName = model.UserName,
                        };
                        var fileModel = new AvatarFile
                        {
                            FileType = file.ContentType,
                            FileName = fileName,
                            FilePath = filePath,
                            FileExtension = extension
                        };
                        _context.Avatars.Add(fileModel);
                        user.ProfilePicId = fileModel.Id;
                        user.ProfilePic = fileModel;
                        var result = await _userManager.CreateAsync(user, model.Password);
                        if (result.Succeeded)
                        {
                            await _signInManager.SignInAsync(user, false);
                            return RedirectToAction("Index", "Home");
                        }
                        foreach (var error in result.Errors)
                            ModelState.AddModelError("Mes", error.Description);
                        _context.SaveChanges();
                    }
                }

            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User();
                if (model.EmailOrUserName.Contains('@'))
                    user = await _userManager.FindByEmailAsync(model.EmailOrUserName);
                else
                    user = await _userManager.FindByNameAsync(model.EmailOrUserName);
                Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(
                    user,
                    model.Password,
                    model.RememberMe,
                    false
                    );
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("Mes", "Неправильный логин и (или) пароль");
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
        public IActionResult PersonalArea()
        {
            var user = _context.Users.Include(u => u.ProfilePic).FirstOrDefault(u => u.Id == _userManager.GetUserId(User));
            return View(user);
        }
    }

}
