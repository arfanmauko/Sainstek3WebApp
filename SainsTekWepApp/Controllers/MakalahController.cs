using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SainsTekWepApp.Data;
using SainsTekWepApp.Entities;
using SainsTekWepApp.Enums;
using SainsTekWepApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SainsTekWepApp.Controllers
{

    [Authorize(Roles = "Pemakalah")]
    public class MakalahController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        const int MaxUploadFileSize = 1048576;

        protected UserManager<ApplicationUser> UserManager { get; set; }

        public MakalahController()
        {
            this.UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }

        // GET: Makalah
        public ActionResult Index()
        {
            var stringQuery = Request.QueryString["InformationMessage"];

            if (stringQuery != null && stringQuery.Length > 0)
            {
                ViewBag.InformationMessage = stringQuery;
            }

            try
            {
                IEnumerable<ListMakalahViewModel> listUserPaper = GetListMakalahViewModel();
                ViewBag.UserFullName = GetUserCompleteName();

                return View(listUserPaper);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }

        // GET: Makalah/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Makalah makalah = db.Makalah.Find(id);

            if (makalah == null)
            {
                return HttpNotFound();
            }

            DetailMakalahViewModel detailMakalahViewModel = new DetailMakalahViewModel
            {
                Id = makalah.Id,
                CategoryId = makalah.CategoryId,
                CategoryName = db.KategoriMakalah.Where(x => x.Id == makalah.CategoryId).Select(x => x.Name).FirstOrDefault(),
                Title = makalah.Title,
                Abstract = makalah.Abstract,
                DateSubmitted = makalah.DateSubmitted,
                DateModified = makalah.DateModified,
                AbstractFileName = makalah.AbstractFileName,
                Description = makalah.Description,
                PaperAbstractStatus = makalah.PaperAbstractStatus,
                PaperAbstractConfirmMessage = makalah.PaperAbstractConfirmMessage,
                FullPaperFileName = makalah.FullPaperFileName,
                FullPaperDateSubmitted = makalah.FullPaperDateSubmitted,
                FullPaperStatus = makalah.FullPaperStatus,
                FullPaperConfirmMessage = makalah.FullPaperConfirmMessage,
                PaperPaymentStatus = makalah.PaperPaymentStatus,
                PaperPaymentConfirmMessage = makalah.PaperPaymentConfirmMessage
            };

            bool allSuccess = detailMakalahViewModel.PaperAbstractStatus == EPaperAbstractStatus.Diterima &&
                detailMakalahViewModel.FullPaperStatus == EFullPaperStatus.Diterima && detailMakalahViewModel.PaperPaymentStatus == EPaperPaymentStatus.Diterima;

            ViewBag.AlertType = "alert alert-warning";

            if (allSuccess)
            {
                ViewBag.AlertType = "alert alert-success";
            }

            return View(detailMakalahViewModel);
        }

        // GET: Makalah/Create
        public ActionResult Create()
        {
            ViewBag.UserFullName = GetUserCompleteName();

            CreateMakalahViewModel newMakalah = new CreateMakalahViewModel();

            return View(newMakalah);
        }

        // POST: Makalah/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateMakalahViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            int titleWordsCount = GetWordsCount(model.Title);

            if (titleWordsCount > GlobalData.TitleMaxWordsCount)
            {
                ModelState.AddModelError("", string.Format("Jumlah kata judul anda {0}. Maksimal untuk jumlah kata untuk judul adalah {1}.", titleWordsCount, GlobalData.TitleMaxWordsCount));

                return View(model);
            }

            int abstractWordsCount = GetWordsCount(model.Abstract);

            if (abstractWordsCount > GlobalData.AbstractMaxWordsCount)
            {
                ModelState.AddModelError("", string.Format("Jumlah kata abstrak anda {0}. Maksimal untuk jumlah kata untuk abstrak adalah {1}.", abstractWordsCount, GlobalData.AbstractMaxWordsCount));

                return View(model);
            }

            if (model.AbstractFileUpload.ContentType != "application/pdf")
            {
                ModelState.AddModelError("File", "Format file abstrak yang diunggah harus bertipe pdf.");
                return View("Create", model);
            }

            bool includeFullPaper = false;

            if (model.FullPaperFileUpload != null && model.FullPaperFileUpload.ContentLength > 0)
            {
                if (model.FullPaperFileUpload.ContentType != "application/pdf")
                {
                    ModelState.AddModelError("File", "Format file full paper yang diunggah harus bertipe pdf.");
                    return View("Create", model);
                }

                if (model.FullPaperFileUpload.ContentLength > MaxUploadFileSize)
                {
                    ModelState.AddModelError("File", string.Format("Maksimal ukuran file full paper adalah {0} bytes (1 MB)", MaxUploadFileSize));
                    return View("Create", model);
                }

                includeFullPaper = true;
            }

            try
            {
                string abstractFileName = GetFileNameFromDateTime(DateTime.UtcNow, "Abs", ".pdf");
                string abstractPath = Server.MapPath(GlobalData.AbstractFileSavePath);

                if (!Directory.Exists(abstractPath))
                {
                    Directory.CreateDirectory(abstractPath);
                }

                string abstractSaveFilePath = Path.Combine(abstractPath, abstractFileName);
                model.AbstractFileUpload.SaveAs(abstractSaveFilePath);

                Makalah newMakalah = new Makalah
                {
                    userId = User.Identity.GetUserId(),
                    CategoryId = model.CategoryId,
                    Title = model.Title,
                    Abstract = model.Abstract,
                    AbstractFileName = abstractFileName,
                    Description = model.Description,
                    PaperAbstractStatus = Enums.EPaperAbstractStatus.Baru,
                    PaperAbstractConfirmMessage = GlobalData.GetPaperAbstractStatusMessage(EPaperAbstractStatus.Baru),
                    FullPaperStatus = Enums.EFullPaperStatus.Kosong,
                    FullPaperConfirmMessage = GlobalData.GetFullPaperStatusMessage(EFullPaperStatus.Kosong),
                    PaperPaymentStatus = EPaperPaymentStatus.BelumBayar,
                    PaperPaymentConfirmMessage = GlobalData.GetPaperPaymentStatusMessage(EPaperPaymentStatus.BelumBayar),
                    DateModified = DateTime.UtcNow
                };

                if (includeFullPaper)
                {
                    string fullPaperFileName = GetFileNameFromDateTime(DateTime.UtcNow, "Ppr", ".pdf");
                    string fullPaperPath = Server.MapPath(GlobalData.PaperFileSavePath);

                    if (!Directory.Exists(fullPaperPath))
                    {
                        Directory.CreateDirectory(fullPaperPath);
                    }

                    string fullPaperSaveFilePath = Path.Combine(fullPaperPath, fullPaperFileName);
                    model.FullPaperFileUpload.SaveAs(fullPaperSaveFilePath);

                    newMakalah.FullPaperDateSubmitted = DateTime.UtcNow;
                    newMakalah.FullPaperFileName = fullPaperFileName;
                    newMakalah.FullPaperStatus = Enums.EFullPaperStatus.Baru;
                    newMakalah.FullPaperConfirmMessage = GlobalData.GetFullPaperStatusMessage(EFullPaperStatus.Baru);
                }

                db.Makalah.Add(newMakalah);
                db.SaveChanges();

                string informationMessage = "Makalah berhasil ditambahkan.";

                return RedirectToAction("Index", new { InformationMessage = informationMessage });
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;

                return View("Error");
            }
        }

        // GET: Makalah/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Makalah makalah = db.Makalah.Find(id);

            if (makalah == null)
            {
                return HttpNotFound();
            }

            EditMakalahViewModel editMakalahViewModel = new EditMakalahViewModel
            {
                Id = makalah.Id,
                CategoryId = makalah.CategoryId,
                Title = makalah.Title,
                Abstract = makalah.Abstract,
                AbstractFileName = makalah.AbstractFileName,
                FullPaperFileName = makalah.FullPaperFileName,
                Description = makalah.Description
            };

            return View(editMakalahViewModel);
        }

        // POST: Makalah/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditMakalahViewModel model)
        {
            Makalah makalah = db.Makalah.Find(model.Id);

            if (makalah == null)
            {
                return HttpNotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            int titleWordsCount = GetWordsCount(model.Title);

            if (titleWordsCount > GlobalData.TitleMaxWordsCount)
            {
                ModelState.AddModelError("", string.Format("Jumlah kata judul anda {0}. Maksimal untuk jumlah kata untuk judul adalah {1}.", titleWordsCount, GlobalData.TitleMaxWordsCount));

                return View(model);
            }

            int abstractWordsCount = GetWordsCount(model.Abstract);

            if (abstractWordsCount > GlobalData.AbstractMaxWordsCount)
            {
                ModelState.AddModelError("", string.Format("Jumlah kata abstrak anda {0}. Maksimal untuk jumlah kata untuk abstrak adalah {1}.", abstractWordsCount, GlobalData.AbstractMaxWordsCount));

                return View(model);
            }

            bool includeNewAbstractFile = false;

            if (model.AbstractFileUpload != null && model.AbstractFileUpload.ContentLength > 0)
            {
                if (model.AbstractFileUpload.ContentType != "application/pdf")
                {
                    ModelState.AddModelError("", "Format file abstrak yang diunggah harus bertipe pdf.");

                    return View("Edit", model);
                }

                if (model.AbstractFileUpload.ContentLength > MaxUploadFileSize)
                {
                    ModelState.AddModelError("", string.Format("Maksimal ukuran file abstrak adalah {0} bytes (1 MB)", MaxUploadFileSize));

                    return View("Edit", model);
                }

                includeNewAbstractFile = true;
            }

            bool includeNewFullPaper = false;

            if (model.FullPaperFileUpload != null && model.FullPaperFileUpload.ContentLength > 0)
            {
                if (model.FullPaperFileUpload.ContentType != "application/pdf")
                {
                    ModelState.AddModelError("", "Format file full paper yang diunggah harus bertipe pdf.");

                    return View("Edit", model);
                }

                if (model.FullPaperFileUpload.ContentLength > MaxUploadFileSize)
                {
                    ModelState.AddModelError("", string.Format("Maksimal ukuran file full paper adalah {0} bytes (1 MB)", MaxUploadFileSize));

                    return View("Edit", model);
                }

                includeNewFullPaper = true;
            }

            try
            {
                string newAbstractFileName = makalah.AbstractFileName;


                if (includeNewAbstractFile)
                {
                    newAbstractFileName = GetFileNameFromDateTime(DateTime.UtcNow, "Abs", ".pdf");
                    string abstractPath = Server.MapPath(GlobalData.AbstractFileSavePath);

                    if (!Directory.Exists(abstractPath))
                    {
                        Directory.CreateDirectory(abstractPath);
                    }

                    string abstractSaveFilePath = Path.Combine(abstractPath, newAbstractFileName);
                    model.AbstractFileUpload.SaveAs(abstractSaveFilePath);

                    //hapus file yang lama
                    if (!string.IsNullOrEmpty(makalah.AbstractFileName))
                    {
                        string oldFileName = makalah.AbstractFileName;
                        FileInfo oldAbstract = new FileInfo(Path.Combine(abstractPath, oldFileName));

                        if (oldAbstract.Exists)
                        {
                            oldAbstract.Delete();
                        }
                    }
                }

                makalah.CategoryId = model.CategoryId;
                makalah.Title = model.Title;
                makalah.Abstract = model.Abstract;
                makalah.AbstractFileName = newAbstractFileName;
                makalah.Description = model.Description;
                makalah.PaperAbstractStatus = EPaperAbstractStatus.Edit;
                makalah.PaperAbstractConfirmMessage = GlobalData.GetPaperAbstractStatusMessage(EPaperAbstractStatus.Edit);
                makalah.DateModified = DateTime.UtcNow;

                string newFullPaperFileName = makalah.FullPaperFileName;

                if (includeNewFullPaper)
                {
                    newFullPaperFileName = GetFileNameFromDateTime(DateTime.UtcNow, "Ppr", ".pdf");
                    string fullPaperPath = Server.MapPath(GlobalData.PaperFileSavePath);

                    if (!Directory.Exists(fullPaperPath))
                    {
                        Directory.CreateDirectory(fullPaperPath);
                    }

                    string fullPaperSaveFilePath = Path.Combine(fullPaperPath, newFullPaperFileName);
                    model.FullPaperFileUpload.SaveAs(fullPaperSaveFilePath);

                    //hapus file yang lama
                    if (!string.IsNullOrEmpty(makalah.FullPaperFileName))
                    {
                        string oldFileName = makalah.FullPaperFileName;
                        FileInfo oldFullPaper = new FileInfo(Path.Combine(fullPaperPath, oldFileName));

                        if (oldFullPaper.Exists)
                        {
                            oldFullPaper.Delete();
                        }
                    }

                    makalah.FullPaperFileName = newFullPaperFileName;
                    //Jika Full Paper baru ditambahkan
                    if (makalah.FullPaperDateSubmitted == null)
                    {
                        makalah.FullPaperStatus = Enums.EFullPaperStatus.Baru;
                        makalah.FullPaperConfirmMessage = GlobalData.GetFullPaperStatusMessage(EFullPaperStatus.Baru);
                        makalah.FullPaperDateSubmitted = DateTime.UtcNow;
                    }
                    else
                    {
                        makalah.FullPaperStatus = Enums.EFullPaperStatus.Edit;
                        makalah.FullPaperConfirmMessage = GlobalData.GetFullPaperStatusMessage(EFullPaperStatus.Edit);
                    }
                }

                db.Entry(makalah).State = EntityState.Modified;
                db.SaveChanges();

                string informationMessage = "Makalah telah diupdate.";
                return RedirectToAction("Index", new { InformationMessage = informationMessage });

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;

                return View("Error");
            }
        }

        // GET: Makalah/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Makalah makalah = db.Makalah.Find(id);

            if (makalah == null)
            {
                return HttpNotFound();
            }

            DeleteMakalahViewModel deleteMakalahViewModel = new DeleteMakalahViewModel
            {
                Id = makalah.Id,
                CategoryId = makalah.CategoryId,
                CategoryName = db.KategoriMakalah.Where(x => x.Id == makalah.CategoryId).Select(x => x.Name).FirstOrDefault(),
                Title = makalah.Title,
                Abstract = makalah.Abstract,
                AbstractFileName = makalah.AbstractFileName,
                FullPaperFileName = makalah.FullPaperFileName,
                PaperPaymentStatus = makalah.PaperPaymentStatus,
                PaperPaymentConfirmMessage = makalah.PaperPaymentConfirmMessage,
                DateSubmitted = makalah.DateSubmitted
            };

            return View(deleteMakalahViewModel);
        }

        // POST: Makalah/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Makalah makalah = db.Makalah.Find(id);
                db.Makalah.Remove(makalah);
                db.SaveChanges();

                //hapus file abstrak yang lama
                if (!string.IsNullOrEmpty(makalah.AbstractFileName))
                {
                    string oldFileName = makalah.AbstractFileName;
                    string abstractPath = Server.MapPath(GlobalData.AbstractFileSavePath);
                    FileInfo oldAbstract = new FileInfo(Path.Combine(abstractPath, oldFileName));

                    if (oldAbstract.Exists)
                    {
                        oldAbstract.Delete();
                    }
                }

                //hapus file full paper yang lama
                if (!string.IsNullOrEmpty(makalah.FullPaperFileName))
                {
                    string oldFileName = makalah.FullPaperFileName;
                    string fullPaperPath = Server.MapPath(GlobalData.PaperFileSavePath);
                    FileInfo oldFullPaper = new FileInfo(Path.Combine(fullPaperPath, oldFileName));

                    if (oldFullPaper.Exists)
                    {
                        oldFullPaper.Delete();
                    }
                }

                string informationMessage = "Makalah telah dihapus.";
                return RedirectToAction("Index", new { InformationMessage = informationMessage });
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;

                return View("Error");
            }

        }

        public ActionResult DownloadAbstract(string fileName)
        {
            var filepath = Path.Combine(Server.MapPath(GlobalData.AbstractFileSavePath), fileName);

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

        public ActionResult DownloadFullPaper(string fileName)
        {
            var filepath = Path.Combine(Server.MapPath(GlobalData.PaperFileSavePath), fileName);

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

        public ActionResult KonfirmasiPembayaranMakalah(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Makalah makalah = db.Makalah.Find(id);

            if (makalah == null)
            {
                return HttpNotFound();
            }

            PaymentConfirmationViewModel paymentConfirmationViewModel = new PaymentConfirmationViewModel();
            paymentConfirmationViewModel.MakalahId = makalah.Id;
            //paymentConfirmationViewModel.MinTransferAmount = 300000;
            //paymentConfirmationViewModel.MaxTransferAmount = 250000;

            //paymentConfirmationViewModel.StringMinTransferAmount = paymentConfirmationViewModel.MinTransferAmount.ToString("C2");
            //paymentConfirmationViewModel.StringMaxTransferAmount = paymentConfirmationViewModel.MaxTransferAmount.ToString("C2");


            string userId = User.Identity.GetUserId();
            int paperAlreadyPaidCount = (from m in db.Makalah
                                         where m.userId == userId &&
                                         m.PaperPaymentStatus == EPaperPaymentStatus.Diterima
                                         select m).Count();

            bool moreThanOnePaper = paperAlreadyPaidCount > 0;

            paymentConfirmationViewModel.TransferAmount = GlobalData.GetHargaMakalah((EPriceLevelCategory)paymentConfirmationViewModel.PriceLevelCategory, moreThanOnePaper);

            ViewBag.MoreThanOne = moreThanOnePaper;

            ViewBag.UserFullName = GetUserCompleteName();
            ViewBag.MakalahTitle = makalah.Title;

            return View(paymentConfirmationViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult KonfirmasiPembayaranMakalah(PaymentConfirmationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.PriceLevelCategory < 0 || model.PriceLevelCategory > 1)
            {
                ModelState.AddModelError("", "Nilai untuk kategori harga salah.");
                return View(model);
            }

            EPriceLevelCategory priceLevelCategory = (EPriceLevelCategory)model.PriceLevelCategory;

            string userId = User.Identity.GetUserId();

            int paperAlreadyPaidCount = (from m in db.Makalah
                                         where m.userId == userId &&
                                         m.PaperPaymentStatus == EPaperPaymentStatus.Diterima
                                         select m).Count();

            decimal makalahPrice = GlobalData.GetHargaMakalah(priceLevelCategory, paperAlreadyPaidCount > 0);

            if (model.TransferAmount < makalahPrice)
            {
                ModelState.AddModelError("", string.Format("Jumlah yang harus ditransfer sebesar {0:C2}", makalahPrice));

                //model.TransferAmount = makalahPrice;

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

            Makalah makalah = db.Makalah.Find(model.MakalahId);

            if (makalah == null)
            {
                return HttpNotFound();
            }

            try
            {
                string imageTransferFileName = GetFileNameFromDateTime(DateTime.UtcNow, "Ppr", ".jpg");
                string paymentPath = Server.MapPath(GlobalData.PaperPaymentConfirmationImageFileSavePath);

                if (!Directory.Exists(paymentPath))
                {
                    Directory.CreateDirectory(paymentPath);
                }

                string paymentSaveFilePath = Path.Combine(paymentPath, imageTransferFileName);
                model.TransferSlipFileUpload.SaveAs(paymentSaveFilePath);

                PembayaranMakalah pembayaranMakalah = new PembayaranMakalah()
                {
                    MakalahId = model.MakalahId,
                    PriceLevelCategory = priceLevelCategory,
                    BankSenderName = model.BankSenderName,
                    TransferAmount = model.TransferAmount,
                    SlipFileName = imageTransferFileName,
                    IsAdminConfirmed = false,
                    ConfirmationResult = false,
                    ConfirmationMessage = string.Empty,
                    UserConfirmationDate = DateTime.UtcNow,
                    Description = model.Description
                };

                if (includeIdentityFile)
                {
                    string identityFileName = GetFileNameFromDateTime(DateTime.UtcNow, "Id", ".jpg");
                    string identityPath = Server.MapPath(GlobalData.IdentityPaymentConfirmationImageFileSavePath);

                    if (!Directory.Exists(identityPath))
                    {
                        Directory.CreateDirectory(identityPath);
                    }

                    string identitySaveFilePath = Path.Combine(identityPath, identityFileName);
                    model.AttachmentFileUpload.SaveAs(identitySaveFilePath);
                    pembayaranMakalah.AttachmentFileName = identityFileName;
                }

                db.PembayaranMakalah.Add(pembayaranMakalah);

                makalah.PaperPaymentStatus = EPaperPaymentStatus.KonfirmasiPembayaran;
                makalah.PaperPaymentConfirmMessage = GlobalData.GetPaperPaymentStatusMessage(EPaperPaymentStatus.KonfirmasiPembayaran);

                db.Entry(makalah).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                string informationMessage = makalah.PaperPaymentConfirmMessage;
                return RedirectToAction("Index", new { InformationMessage = informationMessage });
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;

                return View("Error");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }

            base.Dispose(disposing);
        }


        #region Helpers

        private IEnumerable<ListMakalahViewModel> GetListMakalahViewModel()
        {
            string userId = User.Identity.GetUserId();
            IEnumerable<ListMakalahViewModel> listUserMakalah = from m in db.Makalah
                                                                where m.userId == userId
                                                                select new ListMakalahViewModel
                                                                {
                                                                    Id = m.Id,
                                                                    CategoryId = m.CategoryId,
                                                                    Title = m.Title,
                                                                    Abstract = m.Abstract,
                                                                    DateSubmitted = m.DateSubmitted,
                                                                    DateModified = m.DateModified,
                                                                    AbstractFileName = m.AbstractFileName,
                                                                    PaperAbstractStatus = m.PaperAbstractStatus,
                                                                    PaperAbstractConfirmMessage = m.PaperAbstractConfirmMessage,
                                                                    FullPaperFileName = m.FullPaperFileName,
                                                                    FullPaperDateSubmitted = m.DateSubmitted,
                                                                    FullPaperStatus = m.FullPaperStatus,
                                                                    FullPaperConfirmMessage = m.FullPaperConfirmMessage
                                                                };

            return listUserMakalah;
        }

        private string GetUserCompleteName()
        {
            ApplicationUser currentUser = UserManager.FindById(User.Identity.GetUserId());

            string userFullName = string.Format("{0} {1} {2} {3} {4}", currentUser.FrontTitle, currentUser.FirstName,
            currentUser.MiddleName, currentUser.LastName, currentUser.BehindTitle);

            return userFullName;
        }

        private string GetFileNameFromDateTime(DateTime dateTime, string prefix, string postfix)
        {
            string userName = User.Identity.GetUserName().PadRight(5, 'x').Substring(0, 5);
            return string.Format("{0}-{1:000}-{2:00}{3:00}{4:00}{5:00}{6:00}{7:000}{8}", prefix, userName, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond, postfix);
        }

        private int GetWordsCount(string sentence)
        {
            if (string.IsNullOrEmpty(sentence))
            {

                return 0;
            }

            return sentence.Split(' ').Where(x => x.Trim().Length > 0).Count();
        }

        #endregion
    }
}