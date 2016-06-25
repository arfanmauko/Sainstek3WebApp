using Microsoft.AspNet.Identity;
using SainsTekWepApp.Areas.Admin.Models;
using SainsTekWepApp.Data;
using SainsTekWepApp.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web.Mvc;
using Humanizer;

namespace SainsTekWepApp.Areas.Admin.Controllers
{
    public class PublicMessageController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        //private string shareInboxViewName = "PublicInboxMessages";

        // GET: Admin/PublicMessage

        public ActionResult Index(string messageType)
        {
            //messageType 0 = inbox, i = unread, 2 = unreply
            switch (messageType)
            {
                case "inbox":
                    ViewBag.Action = "PublicInboxMessages";
                    break;
                case "unread":
                    ViewBag.Action = "PublicUnReadMessages";
                    break;
                case "unreply":
                    ViewBag.Action = "PublicUnReplyMessages";
                    break;
                default:
                    return new HttpStatusCodeResult((int)HttpStatusCode.BadRequest);
            }

            return View();
        }

        public ActionResult PublicInboxMessages()
        {
            ViewBag.PublicMessageAction = "PublicInboxMessages";

            return PartialView("_PublicInMessagesPartial");
        }

        [HttpPost]
        public ActionResult PublicInboxMessages(int current, int rowCount, string sortBy, string sortDirection, string searchPhrase)
        {
            IEnumerable<PublicMessageViewModel> model = null;

            if (!string.IsNullOrEmpty(searchPhrase) && searchPhrase.Trim().Length > 0)
            {
                model = (from m in db.PesanPublikToAdmin
                         where m.SenderName.Contains(searchPhrase) ||
                         m.SenderEmail.Contains(searchPhrase) ||
                         m.MessageContent.Contains(searchPhrase)
                         select new PublicMessageViewModel
                         {
                             Id = m.Id,
                             SenderName = m.SenderName,
                             SenderEmail = m.SenderEmail,
                             MessageContent = m.MessageContent,
                             AlreadyReply = m.DateReply != null
                         }).ToList();
            }
            else
            {
                model = (from m in db.PesanPublikToAdmin
                         select new PublicMessageViewModel
                         {
                             Id = m.Id,
                             SenderName = m.SenderName,
                             SenderEmail = m.SenderEmail,
                             MessageContent = m.MessageContent,
                             AlreadyReply = m.DateReply != null
                         }).ToList();
            }

            return GetPublicMessageActionResult(ref current, rowCount, sortBy, sortDirection, ref model);
        }

        public ActionResult PublicUnReadMessages()
        {
            ViewBag.PublicMessageAction = "PublicUnReadMessages";

            return PartialView("_PublicInMessagesPartial");
        }

        [HttpPost]
        public ActionResult PublicUnReadMessages(int current, int rowCount, string sortBy, string sortDirection, string searchPhrase)
        {
            IEnumerable<PublicMessageViewModel> model = null;

            if (!string.IsNullOrEmpty(searchPhrase) && searchPhrase.Trim().Length > 0)
            {
                model = (from m in db.PesanPublikToAdmin
                         where m.DateAdminRead == null && (m.SenderName.Contains(searchPhrase) ||
                         m.SenderEmail.Contains(searchPhrase) ||
                         m.MessageContent.Contains(searchPhrase))
                         select new PublicMessageViewModel
                         {
                             Id = m.Id,
                             SenderName = m.SenderName,
                             SenderEmail = m.SenderEmail,
                             MessageContent = m.MessageContent,
                             AlreadyReply = m.DateReply != null
                         }).ToList();
            }
            else
            {
                model = (from m in db.PesanPublikToAdmin
                         where m.DateAdminRead == null
                         select new PublicMessageViewModel
                         {
                             Id = m.Id,
                             SenderName = m.SenderName,
                             SenderEmail = m.SenderEmail,
                             MessageContent = m.MessageContent,
                             AlreadyReply = m.DateReply != null
                         }).ToList();
            }

            return GetPublicMessageActionResult(ref current, rowCount, sortBy, sortDirection, ref model);
        }

        public ActionResult PublicUnReplyMessages()
        {
            ViewBag.PublicMessageAction = "PublicUnReplyMessages";

            return PartialView("_PublicInMessagesPartial");
        }

        [HttpPost]
        public ActionResult PublicUnReplyMessages(int current, int rowCount, string sortBy, string sortDirection, string searchPhrase)
        {
            IEnumerable<PublicMessageViewModel> model = null;

            if (!string.IsNullOrEmpty(searchPhrase) && searchPhrase.Trim().Length > 0)
            {
                model = (from m in db.PesanPublikToAdmin
                         where m.DateReply == null && (m.SenderName.Contains(searchPhrase) ||
                         m.SenderEmail.Contains(searchPhrase) ||
                         m.MessageContent.Contains(searchPhrase))
                         select new PublicMessageViewModel
                         {
                             Id = m.Id,
                             SenderName = m.SenderName,
                             SenderEmail = m.SenderEmail,
                             MessageContent = m.MessageContent,
                             AlreadyReply = m.DateReply != null
                         }).ToList();
            }
            else
            {
                model = (from m in db.PesanPublikToAdmin
                         where m.DateReply == null
                         select new PublicMessageViewModel
                         {
                             Id = m.Id,
                             SenderName = m.SenderName,
                             SenderEmail = m.SenderEmail,
                             MessageContent = m.MessageContent,
                             AlreadyReply = m.DateReply != null
                         }).ToList();
            }

            return GetPublicMessageActionResult(ref current, rowCount, sortBy, sortDirection, ref model);
        }

        public ActionResult PublicDetailsMessage(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            PesanPublikToAdmin pesanPublik = db.PesanPublikToAdmin.Find(id);

            if (pesanPublik == null)
            {
                return HttpNotFound();
            }

            try
            {
                pesanPublik.DateAdminRead = DateTime.UtcNow;
                db.Entry(pesanPublik).State = EntityState.Modified;
                db.SaveChanges();

                DetailPublicMessageViewModel detailPublicMessage = new DetailPublicMessageViewModel
                {
                    Id = pesanPublik.Id,
                    SenderName = pesanPublik.SenderName,
                    SenderEmail = pesanPublik.SenderEmail,
                    MessageContent = pesanPublik.MessageContent,
                    DateCreated = pesanPublik.DateCreated,
                    DateAdminRead = pesanPublik.DateAdminRead,
                    ReplyMessage = pesanPublik.ReplyMessage,
                    DateReply = pesanPublik.DateReply,
                    ReplyAdminId = pesanPublik.ReplyAdminId,
                    AlreadyReply = pesanPublik.DateReply != null
                };

                return PartialView(detailPublicMessage);
            }
            catch(Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        public ActionResult ReplyMessage(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            PesanPublikToAdmin pesanPublik = db.PesanPublikToAdmin.Find(id);

            if (pesanPublik == null)
            {
                return HttpNotFound();
            }

            ReplyPublicMessageViewModel createReplyMessage = new ReplyPublicMessageViewModel
            {
                Id = pesanPublik.Id,
                MessageContent = pesanPublik.MessageContent,
                ReplyToEmail = pesanPublik.SenderEmail,
                ReplyToName = pesanPublik.SenderName,
                ReplySubject = string.Format("Reply Message from {0} at {1} {2} GMT at www.sainstek3undana.com", pesanPublik.SenderName, pesanPublik.DateCreated.ToLongDateString(), pesanPublik.DateCreated.ToLongTimeString()),
                ReplyMessage = string.Empty
            };

            return PartialView(createReplyMessage);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReplyMessage(ReplyPublicMessageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0)
                           .ToList();

                string errorMessages = "<ul>";

                for (int i = 0; i < errors.Count; i++)
                {
                    ModelErrorCollection modelErrorCollection = errors[i];

                    for (int j = 0; j < modelErrorCollection.Count; j++)
                    {
                        if (!string.IsNullOrEmpty(modelErrorCollection[j].ErrorMessage))
                        {
                            errorMessages += "<li>";
                            errorMessages += modelErrorCollection[j].ErrorMessage;
                            errorMessages += "</li>";
                        }
                    }
                }

                errorMessages += "</ul>";

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, errorMessages);
            }

            PesanPublikToAdmin pesanPublik = db.PesanPublikToAdmin.Find(model.Id);

            if (pesanPublik == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Entity not found.");
            }

            string emailSendErrorMessage = string.Empty;

            if (!SendMailFromPanitiaEmail(model.ReplyToEmail, model.ReplySubject, model.ReplyMessage, out emailSendErrorMessage))
            {
                ModelState.AddModelError("", emailSendErrorMessage);

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, emailSendErrorMessage);
            }
            try
            {
                if (pesanPublik.DateAdminRead == null)
                {
                    pesanPublik.DateAdminRead = DateTime.UtcNow;
                }

                pesanPublik.DateReply = DateTime.UtcNow;
                pesanPublik.ReplyMessage = model.ReplyMessage;
                pesanPublik.ReplyAdminId = User.Identity.GetUserId();

                db.Entry(pesanPublik).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.Message);
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }


        #region Helpers

        private ActionResult GetPublicMessageActionResult(ref int current, int rowCount, string sortBy, string sortDirection, ref IEnumerable<PublicMessageViewModel> model)
        {
            int recordsCount = model.Count();

            if (!string.IsNullOrEmpty(sortBy) && !string.IsNullOrEmpty(sortDirection))
            {
                if (sortDirection == "desc")
                {
                    switch (sortBy)
                    {
                        case "Id":
                            model = model.OrderByDescending(x => x.Id);
                            break;
                        case "SenderEmail":
                            model = model.OrderByDescending(x => x.SenderEmail);
                            break;
                        case "SenderName":
                            model = model.OrderByDescending(x => x.SenderName);
                            break;
                        case "MessageContent":
                            model = model.OrderByDescending(x => x.MessageContent);
                            break;
                    }
                }
                else
                {
                    switch (sortBy)
                    {
                        case "Id":
                            model = model.OrderBy(x => x.Id);
                            break;
                        case "SenderEmail":
                            model = model.OrderBy(x => x.SenderEmail);
                            break;
                        case "SenderName":
                            model = model.OrderBy(x => x.SenderName);
                            break;
                        case "MessageContent":
                            model = model.OrderBy(x => x.MessageContent);
                            break;
                    }
                }
            }

            if (recordsCount == 0)
            {
                current = 0;
            }

            var result = new { current = current, rowCount = rowCount, rows = model.Skip((current - 1) * rowCount).Take(rowCount), total = recordsCount };

            return Json(result);
        }

        public bool SendMailFromPanitiaEmail(string toEmail, string subject, string body, out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                using (var smtpClient = new SmtpClient())
                {
                    //Todo : Email ke panitia
                    //string senderUserName = "panitia.sainstek@gmail.com";
                    //string senderPassword = "onoonoae";
                    string senderUserName = "arfanmauko@gmail.com";
                    string senderPassword = "megananda";
                    string smtpHost = "smtp.gmail.com";
                    smtpClient.EnableSsl = true;
                    smtpClient.Host = smtpHost;
                    smtpClient.Port = 587;
                    smtpClient.UseDefaultCredentials = true;
                    smtpClient.Credentials = new NetworkCredential(senderUserName, senderPassword);
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                    var mailMessage = new MailMessage()
                    {
                        IsBodyHtml = true,
                        BodyEncoding = Encoding.UTF8,
                        From = new MailAddress(senderUserName),
                        Subject = subject,
                        Body = body,
                        Priority = MailPriority.Normal,
                    };

                    mailMessage.To.Add(new MailAddress(toEmail));
                    smtpClient.Send(mailMessage);

                    return true;
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;

                return false;
            }
        }

        #endregion
    }
}