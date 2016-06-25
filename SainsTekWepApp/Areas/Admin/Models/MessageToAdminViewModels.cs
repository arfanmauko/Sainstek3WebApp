using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SainsTekWepApp.Areas.Admin.Models
{
    public class MessageToAdminViewModel
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public string MessageTitle { get; set; }

        public string MessageContent { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateRead { get; set; }

        public string AdminReaderId { get; set; }
    }
}