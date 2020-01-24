using System;
using System.Collections.Generic;
using System.Linq;
using Exam.Contexts;
using Exam.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Exam.Controllers
{
    [Route("replace")]
    public class ReplaceController:Controller
    {
        private HomeContext dbContext;
        public ReplaceController(HomeContext context)
        {
            dbContext = context;
        }
        private User LoggedInUser()
        {
            User LogIn = dbContext.Users.Include(u => u.PlannedBananas)
            .FirstOrDefault( u=> u.UserId == HttpContext.Session.GetInt32("UserId"));
            
            return LogIn;
        }
// ------------------------ Dashboard ------------------------------
        [HttpGet("dashboard")]
        public IActionResult Dashboard()
        {
            User userInDb = LoggedInUser();
            if(userInDb == null)
            {
                return RedirectToAction("LogOut", "Home");
            }
            List<Banana> AllBananas = dbContext.Bananas.Include(t =>t.GuestList)
            .ThenInclude(p => p.Guest)
            .Include(t => t.Planner)
            .Where( t=> t.Date >DateTime.Now)
            .OrderByDescending(t => t.Date)
            .ToList();

            ViewBag.User = userInDb;

            return View(AllBananas);
        }
        // ------------------------ New Banana/Create Banana ------------------------------
        [HttpGet("new/banana")]
        public IActionResult NewBanana()
        {
            User userInDb = LoggedInUser();
            if(userInDb == null)
            {
                return RedirectToAction("LogOut", "Home");
            }
            ViewBag.User = userInDb;
            return View();
        }
        [HttpPost("create/banana")]
        public IActionResult CreateBanana(Banana newBanana)
        {
            User userInDb = LoggedInUser();
            if(userInDb == null)
            {
                return RedirectToAction("LogOut", "Home");
            }

            if(ModelState.IsValid)
            {
                if(userInDb.PlannedBananas.Any( b =>b.Date == newBanana.Date))
                {
                    ModelState.AddModelError("Date", "You already have planned Banana forr this date!");
                    ViewBag.User = userInDb;
                    return View("NewBanana");
                }
                else
                {
                    dbContext.Bananas.Add(newBanana);
                    dbContext.SaveChanges();
                    return Redirect($"/replace/{newBanana.BananaId}");
                }
            }
            ViewBag.User = userInDb;
            return View("NewBanana");
        }
        // ------------------------Show Banana/Delete ------------------------------
        [HttpGet("{bananaId}")]
        public IActionResult ShowBanana (int bananaID)
        {
            User userInDb = LoggedInUser();
            if (userInDb == null)
            {
                return RedirectToAction("LogOut", "Home");
            }
            Banana showBanana = dbContext.Bananas
            .Include(b => b.GuestList)
            .ThenInclude( p => p.Guest)
            .Include( b =>b.Planner)
            .FirstOrDefault(b => b.BananaId == bananaID);
            ViewBag.User = userInDb;
            return View(showBanana);
        }

        [HttpGet("delete/{bananaId}")]
        public IActionResult DeleteBanana (int bananaID)
        {
            User userInDb = LoggedInUser();
            if (userInDb == null)
            {
                return RedirectToAction("LogOut", "Home");
            }
            Banana removeBanana = dbContext.Bananas.FirstOrDefault( b => b.BananaId == bananaID);
            dbContext.Bananas.Remove(removeBanana);
            dbContext.SaveChanges();
            return RedirectToAction ("Dashboard");
        }
        // ------------------------Join Banana ------------------------------
        [HttpGet("response/{bananaId}/{userId}/{status}")]
        public IActionResult Join( int bananaID, int userId, string status)
        {
            User userInDb = LoggedInUser();
            if (userInDb == null)
            {
                return RedirectToAction("LogOut", "Home");
            }

            if(status =="join")
            {
                Participation going = new Participation();
                going.UserId = userId;
                going.BananaId = bananaID;
                dbContext.Participations.Add(going);
                dbContext.SaveChanges();
            }
            else if(status=="leave")
            {
                Participation leave = dbContext.Participations.FirstOrDefault(p =>p.BananaId == bananaID && p.UserId == userId);
                dbContext.Participations.Remove(leave);
                dbContext.SaveChanges();

            }
            else
            {
                return RedirectToAction("LogOut", "Home");
            }
            return RedirectToAction("Dashboard");
        }
    }
}