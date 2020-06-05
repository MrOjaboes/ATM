using ATM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ATM.Controllers
{
    [Authorize]
    public class TransactionController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();


        public void UpdateBalance(int checkingAccountId)

        {
            var checkingaccount = db.CheckingAccounts.Where(c => c.Id == checkingAccountId).First();
            checkingaccount.Balance = db.Transactions.Where(c => c.CheckingAccountId == checkingAccountId).Sum(c => c.Amount);
            db.SaveChanges();
            
            
        }


        // GET: Transaction/Deposit
        public ActionResult Deposit(int CheckingAccountId)
        {
            return View();
        }

        // POST: Transaction/Deposit
        [HttpPost]
        public ActionResult Deposit(Transaction transaction)
        {
            try
            {
                // TODO: Add insert logic here
                db.Transactions.Add(transaction);
                db.SaveChanges();
                UpdateBalance(transaction.CheckingAccountId);
                return RedirectToAction("Account","Home");
            }
            catch
            {
                return View();
            }
        }


        // GET: Transaction/Withdrawal
        public ActionResult Withdrawal(int checkingAccountId)
        {
            return View();
        }

        // POST: Transaction/Withdrawal
        [HttpPost]
        public ActionResult Withdrawal(Transaction transaction)
        {
            
                // TODO: Add insert logic here
                var checkingAccount = db.CheckingAccounts.Find(transaction.CheckingAccountId);
                if(checkingAccount.Balance > transaction.Amount)
                {
                    ModelState.AddModelError("Amount", "You Have Insufficient Funds");
                }
                if (ModelState.IsValid)
                {
                    transaction.Amount = -transaction.Amount;
                    db.Transactions.Add(transaction);
                    db.SaveChanges();

                UpdateBalance(transaction.CheckingAccountId);

                return RedirectToAction("Account", "Home");
                }
 
                return View();
            
        }


        // GET: Transaction/Transfer Funds
        public ActionResult Transfer(int checkingAccountId)
        {
            return View();
        }

        // POST: Transaction/Withdrawal
        [HttpPost]
        public ActionResult Transfer(TransferViewModel transfer)
        {

            // TODO: Add insert logic here
            var sourcecheckingAccount = db.CheckingAccounts.Find(transfer.CheckingAccountId);
            if (sourcecheckingAccount.Balance < transfer.Amount)
            {
                ModelState.AddModelError("Amount", "You Have Insufficient Funds");
            }

            //Checking for a valid destination Account
            var destinationCheckingAccount = db.CheckingAccounts.Where(c => c.AccountNumber == transfer.DestinationAccount).FirstOrDefault();
            if(destinationCheckingAccount == null)
            {
                ModelState.AddModelError("DestinationAccount", "Invalid Destination Account Number");
            }

            //add debit/credit transactions and update account balance
            
            if (ModelState.IsValid)
            {

                db.Transactions.Add(new Transaction { CheckingAccountId = transfer.CheckingAccountId, Amount = transfer.Amount });
                db.Transactions.Add(new Transaction { CheckingAccountId = destinationCheckingAccount.Id, Amount = transfer.Amount }); 
                db.SaveChanges();
                UpdateBalance(transfer.CheckingAccountId);
                UpdateBalance(destinationCheckingAccount.Id);
                return PartialView("_TransferSuccess");
                //return RedirectToAction("Account", "Home");
            }
            return PartialView("_TransferForm");
            // return View();

        }



        // GET: Transaction/Transfer
        public ActionResult QuickCash(int CheckingAccountId, decimal amount)
        {

            var sourcecheckingAccount = db.CheckingAccounts.Find(CheckingAccountId);
            var balance = sourcecheckingAccount.Balance;
            if(balance < amount)
            {
                return View("QuickCashInsufficientFunds");
            }
            db.Transactions.Add(new Transaction { CheckingAccountId = CheckingAccountId, Amount = -amount });
            db.SaveChanges();

            return View();
        }

        // POST: Transaction/Withdrawal
        [HttpPost]
        public ActionResult QuickCash(TransferViewModel withdrawal)
        {

            // TODO: Add insert logic here
            var sourcecheckingAccount = db.CheckingAccounts.Find(withdrawal.CheckingAccountId);
            if (sourcecheckingAccount.Balance > withdrawal.Amount)
            {
                ModelState.AddModelError("Amount", "You Have Insufficient Funds");
            }

            //Checking for a valid destination Account
            var destinationCheckingAccount = db.CheckingAccounts.Where(c => c.AccountNumber == withdrawal.DestinationAccount).FirstOrDefault();
            if (destinationCheckingAccount == null)
            {
                ModelState.AddModelError("DestinationAccount", "Invalid Destination Account Number");
            }

            //add debit/credit transactions and update account balance

            if (ModelState.IsValid)
            {

                db.Transactions.Add(new Transaction { CheckingAccountId = withdrawal.CheckingAccountId, Amount = withdrawal.Amount });
                db.Transactions.Add(new Transaction { CheckingAccountId = destinationCheckingAccount.Id, Amount = withdrawal.Amount });

                db.SaveChanges();
                UpdateBalance(withdrawal.CheckingAccountId);
                UpdateBalance(destinationCheckingAccount.Id);
                return RedirectToAction("Account", "Home");
            }

            return View();

        }





    }
}
