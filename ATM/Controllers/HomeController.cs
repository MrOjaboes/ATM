using ATM.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ATM.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();


        public ActionResult Index()
        {
            return View();
        }
        [Authorize]
        public ActionResult Account()

        {
            var UserId = User.Identity.GetUserId();
            var checkingAccountId = db.CheckingAccounts.Where(c => c.ApplicationUserId == UserId).First().Id;
            ViewBag.CheckingAccountId = checkingAccountId;
            return View();
        }

        [HttpGet]
        public ActionResult About()
        {
            ViewBag.Message = "Having Trouble? Send us a Message!";

            return  View();
        }
        [HttpPost]
        public ActionResult About(string message)
        {
            ViewBag.Message = "Thanks, We Got your message! ";

            return PartialView("_ContactThanks");
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}