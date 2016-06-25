using SainsTekWepApp.Areas.Admin.Models;
using SainsTekWepApp.Data;
using System.Linq;
using System.Web.Mvc;

namespace SainsTekWepApp.Areas.Admin.Controllers
{
    public class MessageToAdminController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/MessageToAdmin
        public ActionResult Index()
        {
            string informationMessage = Request.Params["InformationMessage"];

            if (!string.IsNullOrEmpty(informationMessage))
            {
                ViewBag.InformationMessage = informationMessage;
            }

            var messageToAdmin = (from m in db.PesanUserToAdmin.AsEnumerable()
                                  select new MessageToAdminViewModel
                                  {
                                      Id = m.Id,
                                      MessageTitle = m.MessageTitle,
                                      MessageContent = m.MessageContent,
                                      DateCreated = m.DateCreated,
                                      UserId = m.UserId,
                                      DateRead = m.DateRead,
                                      AdminReaderId = m.AdminReaderId
                                  });

            return View(messageToAdmin);
        }
    }
}