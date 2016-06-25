using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SainsTekWepApp.Areas.Admin.Models
{
    public class PublicMessageViewModel
    {
        public int Id { get; set; }

        [DisplayName("Sender Name")]
        public string SenderName { get; set; }

        [DisplayName("Sender Email")]
        public string SenderEmail { get; set; }

        [DisplayName("Sender Message")]
        public string MessageContent { get; set; }

        [DisplayName("Already Reply")]
        public bool AlreadyReply { get; set; }
    }

    public class DetailPublicMessageViewModel : PublicMessageViewModel
    {
        public DateTime DateCreated { get; set; }

        public DateTime? DateAdminRead { get; set; }

        [AllowHtml]
        [StringLength(10)]
        public string ReplyMessage { get; set; }

        public DateTime? DateReply { get; set; }

        public string ReplyAdminId { get; set; }
    }

    public class ReplyPublicMessageViewModel : DetailPublicMessageViewModel
    {
        public string ReplyToEmail { get; set; }

        public string ReplyToName { get; set; }

        public string ReplySubject { get; set; }
    }
}