using SainsTekWepApp.Data;
using SainsTekWepApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using SainsTekWepApp.Models;
using System.IO;
using SainsTekWepApp.Enums;
using System.Drawing;

namespace SainsTekWepApp.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        const int MaxUploadFileSize = 1048576;

        protected UserManager<ApplicationUser> UserManager { get; set; }

        private ApplicationDbContext db = new ApplicationDbContext();

        public UsersController()
        {
            this.UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }

        // GET: Users/userId
        public ActionResult UserProfile()
        {
            ApplicationUser currentUser = UserManager.FindById(User.Identity.GetUserId());
            ViewBag.UserFullName = GetUserCompleteName(currentUser);
            UserProfileViewModel userProfileViewModel = GetUserProfileViewModel(currentUser);

            if (string.IsNullOrEmpty(userProfileViewModel.ParticipantPaymentConfirmMessage))
            {
                userProfileViewModel.ParticipantPaymentConfirmMessage = GlobalData.GetParticipantPaymentStatusMessage(userProfileViewModel.ParticipantPaymentStatus);
            }

            var stringQuery = Request.QueryString["InformationMessage"];

            if (stringQuery != null && stringQuery.Length > 0)
            {
                ViewBag.InformationMessage = stringQuery;
            }

            return View(userProfileViewModel);
        }

        public ActionResult EditProfile()
        {
            ApplicationUser currentUser = UserManager.FindById(User.Identity.GetUserId());

            ViewBag.UserFullName = GetUserCompleteName(currentUser);

            UserProfileViewModel userProfileViewModel = GetUserProfileViewModel(currentUser);

            return View(userProfileViewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile(UserProfileViewModel model)
        {
            ApplicationUser currentUser = UserManager.FindById(User.Identity.GetUserId());

            ViewBag.UserFullName = GetUserCompleteName(currentUser);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            currentUser.FirstName = model.FirstName;
            currentUser.MiddleName = model.MiddleName;
            currentUser.LastName = model.LastName;
            currentUser.Email = model.Email;
            currentUser.UserName = model.Email;
            currentUser.Sex = model.Sex;
            currentUser.FrontTitle = model.FrontTitle;
            currentUser.BehindTitle = model.BehindTitle;
            currentUser.PhoneNumber = model.PhoneNumber;
            currentUser.MailAddress = model.MailAddress;
            currentUser.Description = model.Description;

            //model.ParticipantPaymentStatus = currentUser.ParticipantPaymentStatus;
            //model.ParticipantPaymentConfirmMessage = currentUser.ParticipantPaymentConfirmMessage;

            try
            {
                UserManager.Update(currentUser);

                string informationMessage = "Sukses update profil.";

                return RedirectToAction("UserProfile", new { InformationMessage = informationMessage });
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;

                return View("Error");
            }
        }

        [Authorize(Roles = "Peserta")]
        public ActionResult KonfirmasiPembayaranPeserta()
        {
            ApplicationUser currentUser = UserManager.FindById(User.Identity.GetUserId());

            ViewBag.UserFullName = GetUserCompleteName(currentUser);

            PaymentConfirmationViewModel paymentConfirmationViewModel = new PaymentConfirmationViewModel();
            paymentConfirmationViewModel.TransferAmount = GlobalData.GetHargaPeserta((EPriceLevelCategory)paymentConfirmationViewModel.PriceLevelCategory);

            return View(paymentConfirmationViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Peserta")]
        public ActionResult KonfirmasiPembayaranPeserta(PaymentConfirmationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.PriceLevelCategory < 0 || model.PriceLevelCategory > 2)
            {
                ModelState.AddModelError("", "Nilai untuk kategori harga salah.");
                return View(model);
            }

            EPriceLevelCategory priceLevelCategory = (EPriceLevelCategory)model.PriceLevelCategory;

            decimal pesertaPrice = GlobalData.GetHargaPeserta(priceLevelCategory);

            if (model.TransferAmount < pesertaPrice)
            {
                ModelState.AddModelError("", string.Format("Jumlah yang harus ditransfer sebesar {0:C2}", pesertaPrice));

                //model.TransferAmount = pesertaPrice;

                return View(model);
            }

            bool includeIdentityFile = false;

            if (priceLevelCategory != EPriceLevelCategory.Dosen)
            {
                if (model.AttachmentFileUpload == null)
                {
                    ModelState.AddModelError("", "Untuk mahasiswa pasca sarjana dan mahasiswa S1 harus melampirkan bukti sebagai mahasiswa aktif");
                    return View(model);
                }

                if (model.AttachmentFileUpload.ContentLength > MaxUploadFileSize)
                {
                    ModelState.AddModelError("", string.Format("Maksimal ukuran file identitas tambahan adalah {0} bytes (1 MB)", MaxUploadFileSize));

                    return View(model);
                }

                if (model.AttachmentFileUpload.ContentType.Substring(0, 5) != "image")
                {
                    ModelState.AddModelError("", "Format file identitas tambahan tidak valid.");

                    return View(model);
                }

                includeIdentityFile = true;
            }
            else
            {
                if (model.AttachmentFileUpload != null)
                {
                    model.AttachmentFileUpload = null;
                }
            }

            if (model.TransferSlipFileUpload == null || model.TransferSlipFileUpload.ContentLength == 0)
            {
                ModelState.AddModelError("", "Belum ada bukti transfer yang diunggah.");

                return View(model);
            }

            if (model.TransferSlipFileUpload.ContentLength > MaxUploadFileSize)
            {
                ModelState.AddModelError("", string.Format("Maksimal ukuran file bukti transfer adalah {0} bytes (1 MB)", MaxUploadFileSize));

                return View(model);
            }

            if (model.TransferSlipFileUpload.ContentType.Substring(0, 5) != "image")
            {
                ModelState.AddModelError("", "Format file bukti transfer tidak valid.");

                return View(model);
            }

            ApplicationUser currentUser = UserManager.FindById(User.Identity.GetUserId());

            ViewBag.UserFullName = GetUserCompleteName(currentUser);

            try
            {
                string participantFileName = GetFileNameFromDateTime(DateTime.UtcNow, "Peserta", ".jpg");
                string participantPaymentPath = Server.MapPath(GlobalData.ParticipantPaymentConfirmationImageFileSavePath);

                if (!Directory.Exists(participantPaymentPath))
                {
                    Directory.CreateDirectory(participantPaymentPath);
                }

                string participantSaveFilePath = Path.Combine(participantPaymentPath, participantFileName);
                model.TransferSlipFileUpload.SaveAs(participantSaveFilePath);

                PembayaranPeserta pembayaranPeserta = new PembayaranPeserta()
                {
                    UserId = User.Identity.GetUserId(),
                    PriceLevelCategory = priceLevelCategory,
                    BankSenderName = model.BankSenderName,
                    TransferAmount = model.TransferAmount,
                    SlipFileName = participantFileName,
                    IsAdminConfirmed = false,
                    ConfirmationResult = false,
                    ConfirmationMessage = string.Empty,
                    UserConfirmationDate = DateTime.UtcNow,
                    Description = model.Description
                };

                if (includeIdentityFile)
                {
                    string identityFileName = GetFileNameFromDateTime(DateTime.UtcNow, "Identitas", ".jpg");
                    string identityPath = Server.MapPath(GlobalData.IdentityPaymentConfirmationImageFileSavePath);

                    if (!Directory.Exists(identityPath))
                    {
                        Directory.CreateDirectory(identityPath);
                    }

                    string identitySaveFilePath = Path.Combine(identityPath, identityFileName);
                    model.AttachmentFileUpload.SaveAs(identitySaveFilePath);
                    pembayaranPeserta.AttachmentFileName = identityFileName;
                }

                db.PembayaranPeserta.Add(pembayaranPeserta);

                currentUser.ParticipantPaymentStatus = EParticipantPaymentStatus.KonfirmasiPembayaran;
                currentUser.ParticipantPaymentConfirmMessage = GlobalData.GetParticipantPaymentStatusMessage(EParticipantPaymentStatus.KonfirmasiPembayaran);

                db.Entry(currentUser).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                //ViewBag.UserFullName = GetUserCompleteName(currentUser);
                //UserProfileViewModel userProfileViewModel = GetUserProfileViewModel(currentUser);

                //if (string.IsNullOrEmpty(userProfileViewModel.ParticipantPaymentConfirmMessage))
                //{
                //    userProfileViewModel.ParticipantPaymentConfirmMessage = GlobalData.GetParticipantPaymentStatusMessage(userProfileViewModel.ParticipantPaymentStatus);
                //}

                //ViewBag.ParticipantStatusMessage = userProfileViewModel.ParticipantPaymentConfirmMessage;

                string informationMessage = "Terima kasih. Konfirmasi pembayaran peserta anda telah terkirim.";

                return RedirectToAction("UserProfile", new { InformationMessage = informationMessage });

                //return View("UserProfile", userProfileViewModel);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;

                return View("Error");
            }
        }


        #region Helpers

        private UserProfileViewModel GetUserProfileViewModel(ApplicationUser currentUser)
        {
            UserProfileViewModel userProfileViewModel = new UserProfileViewModel
                {
                    FirstName = currentUser.FirstName,
                    MiddleName = currentUser.MiddleName,
                    LastName = currentUser.LastName,
                    Email = currentUser.Email,
                    Sex = currentUser.Sex,
                    FrontTitle = currentUser.FrontTitle,
                    BehindTitle = currentUser.BehindTitle,
                    PhoneNumber = currentUser.PhoneNumber,
                    MailAddress = currentUser.MailAddress,
                    Description = currentUser.Description,
                    ParticipantPaymentStatus = currentUser.ParticipantPaymentStatus,
                    ParticipantPaymentConfirmMessage = currentUser.ParticipantPaymentConfirmMessage
                };

            if (UserManager.IsInRole(User.Identity.GetUserId(), "Peserta"))
            {
                userProfileViewModel.UserCategory = "Peserta";
            }

            if (UserManager.IsInRole(User.Identity.GetUserId(), "Pemakalah"))
            {
                userProfileViewModel.UserCategory = "Penulis";
            }

            return userProfileViewModel;
        }

        private string GetUserCompleteName(ApplicationUser currentUser)
        {
            string userFullName = string.Format("{0} {1} {2} {3} {4}", currentUser.FrontTitle, currentUser.FirstName,
            currentUser.MiddleName, currentUser.LastName, currentUser.BehindTitle);

            return userFullName;
        }

        private string GetFileNameFromDateTime(DateTime dateTime, string prefix, string postfix)
        {
            string userName = User.Identity.GetUserName().PadRight(5, 'x').Substring(0, 5);
            return string.Format("{0}-{1:000}-{2:00}{3:00}{4:00}{5:00}{6:00}{7:000}{8}", prefix, userName, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond, postfix);
        }

        #endregion
    }
}