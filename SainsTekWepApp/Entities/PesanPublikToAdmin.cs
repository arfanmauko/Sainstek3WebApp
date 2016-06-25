using SainsTekWepApp.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SainsTekWepApp.Entities
{
    [Table("PublikPesanToAdmin")]
    public class PesanPublikToAdmin : IPublikPesanToAdmin
    {
        public int Id { get; set; }

        public string SenderName { get; set; }

        public string SenderEmail { get; set; }

        public string MessageContent { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateAdminRead { get; set; }

        public string ReplyMessage { get; set; }

        public DateTime? DateReply { get; set; }

        public string ReplyAdminId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public PesanPublikToAdmin()
        {
            DateCreated = DateTime.UtcNow;
        }
    }
}