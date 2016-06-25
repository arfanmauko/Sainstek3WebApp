using SainsTekWepApp.Data;
using SainsTekWepApp.Entities;
using SainsTekWepApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SainsTekWepApp.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Pengumuman()
        {
            List<Pengumuman> listPengumuman = db.Pengumuman.OrderByDescending(x => x.DateCreated).ToList();

            return View(listPengumuman);
        }

        public ActionResult DetailPengumuman(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Pengumuman pengumuman = db.Pengumuman.Find(id);

            if (pengumuman == null)
            {
             return HttpNotFound();   
            }

            return View(pengumuman);
        }

        public ActionResult Panduan()
        {
            return View();
        }

        public ActionResult Jadwal()
        {
            //ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult About()
        {
            //ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Kontak kami.";

            return View();
        }

        public ActionResult Pembayaran()
        {
            ViewBag.Message = "Pembayaran.";

            return View();
        }

        public ActionResult Download(string fileName)
        {
            var filepath = Path.Combine(Server.MapPath(GlobalData.DownloadFilePath), fileName);

            try
            {
                FileInfo fileInfo = new FileInfo(filepath);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;

                return View("Error");
            }

            return File(filepath, MimeMapping.GetMimeMapping(filepath), fileName);
        }


        public ActionResult SendMessage()
        {
            MailMessageViewModel model = new MailMessageViewModel();

            return View(model);
        }

        //[HttpPost]
        //public ActionResult SendMail(MailMessageViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }
            

        //    string smtpUserName = "arfanmauko@gmail.com";
        //    string smtpPassword = "megananda";
        //    string smtpHostName = "smtp.gmail.com";
        //    int smtpPortNumber = 587;

        //    string errorMessage = string.Empty;

        //    if (SendMail(smtpUserName, smtpPassword, smtpHostName, smtpPortNumber, model.email, model.name, model.message, out errorMessage))
        //    {
        //        return View("SuccessSendMail");
        //    }

        //    ModelState.AddModelError("", errorMessage);

        //    return View(model);
        //}

        [HttpPost]
        public ActionResult SendMessage(MailMessageViewModel model)
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

            PesanPublikToAdmin pesan = new PesanPublikToAdmin();
            pesan.SenderName = model.name;
            pesan.SenderEmail = model.email;
            pesan.MessageContent = model.message;
            pesan.DateCreated = DateTime.UtcNow;

            try
            {
                db.PesanPublikToAdmin.Add(pesan);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.Message);
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        #region Helpers

        private bool SendMail(string toEmail, string subject, string body, out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                using (var smtpClient = new SmtpClient())
                {
                    string senderUserName = "arfanmauko@hotmail.com";
                    string senderPassword = "megananda";
                    smtpClient.EnableSsl = true;
                    //smtpClient.Host = "smtp.gmail.com";
                    smtpClient.Host = "smtp.live.com";
                    smtpClient.Port = 465;

                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(senderUserName, senderPassword);

                    var mailMessage = new MailMessage()
                    {
                        IsBodyHtml = true,
                        BodyEncoding = Encoding.UTF8,
                        From = new MailAddress(senderUserName),
                        Subject = subject,
                        Body = body,
                        Priority = MailPriority.Normal,
                    };

                    mailMessage.To.Add(toEmail);

                    smtpClient.Send(mailMessage);
                    //smtpClient.Send(senderUserName, senderUserName, "Just Test", "Just Test Body");

                    return true;
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }


        private bool SendMail(string smtpUserName, string smtpPassword, string smtpHostName, int smtpPortNumber, string toEmail, string subject, string body, out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                using(var smtpClient = new SmtpClient())
                {
                    smtpClient.EnableSsl = true;
                    smtpClient.Host = smtpHostName;
                    smtpClient.Port = smtpPortNumber;
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(smtpUserName, smtpPassword);

                    var mailMessage = new MailMessage()
                    {
                        IsBodyHtml = true,
                        //BodyEncoding = Encoding.UTF8,
                        From = new MailAddress(smtpUserName),
                        Subject = subject,
                        Body = body,
                        Priority = MailPriority.Normal,
                        
                    };

                    mailMessage.To.Add(toEmail);

                    smtpClient.Send(mailMessage);

                    return true;
                }
            }
            catch(Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }

        #endregion
    }
}