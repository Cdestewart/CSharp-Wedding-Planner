using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WeddingPlanning.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace WeddingPlanning.Controllers
{
    
    public class HomeController : Controller
    {
        private weddingplanningContext dbContext;

        public HomeController(weddingplanningContext context)
        {
            dbContext = context;
        }
        [HttpGet("")]
        public IActionResult Index()
        {
            return RedirectToAction("LoginReg");
        }
        [HttpGet("LoginReg")]
        public IActionResult LoginReg()
        {
            return View();
        }

        [HttpPost("LoginReg")]
        public IActionResult CreateAccount(User newUser)
        {
            if(ModelState.IsValid){
                if(dbContext.users.Any(u => u.Email == newUser.Email))
                    {      
                    return View("LoginReg");
                    }
                else{

                        PasswordHasher<User> Hasher = new PasswordHasher<User>();
                        newUser.Password = Hasher.HashPassword(newUser, newUser.Password);
     
                        dbContext.users.Add(newUser);
                        dbContext.SaveChanges();
                        HttpContext.Session.SetString("UserName", newUser.FName);
                        HttpContext.Session.SetString("email", newUser.Email);
                        HttpContext.Session.SetInt32("UserId", newUser.UserId);
                        return Redirect("dashboard");
                } 
            }
            return View("LoginReg");
        }
        [HttpPost("Login")]
        public IActionResult Login(LoginUser userSubmission)
        {
          if(ModelState.IsValid)
        {
            var userInDb = dbContext.users.FirstOrDefault(u => u.Email == userSubmission.Email);
            if(userInDb == null)
            {             
                return View("LoginReg");
            }
            var hasher = new PasswordHasher<LoginUser>();
            var result = hasher.VerifyHashedPassword(userSubmission, userInDb.Password, userSubmission.Password);
            if(result == 0)
            {
               return View("LoginReg");
            }
            HttpContext.Session.SetString("UserName", userInDb.FName);            
            HttpContext.Session.SetString("email", userSubmission.Email);
            HttpContext.Session.SetInt32("UserId", userInDb.UserId);
            return Redirect("dashboard");

        } 
           return View("LoginReg"); 
        }
        [HttpGet("dashboard")]
        public IActionResult Dashboard()
        {
            ViewBag.Weddings = dbContext.wedding.Include(w=>w.WedderOne).Include(w=>w.WedderTwo).Include(w=>w.Attendees).ToList();
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
        
            return View();
        }
        [HttpGet("wedding/{id}")]
        public IActionResult singleWedding(int id)
        {
            ViewBag.Wedding = dbContext.wedding.Include(w=>w.Attendees).ThenInclude(a=>a.User).Where(w=>w.WeddingId==id).ToList();
            ViewBag.Wedders = dbContext.wedding.Include(w=>w.WedderOne).Include(w=>w.WedderTwo).SingleOrDefault(w=>w.WeddingId == id);

            return View();
        }
        [HttpGet("plan-your-wedding")]
        public IActionResult addWedding()
        {
            ViewBag.Wedders = dbContext.users.ToList();
            return View();
        }
        [HttpPost("plan-your-wedding")]
        public IActionResult createWedding(Wedding newWedding)
        {
            if(ModelState.IsValid)
            {
                DateTime now = DateTime.Now;
                if(newWedding.Date>now){
                    dbContext.wedding.Add(newWedding);
                    dbContext.SaveChanges();
                    return Redirect("dashboard");
                }
                return RedirectToAction("addWedding");
            }   
            return RedirectToAction("addWedding");
        }
        [HttpGet("logout")]
        public IActionResult logout()
        {
            HttpContext.Session.Clear();
            return Redirect("LoginReg");
        }   
        [HttpGet("rsvp/{id}")]
        public IActionResult RSVP(int id)
        {
            int userId = (int)HttpContext.Session.GetInt32("UserId");
            Attendee newAttendee = new Attendee();
            newAttendee.UserId = (int)HttpContext.Session.GetInt32("UserId");
            newAttendee.WeddingId = id;
            dbContext.Add(newAttendee);
            dbContext.SaveChanges();
            return Redirect("/dashboard");
        } 
        [HttpGet("unrsvp/{id}")]
        public IActionResult unrsvp(int id)
        {
            int userId = (int)HttpContext.Session.GetInt32("UserId");
            Attendee newAttendee = dbContext.attendees.Where(a=>a.UserId==userId).SingleOrDefault(a=>a.WeddingId==id);
            dbContext.attendees.Remove(newAttendee);
            dbContext.SaveChanges();
            return Redirect("/dashboard");
        } 
        [HttpGet("delete/{id}")]
        public IActionResult delete(int id)
        {
            Attendee deleteAttendee = dbContext.attendees.SingleOrDefault(a=>a.WeddingId==id);
            Wedding deleteWedding = dbContext.wedding.SingleOrDefault(w=>w.WeddingId==id);
            dbContext.attendees.Remove(deleteAttendee);
            dbContext.wedding.Remove(deleteWedding);
            dbContext.SaveChanges();
            return Redirect("/dashboard");
        } 
    }
}
