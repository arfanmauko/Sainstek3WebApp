using Microsoft.AspNet.Identity;
using SainsTekWepApp.Areas.Admin.Models;
using SainsTekWepApp.Data;
using SainsTekWepApp.Entities;
using SainsTekWepApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SainsTekWepApp.Controllers
{
    [Authorize]
    public class MessageController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: Message
        public ActionResult Index()
        {
            string informationMessage = Request.Params["InformationMessage"];

            if (!string.IsNullOrEmpty(informationMessage))
            {
                ViewBag.InformationMessage = informationMessage;
            }

            return View();
        }

        public ActionResult GetInboxMessages()
        {
            var messageToUser = (from m in db.PesanAdminToUser.AsEnumerable()
                                 select new MessageToUserViewModel
                                 {
                                     Id = m.Id,
                                     AdminId = m.AdminId,
                                     MessageTitle = m.MessageTitle,
                                     MessageContent = m.MessageContent,
                                     DateCreated = m.DateCreated,
                                     UserId = m.UserId,
                                     DateRead = m.DateRead
                                 });

            ViewBag.EmptyMessage = "Belum ada pesan dalam kotak pesan.";

            return PartialView("_MessageToUserPartial", messageToUser);
        }

        public ActionResult GetReadMessages()
        {
            var messageToUser = (from m in db.PesanAdminToUser.AsEnumerable()
                                 where m.DateRead != null
                                 select new MessageToUserViewModel
                                 {
                                     Id = m.Id,
                                     AdminId = m.AdminId,
                                     MessageTitle = m.MessageTitle,
                                     MessageContent = m.MessageContent,
                                     DateCreated = m.DateCreated,
                                     UserId = m.UserId,
                                     DateRead = m.DateRead
                                 });

            ViewBag.EmptyMessage = "Belum ada pesan yang sudah dibaca.";

            return PartialView("_MessageToUserPartial", messageToUser);
        }

        public ActionResult GetUnReadMessages()
        {
            var messageToUser = (from m in db.PesanAdminToUser.AsEnumerable()
                                 where m.DateRead == null
                                 select new MessageToUserViewModel
                                 {
                                     Id = m.Id,
                                     AdminId = m.AdminId,
                                     MessageTitle = m.MessageTitle,
                                     MessageContent = m.MessageContent,
                                     DateCreated = m.DateCreated,
                                     UserId = m.UserId,
                                     DateRead = m.DateRead
                                 });

            ViewBag.EmptyMessage = "Belum ada pesan yang blum dibaca.";

            return PartialView("_MessageToUserPartial", messageToUser);
        }

        public ActionResult SendMessage()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SendMessage(SendMessageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                PesanUserToAdmin pesanUser = new PesanUserToAdmin();
                pesanUser.UserId = User.Identity.GetUserId();
                pesanUser.MessageTitle = model.MessageTitle;
                pesanUser.MessageContent = model.MessageContent;
                pesanUser.DateCreated = DateTime.UtcNow;

                db.PesanUserToAdmin.Add(pesanUser);
                db.SaveChanges();

                string informationMessage = "Pesan telah dikirim.";
                return RedirectToAction("Index", new { InformationMessage = informationMessage });
            }
            catch(Exception ex)
            {
                string errorMessage = ex.Message;
                ModelState.AddModelError("", ex.Message);

                return View(model);
            }
        }

    }
}