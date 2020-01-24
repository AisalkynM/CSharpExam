using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Exam.Models;
using Exam.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace Exam.Controllers
{
    public class HomeController : Controller
    {
        private HomeContext dbContext;

        public HomeController(HomeContext context)
        {
            dbContext = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("register")]
        public IActionResult Register(User register)
        {
            if (ModelState.IsValid)
            {
                if (dbContext.Users.Any(u => u.Email == register.Email))
                {
                    ModelState.AddModelError("Email", "That email already exits");
                    return RedirectToAction("Index");
                }
                else
                {
                    PasswordHasher<User> hash = new PasswordHasher<User>();
                    register.Password = hash.HashPassword(register, register.Password);

                    dbContext.Users.Add(register);
                    dbContext.SaveChanges();

                    return RedirectToAction("Login");
                }
            }
            else
            {
                return View("Index");
            }
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("sigin")]
        public IActionResult SignIn(LoginUser log)
        {
            if (ModelState.IsValid)
            {
                User check = dbContext.Users.FirstOrDefault(u => u.Email == log.LoginEmail);
                if (check == null)
                {
                    ModelState.AddModelError("LoginEmail", "Email/Password invalid");
                    return View("Login");
                }
                else
                {

                    PasswordHasher<LoginUser> compare = new PasswordHasher<LoginUser>();
                    var result = compare.VerifyHashedPassword(log, check.Password, log.LoginPassword);

                    if (result == 0)
                    {
                        ModelState.AddModelError("LoginEmail", "Email/Password invalid");
                        return View("Login");
                    }
                    else
                    {
                        HttpContext.Session.SetInt32("UserId", check.UserId);
//REPLACE(where you want to redirect if login is success)
                        return RedirectToAction("Dashboard","Replace");
                    }
                }
            }
            else
            {
                return View("Login");
            }
        }

        [HttpGet("logout")]
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
