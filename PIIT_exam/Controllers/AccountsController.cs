using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PIIT_exam.Data;
using PIIT_exam.Models;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace PIIT_exam.Controllers
{
    public class AccountsController : Controller
    {
        private PIIT_examContext db = new PIIT_examContext();

        // GET: Accounts
        public ActionResult Index()
        {
            return View(db.Accounts.ToList());
        }

        // GET: Accounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // GET: Accounts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserName,PassWord,Email,Phone,Status")] Account account)
        {
            if (ModelState.IsValid)
            {
                var random = new Random();
                account.Status = Account.AccountStatus.INACTICE;
          
                var newAccount = db.Accounts.Add(account);

                db.SaveChanges();
                var code = new VerificationCode()
                {
                    AccountId = newAccount.Id,
                    Code = random.Next(100000, 999999).ToString()
                };

                var newCode = db.VerificationCodes.Add(code);

                var accountSid = "AC6f7db6a784b209ec8a1a9b9da19c68ce";
                var authToken = "[AuthToken]";
                TwilioClient.Init(accountSid, authToken);
             
                var messageOptions = new CreateMessageOptions(
                    new PhoneNumber(newAccount.Phone));
                messageOptions.MessagingServiceSid = "MG1c7f4e78f0273088ca8993888a73d3e5";
                messageOptions.Body = "Your Verification Code : " + newCode.Code;

                var message = MessageResource.Create(messageOptions);
                Console.WriteLine(message.Body);
                return View("VerificationCode");
                //return RedirectToAction("Index");
            }

            return View(account);
        }

    
        public ActionResult VerificationCode(int id)
        {

            return View(db.VerificationCodes.Find(id));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VerificationCode(int Id , VerificationCode verificationCode)
        {
            if (ModelState.IsValid)
            {
                if(verificationCode.Code == db.VerificationCodes.Where(i => i.AccountId == verificationCode.AccountId).FirstOrDefault().Code)
                {
                    return View();
                }
                return View(verificationCode);

            }
            return View(verificationCode);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
