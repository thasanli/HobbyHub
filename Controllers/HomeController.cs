using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HobbyApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace HobbyApp.Controllers
{
    public class HomeController : Controller
    {
        private MyContext db;
        public HomeController(MyContext database)
        {
            db = database;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("registration")]

        public IActionResult Registration(User newUser)
        {
            if (ModelState.IsValid)
            {
                if (db.Users.Any(x => x.Email == newUser.Email))
                {
                    ModelState.AddModelError("Email", "Email alrady taken");
                    return View("Index");
                }
                if (db.Users.Any(x => x.Username == newUser.Username))
                {
                    ModelState.AddModelError("Username", "Username alrady taken");
                    return View("Index");
                }


                PasswordHasher<User> PwdHash = new PasswordHasher<User>();
                newUser.Password = PwdHash.HashPassword(newUser, newUser.Password);
                db.Add(newUser);
                db.SaveChanges();
                HttpContext.Session.SetInt32("UserId", newUser.UserID);

                return RedirectToAction("Hobby");
            }
            return View("Index");
        }

        [HttpGet("hobby")]
        public IActionResult Hobby()
        {
            List<Hobby> AllHobbies = db.Hobbies
            .Include(a => a.Euthusiasts)
            .ToList();

            return View(AllHobbies);
        }

        [HttpPost("login")]
        public IActionResult Login(LoginUser user)
        {
            if (ModelState.IsValid)
            {
                User Username = db.Users.FirstOrDefault(x => x.Username == user.LoginUserName);
                if (Username == null)
                {
                    ModelState.AddModelError("LoginUserName", "Invalid Username and/or Password");
                    return View("Index");
                }
                PasswordHasher<User> pwdHash = new PasswordHasher<User>();
                var hasher = new PasswordHasher<LoginUser>();
                var result = hasher.VerifyHashedPassword(user, Username.Password, user.LoginPassword);
                if (result == 0)
                {
                    ModelState.AddModelError("LoginUserName", "Invalid Username and/or Password");
                    return View("Index");
                }
                HttpContext.Session.SetInt32("UserId", Username.UserID);

                return RedirectToAction("Hobby");
            }
            return View("Index");
        }

        [HttpGet("hobby/new")]
        public IActionResult New()
        {
            return View();
        }

        [HttpPost("newHobby")]
        public IActionResult NewHobby(Hobby data)
        {

            if (ModelState.IsValid)
            {
                Hobby uniqueHobby = db.Hobbies.FirstOrDefault(x => x.HobbyName == data.HobbyName);
                if (uniqueHobby != null)
                {
                    ModelState.AddModelError("HobbyName", "This hobby name already exist");
                    return View("New");

                }
                User user = db.Users.FirstOrDefault(x => x.UserID == (int)HttpContext.Session.GetInt32("UserId"));
                data.UserId = user.UserID;
                db.Hobbies.Add(data);
                db.SaveChanges();
                return RedirectToAction("Hobby");
            }
            return View("New");

        }

        [HttpGet("ViewHobby/{HobbyId}")]

        public IActionResult ViewHobby(int HobbyId)
        {
            var data = db.Hobbies
            .Include(a => a.Euthusiasts)
            .ThenInclude(b => b.User)
            .FirstOrDefault(x => x.HobbyId == HobbyId);
            ViewBag.data = data;
            User user = db.Users.FirstOrDefault(x => x.UserID == (int)HttpContext.Session.GetInt32("UserId"));
            ViewBag.UserId = user.UserID;
            return View();
        }

        [HttpPost("addEuthusiasts")]
        public IActionResult AddEuthusiasts(Euthusiasts data, int id)
        {

            User user = db.Users.FirstOrDefault(x => x.UserID == (int)HttpContext.Session.GetInt32("UserId"));
            data.HobbyId = id;
            data.UserID = user.UserID;
            db.Add(data);
            db.SaveChanges();
            return RedirectToAction("Hobby");

        }

        [HttpGet("viewHobby/edit/{HobbyId}")]

        public IActionResult Edit(int HobbyId)
        {
            var hobby = db.Hobbies.FirstOrDefault(x => x.HobbyId == HobbyId);

            return View(hobby);
        }

        [HttpPost("EditHubby")]
        public IActionResult EditHubby(Hobby data, int id)
        {
            if (ModelState.IsValid)
            {

                Hobby EditHubby = db.Hobbies.FirstOrDefault(x => x.HobbyId == id);
                EditHubby.Description = data.Description;
                EditHubby.HobbyName = data.HobbyName;
                db.SaveChanges();
                return RedirectToAction("Hobby");
            }
            return RedirectToAction("Hobby");
        }
    }
}
