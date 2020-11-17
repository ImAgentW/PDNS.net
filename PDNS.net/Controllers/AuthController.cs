using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PDNS.net.Models;
using PDNS.net.ViewModels;

namespace PDNS.net.Controllers
{
    public class AuthController : Controller
    {
        private readonly SignInManager<User> _signInManager;

        public AuthController(SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return PartialView("Login");
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["returnUrl"] = returnUrl;
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(LoginViewModel User, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(User.Username, User.Password, false, lockoutOnFailure: true);

                if (result.Succeeded)
                {
                    if (string.IsNullOrWhiteSpace(returnUrl))
                        return RedirectToAction("Index", "Home");
                    else
                        return Redirect(returnUrl);
                }
                else if (result.IsLockedOut)
                {
                    ViewData["Error"] = "Your account has been locked.";
                    return PartialView("Login");
                }
            }
            ViewData["Error"] = "Invalid login attempt.";
            return PartialView("Login");
        }

        [HttpGet]
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            if (string.IsNullOrWhiteSpace(returnUrl))
                return RedirectToAction("Index", "Home");
            else
                return Redirect(returnUrl);
        }
    }
}
