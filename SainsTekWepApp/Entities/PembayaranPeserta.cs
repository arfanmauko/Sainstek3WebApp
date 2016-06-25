using SainsTekWepApp.Entities.Abstracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SainsTekWepApp.Entities
{
    [Table("PembayaranPeserta")]
    public class PembayaranPeserta : Pembayaran
    {
        public string UserId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}