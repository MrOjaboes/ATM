using ATM.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ATM.Controllers
{
    [Authorize]
    public class CheckingAccountController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        // GET: CheckingAccount
        public ActionResult Statement(int id)
        {
            var checkingAccount = db.CheckingAccounts.Find(id);
            ViewBag.CheckingAccountId = checkingAccount;
            return View(checkingAccount.Transactions.ToList());
        }

        [Authorize(Roles ="Admin")]
        public ActionResult List()
        {
            var accounts = db.CheckingAccounts.ToList();
            return View(accounts);
        }

        // GET: CheckingAccount/Details/5
        
        public ActionResult AccountDetails(int CheckingAccountId)
        {
            var UserId = User.Identity.GetUserId();
            var checkingAccountId = db.CheckingAccounts.Where(c => c.ApplicationUserId == UserId).First();
            ViewBag.CheckingAccountId = checkingAccountId;

            return View();
        }

       [Authorize(Roles = "Admin")]
        public ActionResult Details(int id)
        {
            
            var checkingAccountId = db.CheckingAccounts.FirstOrDefault(c => c.Id == id);
            
            return View();
        }

         
         
        }
    }

