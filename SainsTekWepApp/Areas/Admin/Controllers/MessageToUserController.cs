using SainsTekWepApp.Areas.Admin.Models;
using SainsTekWepApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SainsTekWepApp.Areas.Admin.Controllers
{
    public class MessageToUserController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/MessageToUser
        public ActionResult Index()
        {
            string informationMessage = Request.Params["InformationMessage"];

            if (!string.IsNullOrEmpty(informationMessage))
            {
                ViewBag.InformationMessage = informationMessage;
            }

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

            return View(messageToUser);
        }
    }
}