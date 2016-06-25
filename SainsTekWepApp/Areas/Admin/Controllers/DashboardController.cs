using Microsoft.AspNet.Identity.EntityFramework;
using SainsTekWepApp.Areas.Admin.Models;
using SainsTekWepApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SainsTekWepApp.Areas.Admin.Controllers
{
    [Authorize(Roles="Admin")]
    public class DashboardController : Controller
    {

        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin/Dashboard
        public ActionResult Index()
        {
            DashboardViewModels dashboardViewModels = new DashboardViewModels();

            try
            {

                dashboardViewModels.NewPaperAbstractCount = db.Makalah.Where(x => x.PaperAbstractStatus == Enums.EPaperAbstractStatus.Baru).Count();
                dashboardViewModels.NewPaperAbstractEditCount = db.Makalah.Where(x => x.PaperAbstractStatus == Enums.EPaperAbstractStatus.Edit).Count();

                dashboardViewModels.NewFullPaperCount = db.Makalah.Where(x => x.FullPaperStatus == Enums.EFullPaperStatus.Baru).Count();
                dashboardViewModels.NewFullPaperEditCount = db.Makalah.Where(x => x.FullPaperStatus == Enums.EFullPaperStatus.Edit).Count();


                var role = db.Roles.SingleOrDefault(m => m.Name == "Peserta");
                dashboardViewModels.NewParticipantCount = db.Users.Where(m => m.Roles.All(r => r.RoleId == role.Id)).Count();
                dashboardViewModels.NewParticipantPaymentConfirmationCount = db.Users.Where(x => x.ParticipantPaymentStatus == Enums.EParticipantPaymentStatus.KonfirmasiPembayaran).Count();

                dashboardViewModels.NewPaperPaymentConfirmationCount = db.Makalah.Where(x => x.PaperPaymentStatus == Enums.EPaperPaymentStatus.KonfirmasiPembayaran).Count();

                dashboardViewModels.NewMessagesCount = db.PesanPublikToAdmin.Where(x => x.DateAdminRead == null).Count();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }

            return View(dashboardViewModels);
        }
    }
}